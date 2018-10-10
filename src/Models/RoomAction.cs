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
        public RoomAction(Precondition[] preconditions)
        {
            this.Preconditions = preconditions;
        }
        
        public Precondition[] Preconditions { get; }

        public abstract string Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state);

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