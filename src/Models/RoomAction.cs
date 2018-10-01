using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;

namespace GameATron4000.Models
{
    // tODO Rename to CommandAction???
    public abstract class RoomAction
    {
        // TODO
        // public const string AddAchievement = "AddAchievement";
        // public const string AddToInventory = "AddToInventory";
        // public const string EndConversation = "EndConversation";
        // public const string GoToConversationTopic = "GoToConversationTopic";
        // public const string GuiAddRoomObject = "Gui:AddRoomObject";
        // public const string GuiDelay = "Gui:Delay";
        // public const string GuiFaceAway = "Gui:FaceAway";
        // public const string GuiFaceFront = "Gui:FaceFront";
        // public const string GuiRemoveRoomObject = "Gui:RemoveRoomObject";
        // public const string Speak = "Speak";
        // public const string TalkTo = "TalkTo";
        // public const string TextDescribe = "Text:Describe";

        public RoomAction(Precondition[] preconditions)
        {
            this.Preconditions = preconditions;
        }
        
        public Precondition[] Preconditions { get; }

        public abstract string Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state, GameRoom roomInfo);

        protected static Activity CreateEventActivity(DialogContext dc, string name, JObject properties = null)
        {
            var eventActivity = dc.Context.Activity.CreateReply();
            eventActivity.Type = "event";
            eventActivity.Name = name;
            eventActivity.Properties = properties;
            return eventActivity;
        }
    }
}