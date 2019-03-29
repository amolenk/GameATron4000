using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameATron4000.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using GameATron4000.Core;

namespace GameATron4000.Dialogs
{
    // TODO For the love of god, cache the GameInfo (state) and Lua objects (field)
    public class Room : Dialog
    {
        private readonly string _roomId;
        private readonly IStatePropertyAccessor<Dictionary<string, RoomState>> _roomStateAccessor;
        private readonly IStatePropertyAccessor<List<string>> _inventoryItemsAccessor;
        private readonly IStatePropertyAccessor<Dictionary<string, object>> _customStateAccessor;

        public Room(
            string roomId,
            IStatePropertyAccessor<Dictionary<string, RoomState>> roomStateAccessor,
            IStatePropertyAccessor<List<string>> inventoryItemsAccessor,
            IStatePropertyAccessor<Dictionary<string, object>> customStateAccessor)
            : base(roomId)
        {
            _roomId = roomId;
            _roomStateAccessor = roomStateAccessor;
            _inventoryItemsAccessor = inventoryItemsAccessor;
            _customStateAccessor = customStateAccessor;
        }

        public override async Task<DialogTurnResult> BeginDialogAsync(
            DialogContext dc,
            object options = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            var scriptRunner = await ScriptRunner.CreateAsync(_roomId, _roomStateAccessor,
                _inventoryItemsAccessor, _customStateAccessor, dc.Context);

            await scriptRunner.EnterRoomAsync();

            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        public override async Task<DialogTurnResult> ContinueDialogAsync(
            DialogContext dc,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            // The activity text contains the player command.
            var scriptRunner = await ScriptRunner.CreateAsync(_roomId, _roomStateAccessor,
                _inventoryItemsAccessor, _customStateAccessor, dc.Context);

            var nextDialogId = await scriptRunner.RunCommandAsync(dc.Context.Activity.Text);

            if (nextDialogId.Length > 0)
            {
                await dc.ReplaceDialogAsync(nextDialogId);
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

            // // We've just finished a conversation with an actor. Check the state if the game script
            // // contains any further actions to execute.
            // object pendingActions;
            // if (dc.ActiveDialog.State.TryGetValue(DialogStatePendingActions, out pendingActions))
            // {
            //     // Map the actions to Bot Framework activities and send them to the client.
            //     if (!await ExecuteActionsAsync(dc, (List<CommandAction>)pendingActions))
            //     {
            //         // If ExecuteActionsAsync returns false, don't send anymore activities
            //         // to the client. Control is handed over to a different dialog.
            //         return new DialogTurnResult(DialogTurnStatus.Waiting);
            //     }

            //     dc.ActiveDialog.State.Remove(DialogStatePendingActions);
            // }

            // // Send an Idle activity to let the GUI know that we're waiting for user interaction.
            // await dc.Context.SendActivityAsync(_activityFactory.Idle(dc));

            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }
    }
}