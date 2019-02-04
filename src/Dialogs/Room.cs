using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameATron4000.Models;
using GameATron4000.Scripting.Actions;
using GameATron4000.Scripting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using GameATron4000.Core;

namespace GameATron4000.Dialogs
{
    public class Room : Dialog
    {
        private const string DialogStatePendingActions = "PendingActions";

        private readonly string _roomId;
        private readonly List<Command> _commands;
        private readonly GameInfo _gameInfo;
        private readonly IStatePropertyAccessor<List<string>> _inventoryItemsAccessor;
        private readonly IStatePropertyAccessor<List<string>> _stateFlagsStateAccessor;
        private readonly IStatePropertyAccessor<Dictionary<string, RoomState>> _roomStateAccessor;
        private readonly ActivityFactory _activityFactory;

        public Room(
            string roomId,
            List<Command> commands,
            GameInfo gameInfo,
            IStatePropertyAccessor<List<string>> inventoryItemsAccessor,
            IStatePropertyAccessor<List<string>> stateFlagsStateAccessor,
            IStatePropertyAccessor<Dictionary<string, RoomState>> roomStateAccessor)
            : base(roomId)
        {
            _roomId = roomId;
            _commands = commands;
            _gameInfo = gameInfo;
            _inventoryItemsAccessor = inventoryItemsAccessor;
            _stateFlagsStateAccessor = stateFlagsStateAccessor;
            _roomStateAccessor = roomStateAccessor;
            _activityFactory = new ActivityFactory(gameInfo);
        }

        public override async Task<DialogTurnResult> BeginDialogAsync(
            DialogContext dc,
            object options = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            // We load the object and actor positions from the room state.
            var roomStates = await _roomStateAccessor.GetAsync(dc.Context, () => new Dictionary<string, RoomState>());

            // If there's no room state found, use the initial room state.
            RoomState roomState;
            if (!roomStates.TryGetValue(_roomId, out roomState))
            {
                roomState = _gameInfo.InitialRoomStates[_roomId];
                roomStates.Add(_roomId, roomState);
            }

            // When the player enters a new room, send a RoomEntered event to 
            // populate the room.
            await dc.Context.SendActivityAsync(_activityFactory.RoomEntered(dc, _roomId, roomState));

            // There may be some actions that must be performed immediately after the player
            // enters the room (e.g. walk to a specific spot).
            var actions = await GetActionsAsync(dc, Command.EnterRoom);
            if (actions.Any())
            {
                // Map the actions to Bot Framework activities and send them to the client.
                if (!await ExecuteActionsAsync(dc, actions))
                {
                    // If ExecuteActionsAsync returns false, don't send anymore activities
                    // to the client. Control is handed over to a different dialog.
                    return new DialogTurnResult(DialogTurnStatus.Waiting);
                }
            }

            // Send an Idle activity to let the GUI know that we're waiting for user interaction.
            await dc.Context.SendActivityAsync(_activityFactory.Idle(dc));

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
            if (actions.Any())
            {
                // Map the actions to Bot Framework activities and send them to the client.
                if (!await ExecuteActionsAsync(dc, actions))
                {
                    // If ExecuteActionsAsync returns false, don't send anymore activities
                    // to the client. Control is handed over to a different dialog.
                    return new DialogTurnResult(DialogTurnStatus.Waiting);
                }
            }
            else
            {
                // If there are no actions; reply with a standard response.
                await dc.Context.SendActivityAsync(_activityFactory.CannedResponse(dc));
            }

            // Send an Idle activity to let the GUI know that we're waiting for user interaction.
            await dc.Context.SendActivityAsync(_activityFactory.Idle(dc));

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
                if (!await ExecuteActionsAsync(dc, (List<CommandAction>)pendingActions))
                {
                    // If ExecuteActionsAsync returns false, don't send anymore activities
                    // to the client. Control is handed over to a different dialog.
                    return new DialogTurnResult(DialogTurnStatus.Waiting);
                }

                dc.ActiveDialog.State.Remove(DialogStatePendingActions);
            }

