using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameATron4000.Infrastructure.Phaser
{
    public class PhaserGame
    {
        private readonly IPhaserInterop _phaser;

        public PhaserGame(IPhaserInterop phaser)
        {
            _phaser = phaser;
        }

        public void Start(string containerElement, string title)
        {
            _phaser.Start(containerElement, title);
        }

        //public void HandleEvent(object @event)
        //{
        //    if (_handlers.TryGetValue(@event.GetType(), out Action<object, IStateManager> handler))
        //    {
        //        handler(@event, _stateManager);
        //    }
        //}
    }
}