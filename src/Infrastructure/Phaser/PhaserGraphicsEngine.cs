using GameATron4000.Core.Services;
using Microsoft.JSInterop;

namespace GameATron4000.Infrastructure.Phaser
{
    public class PhaserGraphicsEngine : IGraphicsEngine
    {
        private const string FUNCTION_INIT_PHASER = "startPhaser"; // TODO

        private readonly IJSInProcessRuntime _jsRuntime;

        public PhaserGraphicsEngine(IJSRuntime jsRuntime)
        {
            _jsRuntime = (IJSInProcessRuntime)jsRuntime;
        }

        public void Initialize(string containerElement, string title)
        {
            _jsRuntime.InvokeVoid(FUNCTION_INIT_PHASER, containerElement, title);
        }
    }
}
