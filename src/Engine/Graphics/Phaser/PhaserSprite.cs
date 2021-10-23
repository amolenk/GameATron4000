using System;

namespace Amolenk.GameATron4000.Engine.Graphics.Phaser
{
    public class PhaserSprite : ISprite
    {
        private readonly PhaserSpriteInfo _info;
        private readonly IJSRuntime _jsRuntime;
        private readonly ILogger<PhaserSprite> _logger;

        private Func<Task>? _onPointerDown;

        public int Width => _info.Width;

        public int Height => _info.Height;

        public PhaserSprite(PhaserSpriteInfo info, IJSRuntime jsRuntime, ILogger<PhaserSprite> logger)
        {
            _info = info;
            _jsRuntime = jsRuntime;
            _logger = logger;
        }

        public async ValueTask OnPointerDownAsync(Func<Task> handler)
        {
            _onPointerDown = handler;

            await _jsRuntime.InvokeVoidAsync(
                PhaserConstants.Functions.SetSpriteInteraction,
                _info.Id,
                PhaserConstants.Input.Events.PointerDown,
                DotNetObjectReference.Create(this),
                nameof(OnPointerDownAsync));
        }

        public ValueTask SetAnchorAsync(double value)
        {
            throw new NotImplementedException();
        }

        [JSInvokable]
        public async Task OnPointerDownAsync()
        {
            if (_onPointerDown != null)
            {
                await _onPointerDown();
            }
        }
    }
}

