using System;
using System.Collections.Generic;
using GameATron4000.Models.Actions;

namespace GameATron4000.Models
{
    public class ActionBuilder
    {
        private static readonly Dictionary<string, Func<List<string>, Precondition[], CommandAction>> Factories
            = new Dictionary<string, Func<List<string>, Precondition[], CommandAction>>
            {
                [ AddToInventoryAction.Name ] = (args, preconditions) => new AddToInventoryAction(args, preconditions),
                [ ClearFlagAction.Name ] = (args, preconditions) => new ClearFlagAction(args, preconditions),
                [ EndConversationAction.Name ] = (args, preconditions) => new EndConversationAction(args, preconditions),
                [ GoToConversationTopicAction.Name ] = (args, preconditions) => new GoToConversationTopicAction(args, preconditions),
                [ GuiCloseCloseUpAction.Name ] = (args, preconditions) => new GuiCloseCloseUpAction(args, preconditions),
                [ GuiDelayAction.Name ] = (args, preconditions) => new GuiDelayAction(args, preconditions),
                [ GuiFaceActorAwayAction.Name ] = (args, preconditions) => new GuiFaceActorAwayAction(args, preconditions),
                [ GuiFaceActorFrontAction.Name ] = (args, preconditions) => new GuiFaceActorFrontAction(args, preconditions),
                [ GuiMoveActorAction.Name ] = (args, preconditions) => new GuiMoveActorAction(args, preconditions),
                [ GuiOpenCloseUpAction.Name ] = (args, preconditions) => new GuiOpenCloseUpAction(args, preconditions),
                [ GuiPlaceActorAction.Name ] = (args, preconditions) => new GuiPlaceActorAction(args, preconditions),
                [ GuiPlaceObjectAction.Name ] = (args, preconditions) => new GuiPlaceObjectAction(args, preconditions),
                [ GuiRemoveObjectAction.Name ] = (args, preconditions) => new GuiRemoveObjectAction(args, preconditions),
                [ RemoveFromInventoryAction.Name] = (args, preconditions) => new RemoveFromInventoryAction(args, preconditions),
                [ SetFlagAction.Name ] = (args, preconditions) => new SetFlagAction(args, preconditions),
                [ SpeakAction.Name ] = (args, preconditions) => new SpeakAction(args, preconditions),
                [ StartConversationAction.Name ] = (args, preconditions) => new StartConversationAction(args, preconditions),
                [ SwitchRoomAction.Name ] = (args, preconditions) => new SwitchRoomAction(args, preconditions),
                [ TextDescribeAction.Name ] = (args, preconditions) => new TextDescribeAction(args, preconditions)
            };

        private string _name;
        private List<string> _args;
        private List<Precondition> _preconditions;

        public ActionBuilder()
        {
            _args = new List<string>();
            _preconditions = new List<Precondition>();
        }

        public ActionBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ActionBuilder WithArgument(string arg)
        {
            _args.Add(arg);
            return this;
        }

        public ActionBuilder WithArguments(IEnumerable<string> args)
        {
            _args = new List<string>();

            if (args != null)
            {
                _args.AddRange(args);
            }
            return this;
        }

        public ActionBuilder WithPreconditions(IEnumerable<Precondition> preconditions)
        {
            _preconditions = new List<Precondition>();

            if (preconditions != null)
            {
                _preconditions.AddRange(preconditions);
            }
            return this;
        }

        // TODO
        // public bool IsValid()
        // {
        //     switch (_name)
        //     {
        //         case Models.Action.AddAchievement:
        //         case Models.Action.AddToInventory:
        //             return _args.Count == 1;

        //         case Models.Action.EndConversation:
        //             return _args.Count == 0;

        //         case Models.Action.GoToConversationTopic:
        //             return _args.Count == 1
        //                 && (string.Equals(_args[0], "parent", StringComparison.OrdinalIgnoreCase)
        //                 || string.Equals(_args[0], "root", StringComparison.OrdinalIgnoreCase));

        //         case Models.Action.GuiAddRoomObject:
        //             return (_args.Count == 3 || _args.Count == 4);
                    
        //         default:
        //             return false;
        //     }

        // }

        public CommandAction Build()
        {
            Func<List<string>, Precondition[], CommandAction> factory;

            if (Factories.TryGetValue(_name, out factory))
            {
                return factory(_args, _preconditions.ToArray());
            }
            else
            {
                // TODO GameEx
                throw new Exception($"Unknown action '{_name}'.");
            }
        }
    }
}