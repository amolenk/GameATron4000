using System;
using System.Collections.Generic;
using System.Linq;

namespace GameATron4000.Models
{
    public class Action
    {
        public const string AddAchievement = "AddAchievement";
        public const string AddToInventory = "AddToInventory";
        public const string EndConversation = "EndConversation";
        public const string GoToConversationTopic = "GoToConversationTopic";
        public const string Speak = "Speak";
        public const string TalkTo = "TalkTo";
        public const string TextDescribe = "Text:Describe";

        public Action(string name, string[] args, Precondition[] preconditions)
        {
            this.Name = name;
            this.Args = args;
            this.Preconditions = preconditions;
        }

        public string Name { get; } 

        public string[] Args { get; }
        
        public Precondition[] Preconditions { get; }
    }
}