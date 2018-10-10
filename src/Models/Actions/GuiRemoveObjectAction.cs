using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Models.Actions
{
    public class GuiRemoveObjectAction : RoomAction
    {
        public const string Name = "GUI:RemoveObject";

        private readonly string _objectId;

        public GuiRemoveObjectAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            _objectId = args[0];
        }

        public override string Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state) {

            activities.Add(CreateEventActivity(dc, "ObjectRemovedFromRoom", JObject.FromObject(new
            {
                objectId = _objectId
            })));

            return string.Empty;
        }
    }
}