            // Send an Idle activity to let the GUI know that we're waiting for user interaction.
            await dc.Context.SendActivityAsync(_activityFactory.Idle(dc));

            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        private async Task<IEnumerable<CommandAction>> GetActionsAsync(DialogContext dc, string commandText)
        {
            var stateFlags = await _stateFlagsStateAccessor.GetAsync(dc.Context, () => new List<string>());
            var inventoryItems = await _inventoryItemsAccessor.GetAsync(dc.Context);

            // Try to find a matching command for the player's input.
            var command = _commands
                .Where(cmd => string.Equals(cmd.Text, commandText, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            // If we've found a matching command, return all actions for which the preconditions can be
            // satisfied.
            return (command != null)
                ? command.Actions.Where(a => a.Preconditions.All(precondition =>
                {
                    if (precondition.Inverted)
                    {
                        return !stateFlags.Contains(precondition.FlagOrInventoryItemId)
                            && !inventoryItems.Contains(precondition.FlagOrInventoryItemId);
                    }
                    else
                    {
                        return stateFlags.Contains(precondition.FlagOrInventoryItemId)
                            || inventoryItems.Contains(precondition.FlagOrInventoryItemId);
                    }
                }))
                : Enumerable.Empty<CommandAction>();
        }

        private async Task<bool> ExecuteActionsAsync(DialogContext dc, IEnumerable<CommandAction> actions)
        {
            var stateFlags = await _stateFlagsStateAccessor.GetAsync(dc.Context);
            var inventoryItems = await _inventoryItemsAccessor.GetAsync(dc.Context);

            // Get the room state. It always exists at this point because it's
            // set up in the BeginDialogAsync method.
            var roomStates = await _roomStateAccessor.GetAsync(dc.Context, () => new Dictionary<string, RoomState>());
            var roomState = roomStates[_roomId];

            var activities = new List<IActivity>();
            var actionStack = new Stack<CommandAction>(actions.Reverse());
            
            // Process each action in turn, populating the activities list.
            while (actionStack.TryPop(out CommandAction nextAction))
            {
                switch (nextAction)
                {
                    case AddToInventoryAction action:
                    {
                        if (!inventoryItems.Contains(action.InventoryItemId))
                        {
                            inventoryItems.Add(action.InventoryItemId);

                            activities.Add(_activityFactory.InventoryItemAdded(
                                dc, action.InventoryItemId, action.Description));
                        }
                        break;
                    }
                    case ClearFlagAction action:
                    {
                        if (stateFlags.Contains(action.Flag))
                        {
                            stateFlags.Remove(action.Flag);
                        }
                        break;
                    }
                    case GuiDelayAction action:
                    {
                        activities.Add(_activityFactory.Delayed(dc, action.Milliseconds));
                        break;
                    }
                    case GuiChangeActorDirectionAction action:
                    {
                        activities.Add(_activityFactory.ActorDirectionChanged(dc,
                            action.ActorId, action.Direction));
                        break;
                    }
                    case GuiMoveActorAction action:
                    {
                        // Save the actor's new position in the room state.
                        roomState.ActorPositions[action.ActorId] = action.Position;

                        activities.Add(_activityFactory.ActorMoved(dc, action.ActorId, action.Position));
                        break;
                    }
                    case GuiNarratorAction action:
                    {
                        activities.Add(_activityFactory.Narrated(dc, action.Text));
                        break;
                    }
                    case GuiPlaceActorAction action:
                    {
                        // Save the actor's new position in the room state.
                        roomState.ActorPositions[action.ActorId] = action.Position;

                        activities.Add(_activityFactory.ActorPlacedInRoom(dc, action.ActorId, action.Position));
                        break;
                    }
                    case GuiPlaceObjectAction action:
                    {
                        // Save the object's new position in the room state.
                        roomState.ObjectPositions[action.ObjectId] = action.Position;

                        activities.Add(_activityFactory.ObjectPlacedInRoom(dc, action.ObjectId, action.Position));
                        break;
                    }
                    case GuiRemoveObjectAction action:
                    {
                        // Remove the object from the room state.
                        roomState.ObjectPositions.Remove(action.ObjectId);

                        activities.Add(_activityFactory.ObjectRemovedFromRoom(dc, action.ObjectId));
                        break;
                    }
                    case RemoveFromInventoryAction action:
                    {
                        if (inventoryItems.Contains(action.InventoryItemId))
                        {
                            inventoryItems.Remove(action.InventoryItemId);
                        }                        
                        activities.Add(_activityFactory.InventoryItemRemoved(dc, action.InventoryItemId));
                        break;
                    }
                    case SetFlagAction action:
                    {
                        if (!stateFlags.Contains(action.Flag))
                        {
                            stateFlags.Add(action.Flag);
                        }
                        break;
                    }
                    case SpeakAction action:
                    {
                        activities.Add(_activityFactory.Speak(dc, action.ActorId, action.Text));
                        break;
                    }
                    case StartConversationAction action:
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
                        await dc.BeginDialogAsync(action.ConversationId);

                        // Stop processing any further actions now that we've switched to a new dialog.
                        return false;
                    }
                    case SwitchRoomAction action:
                    {
                        // If the result of the action indicates that we need to start a new dialog
                        // (e.g. conversation with an actor, or a switch to a new room), first send all
                        // activities collected up to this point to the client.
                        if (activities.Any())
                        {
                            await dc.Context.SendActivitiesAsync(activities.ToArray());
                        }

                        // Switch to the new room.
                        await dc.ReplaceDialogAsync(action.RoomId);

                        // Stop processing any further actions now that we've switched to a new dialog.
                        return false;
                    }
                    case TextDescribeAction action:
                    {
                        activities.Add(MessageFactory.Text(action.Text));
                        break;
                    }
                }
            }

            await dc.Context.SendActivitiesAsync(activities.ToArray());

            // Returning true here indicates that we can continue processing the actions for this turn.
            return true;
        }
    }
}