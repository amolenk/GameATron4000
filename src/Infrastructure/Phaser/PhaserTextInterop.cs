//using Microsoft.JSInterop;

//namespace Amolenk.ServerlessPonies.ClientApplication.Phaser
//{
//    public class PhaserTextInterop : IPhaserTextInterop
//    {
//        private readonly IJSInProcessRuntime _jsRuntime;
//        private readonly string _sceneName;
//        private readonly string _textName;

//        public PhaserTextInterop(IJSInProcessRuntime jsRuntime, string sceneName, string textName)
//        {
//            _jsRuntime = jsRuntime;
//            _sceneName = sceneName;
//            _textName = textName;
//        }

//        public IPhaserTextInterop Value(string text)
//        {
//            _jsRuntime.InvokeVoid("setTextValue", _sceneName, _textName, text);
//            return this;
//        }

//        public IPhaserTextInterop WithOrigin(double x, double y)
//        {
//            _jsRuntime.InvokeVoid("setTextOrigin", _sceneName, _textName, x, y);
//            return this;
//        }
//    }
//}