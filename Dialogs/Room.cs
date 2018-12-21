using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameATron4000.Models;
using GameATron4000.Models.Actions;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;

namespace GameATron4000.Dialogs
{
    public class Room : Dialog
    {
        private const string DialogStatePendingActions = "PendingActions";

        private readonly string _roomId;
        private readonly List<Command> _commands;
        private readonly GameInfo _gameInfo;
        private readonly IStatePropertyAccessor<GameFlags> _gameFlagsAccessor;
        private readonly IStatePropertyAccessor<Dictionary<string, RoomState>> _roomStateAccessor;
        private readonly Random _random;

        public Room(
            string roomId,
            List<Command> commands,
            GameInfo gameInfo,
            IStatePropertyAccessor<GameFlags> gameFlagsAccessor,
            IStatePropertyAccessor<Dictionary<string, RoomState>> roomStateAccessor)
            : base(roomId)
        {
            _roomId = roomId;
            _commands = commands;
            _gameInfo = gameInfo;
            _gameFlagsAccessor = gameFlagsAccessor;
            _roomStateAccessor = roomStateAccessor;
            _random = new Random();
        }

        public override async Task<DialogTurnResult> BeginDialogAsync(
            DialogContext dc,
            object options = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            // When the player enters a new room, send a bunch of activities to
            // populate the room. Start with a RoomInitializationStarted event
            // activity to let the client now we're populating the room.
            var actions = new List<CommandAction>();
            actions.Add(new GuiRoomInitializationStartedAction(_roomId));

            // We load the object and actor positions from the room state.
            var roomState = await GetRoomStateAsync(dc.Context);

            foreach (var objectPlacement in roomState.ObjectPlacements)
            {
                actions.Add(new GuiPlaceObjectAction(objectPlacement.Key, objectPlacement.Value));
            }

            foreach (var actorPlacement in roomState.ActorPlacements)
            {
                actions.Add(new GuiPlaceActorAction(actorPlacement.Key, actorPlacement.Value));
            }

            // Let the client know we're done populating the room.
            actions.Add(new GuiRoomInitializationCompletedAction());

            // There may be some actions that must be performed immediately after the player
            // enters the room (e.g. walk to a specific spot). Add those actions to the list as well.
            actions.AddRange(await GetActionsAsync(dc, Command.EnterRoom));

            // Map the actions to Bot Framework activities and send them to the client.
            await ExecuteActionsAsync(dc, actions);

            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        public override async Task<DialogTurnResult> ContinueDialogAsync(
            DialogContext dc,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            // The activity text contains the player command.
            var command = dc.Context.Activity.Text;

            // Get the actions for the command from the game script.
            var actions = await GetActionsAsync(dc, command);

            // If there are no actions; reply with a standard response.
            if (!actions.Any())
            {
                actions = new CommandAction[]
                {
                    new SpeakAction(
                        _gameInfo.BadCommandResponses[_random.Next(0, _gameInfo.BadCommandResponses.Length)],
                        _gameInfo.PlayerActor)
                };
            }

            // Map the actions to Bot Framework activities and send them to the client.
            await ExecuteActionsAsync(dc, actions);

            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        public override async Task<DialogTurnResult> ResumeDialogAsync(
            DialogContext dc,
            DialogReason reason,
            object result = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            // We've just finished a conversation with an actor. Check the state if the game script
            // contains any further actions to execute.
            object pendingActions;
            if (dc.ActiveDialog.State.TryGetValue(DialogStatePendingActions, out pendingActions))
            {
                // Map the actions to Bot Framework activities and send them to the client.
                await ExecuteActionsAsync(dc, (List<CommandAction>)pendingActions);
                dc.ActiveDialog.State.Remove(DialogStatePendingActions);
            }

            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        private async Task<IEnumerable<CommandAction>> GetActionsAsync(DialogContext dc, string commandText)
        {
            var gameFlags = await _gameFlagsAccessor.GetAsync(dc.Context, () => new GameFlags());

            // Try to find a matching command for the player's input.
            var command = _commands
                .Where(cmd => string.Equals(cmd.Text, commandText, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            // If we've found a matching command, return all actions for which the preconditions can be
            // satisfied.
            return (command != null)
                ? command.Actions.Where(a => a.Preconditions.All(pc => gameFlags.SatisfyPrecondition(pc)))
                : Enumerable.Empty<CommandAction>();
        }

        private async Task ExecuteActionsAsync(DialogContext dc, IEnumerable<CommandAction> actions)
        {
            var gameFlags = await _gameFlagsAccessor.GetAsync(dc.Context, () => new GameFlags());

            var activities = new List<IActivity>();
            var actionStack = new Stack<CommandAction>(actions.Reverse());
            var roomState = await GetRoomStateAsync(dc.Context);
            
            // Process each action in turn, populating the activities list.
            CommandAction action;
            while (actionStack.TryPop(out action))
            {
                switch (action)
                {
                    case AddToInventoryAction addToInventory:
                    {
                        gameFlags.SetFlag(addToInventory.InventoryItemId);

                        activities.Add(CreateEventActivity(dc, "InventoryItemAdded", new
                        {
                            inventoryItemId = addToInventory.InventoryItemId,
                            description = addToInventory.Description
                        }));
                        break;
                    }
                    case ClearFlagAction clearFlag:
                    {
                        gameFlags.ClearFlag(clearFlag.FlagName);
                        break;
                    }
                    case GuiDelayAction guiDelay:
                    {
                        activities.Add(CreateEventActivity(dc, "Delayed", new
                        {
                            time = guiDelay.Milliseconds
                        }));
                        break;
                    }
                    case GuiFaceActorAwayAction guiFaceActorAway:
                    {
                        activities.Add(CreateEventActivity(dc, "ActorFacedAway", new
                        {
                            actorId = guiFaceActorAway.ActorId
                        }));
                        break;
                    }
                    case GuiFaceActorFrontAction guiFaceActorFront:
                    {
                        activities.Add(CreateEventActivity(dc, "ActorFacedFront", new
                        {
                            actorId = guiFaceActorFront.ActorId
                        }));
                        break;
                    }
                    case GuiMoveActorAction guiMoveActor:
                    {
                        activities.Add(CreateEventActivity(dc, "ActorMoved", new
                        {
                            actorId = guiMoveActor.ActorId,
                            x = guiMoveActor.X,
                            y = guiMoveActor.Y
                        }));
                        break;
                    }
                    case GuiNarratorAction guiNarrator:
                    {
                        activities.Add(CreateEventActivity(dc, "Narrated", new
                        {
                            text = guiNarrator.Text
                        }));
                        break;
                    }
                    case GuiPlaceActorAction guiPlaceActor:
                    {
                        // TODO If ations can validate themselves, we don't need this check here!
                        if (_gameInfo.Actors.TryGetValue(guiPlaceActor.ActorId, out GameActor gameActor))
                        {
                            // Save the actor's new position in the room state.
                            roomState.ActorPlacements[guiPlaceActor.ActorId] = guiPlaceActor.Placement;

                            activities.Add(CreateEventActivity(dc, "ActorPlacedInRoom", new
                            {
                                actorId = guiPlaceActor.ActorId,
                                description = gameActor.Description,
                                x = guiPlaceActor.Placement.X,
                                y = guiPlaceActor.Placement.Y,
                                textColor = gameActor.TextColor
                            }));
                        }
                        else
                        {
                            // TODO Specialized exception
                            throw new Exception($"Unknown actor id '{guiPlaceActor.ActorId}'.");
                        }
                        break;
                    }
                    case GuiPlaceObjectAction guiPlaceObject:
                    {
                        if (_gameInfo.Objects.TryGetValue(guiPlaceObject.ObjectId, out GameObject gameObject))
                        {
                            // Save the object's new position in the room state.
                            // TODO If ations can validate themselves, we don't need this check here!
                            if (!roomState.ObjectPlacements.ContainsKey(guiPlaceObject.ObjectId))
                            {
                                roomState.ObjectPlacements.Add(guiPlaceObject.ObjectId, guiPlaceObject.Placement);
                            }
                            else
                            {
                                roomState.ObjectPlacements[guiPlaceObject.ObjectId] = guiPlaceObject.Placement;
                            }

                            activities.Add(CreateEventActivity(dc, "ObjectPlacedInRoom", new
                            {
                                objectId = guiPlaceObject.ObjectId,
                                description = gameObject.Description,
                                x = guiPlaceObject.Placement.X,
                                y = guiPlaceObject.Placement.Y,
                                foreground = guiPlaceObject.Placement.Foreground
                            }));
                        }
                        else
                        {
                            // TODO Specialized exception
                            throw new Exception($"Unknown object id '{guiPlaceObject.ObjectId}'.");
                        }
                        break;
                    }
                    case GuiRemoveObjectAction guiRemoveObject:
                    {
                        activities.Add(CreateEventActivity(dc, "ObjectRemovedFromRoom", new
                        {
                            objectId = guiRemoveObject.ObjectId
                        }));
                        break;
                    }
                    case GuiRoomInitializationCompletedAction guiRoomInitializationCompleted:
                    {
                        activities.Add(CreateEventActivity(dc, "RoomInitializationCompleted"));
                        break;
                    }
                    case GuiRoomInitializationStartedAction guiRoomInitializationStarted:
                    {
                        activities.Add(CreateEventActivity(dc, "RoomInitializationStarted", new
                        {
                            roomId = guiRoomInitializationStarted.RoomId
                        }));
                        break;
                    }
                    case RemoveFromInventoryAction removeFromInventory:
                    {
                        gameFlags.ClearFlag(removeFromInventory.InventoryItemId);
                        
                        activities.Add(CreateEventActivity(dc, "InventoryItemRemoved", new
                        {
                            inventoryItemId = removeFromInventory.InventoryItemId
                        }));
                        break;
                    }
                    case SetFlagAction setFlag:
                    {
                        gameFlags.SetFlag(setFlag.FlagName);
                        break;
                    }
                    case SpeakAction speak:
                    {
                        var gameActor = _gameInfo.Actors[speak.ActorId];

                        var messageActivity = MessageFactory.Text($"{gameActor.Description} > {speak.Text}");
                        messageActivity.Properties = JObject.FromObject(new {
                            actorId = speak.ActorId
                        });

                        activities.Add(messageActivity);
                        break;
                    }
                    case StartConversationAction startConversation:
                    {
                        // If the result of the action indicates that we need to start a new dialog
                        // (e.g. conversation with an actor, or a switch to a new room), first send all
                        // activities collected up to this point to the client.
                        if (activities.Any())
                        {
                            await dc.Context.SendActivitiesAsync(activities.ToArray());
                        }

                        // If the player starts a conversation, save any remaining actions to the state.
                        // These will be executed when the conversation is done and this dialog
                        // continues.
                        if (actionStack.Any())
                        {
                            // Conver to List because Stacks don't serialize in the correct order.
                            // See https://github.com/JamesNK/Newtonsoft.Json/issues/971.
                            dc.ActiveDialog.State.Add(DialogStatePendingActions, actionStack.ToList());
                        }

                        // Start the conversation.
                        await dc.BeginDialogAsync(startConversation.ConversationId);

                        // Stop processing any further actions now that we've switched to a new dialog.
                        return;
                    }
                    case SwitchRoomAction switchRoom:
                    {
                        // If the result of the action indicates that we need to start a new dialog
                        // (e.g. conversation with an actor, or a switch to a new room), first send all
                        // activities collected up to this point to the client.
                        if (activities.Any())
                        {
                            await dc.Context.SendActivitiesAsync(activities.ToArray());
                        }

                        // Switch to the new room.
                        await dc.ReplaceDialogAsync(switchRoom.RoomId);

                        // Stop processing any further actions now that we've switched to a new dialog.
                        return;
                    }
                    case TextDescribeAction textDescribe:
                    {
                        activities.Add(MessageFactory.Text(textDescribe.Text));
                        break;
                    }
                }
            }

            // Add an Idle activity to let the GUI know that we're waiting for user interaction.
            activities.Add(CreateEventActivity(dc, "Idle"));

            await dc.Context.SendActivitiesAsync(activities.ToArray());
        }

        private async Task<RoomState> GetRoomStateAsync(ITurnContext context)
        {
            var roomStates = await _roomStateAccessor.GetAsync(context, () => new Dictionary<string, RoomState>());
            if (!roomStates.ContainsKey(_roomId))
            {
                if (_gameInfo.InitialRoomStates.ContainsKey(_roomId))
                {
                    roomStates.Add(_roomId, _gameInfo.InitialRoomStates[_roomId]);
                }
                else
                {
                    roomStates.Add(_roomId, new RoomState());
                }
            }

            return roomStates[_roomId];
        }

        private static Activity CreateEventActivity(DialogContext dc, string name, object properties = null)
        {
            var eventActivity = dc.Context.Activity.CreateReply();
            eventActivity.Type = "event";
            eventActivity.Name = name;

            if (properties != null)
            {
                eventActivity.Properties = JObject.FromObject(properties);
            }

            return eventActivity;
        }
    }
}