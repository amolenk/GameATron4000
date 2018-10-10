using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Models.Actions
{
    public class GuiOpenCloseUpAction : RoomAction
    {
        public const string Name = "GUI:OpenCloseUp";

        private readonly string _closeUpId;

        public GuiOpenCloseUpAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            _closeUpId = args[0];
        }

        public override string Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state) {

            activities.Add(CreateEventActivity(dc, "CloseUpOpened", JObject.FromObject(new
            {
                closeUpId = _closeUpId
            })));

            return string.Empty;
        }
    }
}