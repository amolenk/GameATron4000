using System;

namespace Amolenk.GameATron4000.Engine.Graphics.Phaser
{
    public class PhaserSprite : ISprite
    {

        private readonly PhaserSpriteInfo _info;
        private readonly IJSRuntime _jsRuntime;
        private readonly ILogger<PhaserSprite> _logger;

        private Func<Point, Task>? _onPointerDown;

        public string Id => _info.Id;

        public int Width => _info.Width;

        public int Height => _info.Height;

        public Point Position { get; private set; }

        public PhaserSprite(PhaserSpriteInfo info, int x, int y, IJSRuntime jsRuntime, ILogger<PhaserSprite> logger)
        {
            _info = info;
            Position = new Point(x, y);
            _jsRuntime = jsRuntime;
            _logger = logger;
        }

        public async ValueTask OnPointerDownAsync(Func<Point, Task> handler)
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
        public async Task OnPointerDownAsync(Point mousePosition)
        {
            if (_onPointerDown != null)
            {
                await _onPointerDown(mousePosition);
            }
        }

        public ITween Move(
            Point target,
            int duration,
            Action<Point> onUpdate,
            Action<Point> onComplete)
        {
            IJSInProcessRuntime js = (IJSInProcessRuntime)_jsRuntime;

            // TODO Rename to PhaserSpriteTween
            return PhaserTween.MoveSprite(this, target, duration, onUpdate, onComplete, js);
        }
    }
}

