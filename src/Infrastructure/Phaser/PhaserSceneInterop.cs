//using Microsoft.JSInterop;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GameATron4000.Infrastructure.Phaser
//{
//    public class PhaserSceneInterop : IPhaserSceneInterop
//    {
//        private readonly IJSInProcessRuntime _jsRuntime;
//        private readonly Scene _scene;

//        public PhaserSceneInterop(IJSInProcessRuntime jsRuntime, Scene scene)
//        {
//            _jsRuntime = jsRuntime;
//            _scene = scene;
//        }

//        public bool IsVisible()
//            => _jsRuntime.Invoke<bool>("isSceneVisible", _scene.GetName());

//        public IPhaserSceneInterop AddSprite(string name, string imageName, double x, double y, Action<IPhaserSpriteInterop> options)
//        {
//            _jsRuntime.InvokeVoid("addSprite", _scene.GetName(), name, imageName, x, y, 1);

//            if (options != null)
//            {
//                options(new PhaserSpriteInterop(_jsRuntime, _scene.GetName(), name));
//            }

//            return this;
//        }

//        public IPhaserSceneInterop AddText(string name, double x, double y, string text, int fontSize, string color, string style)
//        {
//            _jsRuntime.InvokeVoid("addText", _scene.GetName(), name, x, y, text, $"{fontSize}px", "black", style);
//            return this;
//        }

//        public IPhaserSceneInterop AddText(string name, double x, double y, string text, Action<IPhaserTextInterop> options)
//        {
//            _jsRuntime.InvokeVoid("addText", _scene.GetName(), name, x, y, text, "24px", "black", null);

//            if (options != null)
//            {
//                options(new PhaserTextInterop(_jsRuntime, _scene.GetName(), name));
//            }

//            return this;
//        }

//        public IPhaserSceneInterop AddFireworks()
//        {
//            _jsRuntime.InvokeVoid("addFireworks", _scene.GetName());
//            return this;
//        }

//        public IPhaserSceneInterop RemoveSprite(string name)
//        {
//            _jsRuntime.InvokeVoid("removeSprite", _scene.GetName(), name);
//            return this;
//        }

//        public IPhaserSpriteInterop Sprite(string name)
//        {
//            return new PhaserSpriteInterop(_jsRuntime, _scene.GetName(), name);
//        }

//        public IPhaserTextInterop Text(string name)
//        {
//            return new PhaserTextInterop(_jsRuntime, _scene.GetName(), name);
//        }

//        public IPhaserSceneInterop OnPointerMove(string handlerName)
//        {
//            _jsRuntime.InvokeVoid("addSceneEventHandler", _scene.GetName(), "pointermove", handlerName);
//            return this;
//        }

//        public IPhaserSceneInterop OnPointerDown(string handlerName)
//        {
//            _jsRuntime.InvokeVoid("addSceneEventHandler", _scene.GetName(), "pointerdown", handlerName);
//            return this;
//        }

//        public IPhaserSceneInterop OnPointerUp(string handlerName)
//        {
//            _jsRuntime.InvokeVoid("addSceneEventHandler", _scene.GetName(), "pointerup", handlerName);
//            return this;
//        }

//        public IPhaserSceneInterop StartScene(string name)
//        {
//            _jsRuntime.InvokeVoid("startScene", name);
//            return this;
//        }

//        public IPhaserSceneInterop StopScene(string name)
//        {
//            _jsRuntime.InvokeVoid("stopScene", name);
//            return this;
//        }

//        public void SwitchToScene(string name)
//        {
//            _jsRuntime.InvokeVoid("switchScene", _scene.GetName(), name);
//        }

//        public IPhaserSceneInterop ShakeCamera()
//        {
//            _jsRuntime.InvokeVoid("shakeCamera", _scene.GetName());
//            return this;
//        }
//    }
//}