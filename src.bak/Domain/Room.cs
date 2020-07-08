using System.Threading.Tasks;
using GameATron4000.Core;
using MediatR;

namespace GameATron4000.Domain
{
    public class Room
    {
        private readonly IMediator _mediator;

        public Room(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task EnterAsync(string roomId)
        {
            // Publish a BeforeRoomEntered event to let subscribers know the player is about to enter a new room.
            await _mediator.Send(new OnBeforeEnterRoomCommand(roomId));

            // TODO Do all the Phaser setup for the new room here.

            // Publish a AfterRoomEntered event to let subscribers know the player has entered a new room.
            await _mediator.Send(new OnAfterEnterRoomCommand(roomId));

            // // TODO Below part can be extracted to separate method ''
            // // If the script tells us to go to another room, or start a conversation, 
            // // update the dialog stack.
            // if (scriptResult.NextDialogId.Length > 0)
            // {
            //     if (scriptResult.NextDialogReplace)
            //     {
            //         await dc.ReplaceDialogAsync(scriptResult.NextDialogId);
            //     }
            //     else
            //     {
            //         await dc.BeginDialogAsync(scriptResult.NextDialogId);
            //     }
            // }
            // // Otherwise, send an Idle activity to let the GUI know that we're waiting
            // // for the player's input.
            // else
            // {
            //     await dc.Context.SendActivityAsync(activityFactory.Idle());
            // }
        }

        // public override async Task<DialogTurnResult> ContinueDialogAsync(
        //     DialogContext dc,
        //     CancellationToken cancellationToken = default(CancellationToken))
        // {
        //     if (dc == null) throw new ArgumentNullException(nameof(dc));

        //     var activityFactory = new ActivityFactory(dc.Context);

        //     // The activity text contains the player's input.
        //     var input = dc.Context.Activity.Text;

        //     // Execute the player's input and retrieve the result from the script.
        //     var scriptResult = _script.OnPlayerInput(input);

        //     // If there are any activities, send them to the client.
        //     if (scriptResult.Activities.Any())
        //     {
        //         await dc.Context.SendActivitiesAsync(scriptResult.Activities.ToArray());
        //     }

        //     // If the script tells us to go to another room, or start a conversation, 
        //     // update the dialog stack.
        //     if (scriptResult.NextDialogId.Length > 0)
        //     {
        //         await StartNextDialog(dc, scriptResult);
        //     }
        //     // Otherwise, send an Idle activity to let the GUI know that we're waiting
        //     // for the player's input.
        //     else
        //     {
        //         await dc.Context.SendActivityAsync(activityFactory.Idle());
        //     }

        //     return new DialogTurnResult(DialogTurnStatus.Waiting);
        // }

        // public override async Task<DialogTurnResult> ResumeDialogAsync(
        //     DialogContext dc,
        //     DialogReason reason,
        //     object result = null,
        //     CancellationToken cancellationToken = default(CancellationToken))
        // {
        //     if (dc == null) throw new ArgumentNullException(nameof(dc));

        //     if (result is IGameScriptResult scriptResult
        //         && scriptResult.NextDialogId.Length > 0)
        //     {
        //         await StartNextDialog(dc, scriptResult);
        //     }
        //     else
        //     {
        //         var activityFactory = new ActivityFactory(dc.Context);

        //         // We've just finished a conversation with an actor. 
        //         // Send an Idle activity to let the GUI know that we're waiting for user interaction.
        //         await dc.Context.SendActivityAsync(activityFactory.Idle());
        //     }

        //     return new DialogTurnResult(DialogTurnStatus.Waiting);
        // }

        // private async Task StartNextDialog(DialogContext dc, IGameScriptResult scriptResult)
        // {
        //     if (scriptResult.NextDialogReplace)
        //     {
        //         await dc.ReplaceDialogAsync(scriptResult.NextDialogId);
        //     }
        //     else
        //     {
        //         await dc.BeginDialogAsync(scriptResult.NextDialogId);
        //     }
        // }
    }
}