using System;
using System.Collections.Generic;
using GameATron4000.Configuration;
using GameATron4000.Models;
using GameATron4000.Scripting;
using GameATron4000.Scripting.Actions;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Configuration;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace GameATron4000.Scripting
{
    // TODO Add validations
    public class ActionFactory
    {
        private readonly GameInfo _gameInfo;

        public ActionFactory(GameInfo gameInfo)
        {
            _gameInfo = gameInfo;
        }

        public AddToInventoryAction AddToInventory(string inventoryItemId, string description,
            List<ActionPrecondition> preconditions = null)
        {
            return new AddToInventoryAction(inventoryItemId, description, preconditions);
        }

        public ClearFlagAction ClearFlag(string flag, List<ActionPrecondition> preconditions = null)
        {
            return new ClearFlagAction(flag, preconditions);
        }

        public EndConversationAction EndConversation(List<ActionPrecondition> preconditions = null)
        {
            return new EndConversationAction(preconditions);
        }

        public GoToConversationTopicAction GoToConversationTopic(string topic,
            List<ActionPrecondition> preconditions = null)
        {
            return new GoToConversationTopicAction(topic, preconditions);
        }

        public GuiDelayAction GuiDelay(int milliseconds, List<ActionPrecondition> preconditions = null)
        {
            return new GuiDelayAction(milliseconds, preconditions);
        }

        public GuiFaceActorAwayAction GuiFaceActorAway(string actorId, List<ActionPrecondition> preconditions = null)
        {
            return new GuiFaceActorAwayAction(actorId, preconditions);
        }

        public GuiFaceActorFrontAction GuiFaceActorFront(string actorId,
            List<ActionPrecondition> preconditions = null)
        {
            return new GuiFaceActorFrontAction(actorId, preconditions);
        }

        public GuiMoveActorAction GuiMoveActor(string actorId, int x, int y,
            List<ActionPrecondition> preconditions = null)
        {
            return new GuiMoveActorAction(actorId, new GamePosition(x, y), preconditions);
        }

        public GuiNarratorAction GuiNarrator(string text, List<ActionPrecondition> preconditions = null)
        {
            return new GuiNarratorAction(text, preconditions);
        }

        public GuiPlaceActorAction GuiPlaceActor(string actorId, int x, int y,
            List<ActionPrecondition> preconditions = null)
        {
            return new GuiPlaceActorAction(actorId, new GamePosition(x, y), preconditions);
        }

        public GuiPlaceObjectAction GuiPlaceObject(string objectId, int x, int y, bool foreground,
            List<ActionPrecondition> preconditions = null)
        {
            return new GuiPlaceObjectAction(objectId, new GamePosition(x, y, foreground), preconditions);
        }

        public GuiRemoveObjectAction GuiRemoveObject(string objectId, List<ActionPrecondition> preconditions = null)
        {
            return new GuiRemoveObjectAction(objectId, preconditions);
        }

        public RemoveFromInventoryAction RemoveFromInventory(string inventoryItemId,
            List<ActionPrecondition> preconditions = null)
        {
            return new RemoveFromInventoryAction(inventoryItemId, preconditions);
        }

        public SetFlagAction SetFlag(string flag, List<ActionPrecondition> preconditions = null)
        {
            return new SetFlagAction(flag, preconditions);
        }

        public SpeakAction Speak(string actorId, string text, List<ActionPrecondition> preconditions = null)
        {
            return new SpeakAction(actorId, text, preconditions);
        }

        public StartConversationAction StartConversation(string conversationId,
            List<ActionPrecondition> preconditions = null)
        {
            return new StartConversationAction(conversationId, preconditions);
        }

        public SwitchRoomAction SwitchRoom(string roomId, List<ActionPrecondition> preconditions = null)
        {
            return new SwitchRoomAction(roomId, preconditions);
        }

        public TextDescribeAction TextDescribe(string text, List<ActionPrecondition> preconditions = null)
        {
            return new TextDescribeAction(text, preconditions);
        }

        public CommandAction CreateAction(string name, List<string> args, List<ActionPrecondition> preconditions = null)
        {
            switch (name)
            {
                case AddToInventoryAction.Name:
                    return AddToInventory(args[0], args[1], preconditions);
                case ClearFlagAction.Name:
                    return ClearFlag(args[0], preconditions);
                case EndConversationAction.Name:
                    return EndConversation(preconditions);
                case GoToConversationTopicAction.Name:
                    return GoToConversationTopic(args[0], preconditions);
                case GuiDelayAction.Name:
                    return GuiDelay(int.Parse(args[0]), preconditions);
                case GuiFaceActorAwayAction.Name:
                    return GuiFaceActorAway(args[0], preconditions);
                case GuiFaceActorFrontAction.Name:
                    return GuiFaceActorFront(args[0], preconditions);
                case GuiMoveActorAction.Name:
                    return GuiMoveActor(args[0], int.Parse(args[1]), int.Parse(args[2]), preconditions);
                case GuiNarratorAction.Name:
                    return GuiNarrator(args[0], preconditions);
                case GuiPlaceActorAction.Name:
                    return GuiPlaceActor(args[0], int.Parse(args[1]), int.Parse(args[2]), preconditions);
                case GuiPlaceObjectAction.Name:
                    return GuiPlaceObject(args[0], int.Parse(args[1]), int.Parse(args[2]), args.Count > 3 && bool.Parse(args[3]), preconditions);
                case GuiRemoveObjectAction.Name:
                    return GuiRemoveObject(args[0], preconditions);
                case RemoveFromInventoryAction.Name:
                    return RemoveFromInventory(args[0], preconditions);
                case SetFlagAction.Name:
                    return SetFlag(args[0], preconditions);
                case SpeakAction.Name:
                    return Speak(args[0], args[1], preconditions);
                case StartConversationAction.Name:
                    return StartConversation(args[0], preconditions);
                case SwitchRoomAction.Name:
                    return SwitchRoom(args[0], preconditions);
                case TextDescribeAction.Name:
                    return TextDescribe(args[0], preconditions);
                default:
                    throw new ArgumentException($"Unknown action '{name}'.", "name");
            }
        }
    }
}