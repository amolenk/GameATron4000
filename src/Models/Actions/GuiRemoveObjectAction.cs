using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Models.Actions
{
    public class GuiRemoveObjectAction : CommandAction
    {
        public const string Name = "GUI:RemoveObject";

        [JsonConstructor]
        private GuiRemoveObjectAction()
        {
        }

        public GuiRemoveObjectAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            ObjectId = args[0];
        }

        [JsonProperty]
        public string ObjectId { get; private set; }

        public override string Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state) {

            activities.Add(CreateEventActivity(dc, "ObjectRemovedFromRoom", JObject.FromObject(new
            {
                objectId = ObjectId
            })));

            return string.Empty;
        }
    }
}