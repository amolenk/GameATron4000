using System;
using System.Collections.Generic;
using GameATron4000.Models.Actions;

namespace GameATron4000.Models
{
    public class ActionBuilder
    {
        private static readonly Dictionary<string, Func<List<string>, Precondition[], RoomAction>> Factories
            = new Dictionary<string, Func<List<string>, Precondition[], RoomAction>>
            {
                [ AddToInventoryAction.Name ] = (args, preconditions) => new AddToInventoryAction(args, preconditions),
                [ GuiAddRoomObjectAction.Name ] = (args, preconditions) => new GuiAddRoomObjectAction(args, preconditions),
                [ GuiDelayAction.Name ] = (args, preconditions) => new GuiDelayAction(args, preconditions),
                [ GuiMoveActorAction.Name ] = (args, preconditions) => new GuiMoveActorAction(args, preconditions),
                [ GuiPlaceActorAction.Name ] = (args, preconditions) => new GuiPlaceActorAction(args, preconditions),
                [ GuiPlaceObjectAction.Name ] = (args, preconditions) => new GuiPlaceObjectAction(args, preconditions),
                [ SetFlagAction.Name ] = (args, preconditions) => new SetFlagAction(args, preconditions),
                [ SpeakAction.Name ] = (args, preconditions) => new SpeakAction(args, preconditions),
                [ TalkToAction.Name ] = (args, preconditions) => new TalkToAction(args, preconditions),
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

        public RoomAction Build()
        {
            Func<List<string>, Precondition[], RoomAction> factory;

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