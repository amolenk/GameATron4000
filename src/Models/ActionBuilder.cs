using System;
using System.Collections.Generic;

namespace GameATron4000.Models
{
    public class ActionBuilder
    {
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

        public bool IsValid()
        {
            switch (_name)
            {
                case Models.Action.AddAchievement:
                case Models.Action.AddToInventory:
                    return _args.Count == 1;

                case Models.Action.EndConversation:
                    return _args.Count == 0;

                case Models.Action.GoToConversationTopic:
                    return _args.Count == 1
                        && (string.Equals(_args[0], "parent", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(_args[0], "root", StringComparison.OrdinalIgnoreCase));

                default:
                    return false;
            }

        }

        public Models.Action Build()
        {
            return new Models.Action(_name, _args.ToArray(), _preconditions.ToArray());
        }
    }
}