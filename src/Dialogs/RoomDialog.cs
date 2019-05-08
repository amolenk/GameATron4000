using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using GameATron4000.Core;
using GameATron4000.Models;
using GameATron4000.Scripting;

namespace GameATron4000.Dialogs
{
    public class RoomDialog : Dialog
    {
        private readonly IGameScript _script;

        public RoomDialog(string roomId, IGameScript script)
            : base(roomId)
        {
            _script = script;
        }

        public override async Task<DialogTurnResult> BeginDialogAsync(
            DialogContext dc,
            object options = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            var activityFactory = new ActivityFactory(dc.Context);
            var activities = new List<Activity>();

            // Run the optional script commands to set up the room. At this point,
            // the room is not visible yet to the player.
            _script.OnBeforeEnterRoom();

            // Create a RoomEntered event activity to let the client know that the
            // player has entered a new room.
            var roomEntered = activityFactory.RoomEntered(_script);
            activities.Add(roomEntered);

            // Run the optional script commands to immediately take some action when
            // the player has entered the room. At this point, the room is visible
            // to the player.
            var scriptResult = _script.OnAfterEnterRoom();
            activities.AddRange(scriptResult.Activities);

            // // Add an Idle activity to let the GUI know that we're waiting for the
            // // player's input.
            // activities.Add(activityFactory.Idle());

            // Send all collected activities to the client.
            await dc.Context.SendActivitiesAsync(activities.ToArray());

            // TODO Below part can be extracted to separate method ''
            // If the script tells us to go to another room, or start a conversation, 
            // update the dialog stack.
            if (scriptResult.NextDialogId.Length > 0)
            {
                if (scriptResult.NextDialogReplace)
                {
                    await dc.ReplaceDialogAsync(scriptResult.NextDialogId);
                }
                else
                {
                    await dc.BeginDialogAsync(scriptResult.NextDialogId);
                }
            }
            // Otherwise, send an Idle activity to let the GUI know that we're waiting
            // for the player's input.
            else
            {
                await dc.Context.SendActivityAsync(activityFactory.Idle());
            }

            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        public override async Task<DialogTurnResult> ContinueDialogAsync(
            DialogContext dc,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            var activityFactory = new ActivityFactory(dc.Context);

            // The activity text contains the player's input.
            var input = dc.Context.Activity.Text;

            // Execute the player's input and retrieve the result from the script.
            var scriptResult = _script.OnPlayerInput(input);

            // If there are any activities, send them to the client.
            if (scriptResult.Activities.Any())
            {
                await dc.Context.SendActivitiesAsync(scriptResult.Activities.ToArray());
            }

            // If the script tells us to go to another room, or start a conversation, 
            // update the dialog stack.
            if (scriptResult.NextDialogId.Length > 0)
            {
                await StartNextDialog(dc, scriptResult);
            }
            // Otherwise, send an Idle activity to let the GUI know that we're waiting
            // for the player's input.
            else
            {
                await dc.Context.SendActivityAsync(activityFactory.Idle());
            }

            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        public override async Task<DialogTurnResult> ResumeDialogAsync(
            DialogContext dc,
            DialogReason reason,
            object result = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            if (result is IGameScriptResult scriptResult
                && scriptResult.NextDialogId.Length > 0)
            {
                await StartNextDialog(dc, scriptResult);
            }
            else
            {
                var activityFactory = new ActivityFactory(dc.Context);

                // We've just finished a conversation with an actor. 
                // Send an Idle activity to let the GUI know that we're waiting for user interaction.
                await dc.Context.SendActivityAsync(activityFactory.Idle());
            }

            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        private async Task StartNextDialog(DialogContext dc, IGameScriptResult scriptResult)
        {
            if (scriptResult.NextDialogReplace)
            {
                await dc.ReplaceDialogAsync(scriptResult.NextDialogId);
            }
            else
            {
                await dc.BeginDialogAsync(scriptResult.NextDialogId);
            }
        }
    }
}