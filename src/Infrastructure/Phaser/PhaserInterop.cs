using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameATron4000.Infrastructure.Phaser
{
    /// <summary>
    /// Generic client class that interfaces .NET Standard/Blazor with SignalR Javascript client
    /// </summary>
    public class PhaserInterop : IPhaserInterop
    {
        private readonly IJSInProcessRuntime _jSRuntime;

        public PhaserInterop(IJSRuntime jsRuntime)
        {
            _jSRuntime = (IJSInProcessRuntime)jsRuntime;
        }

        //public void RegisterScene(Scene scene)
        //{
        //    _jSRuntime.InvokeVoid("registerScene", scene.GetName(), DotNetObjectReference.Create(scene));
        //}

        // public void SwitchScene(Scene from, Scene to)
        // {
        //     _jSRuntime.InvokeVoid("switchScene", from.GetName(), to.GetName());
        // }

        public void Start(string containerElement, string title)
        {
            _jSRuntime.InvokeVoid("startPhaser", containerElement, title);
        }

        //public IPhaserSceneInterop Scene(Scene scene)
        //    => new PhaserSceneInterop(_jSRuntime, scene);

        // public IPhaserClient AddSprite(Scene scene, string sprite, string imageName, int x, int y, double scale, bool interactive)
        // {
        //     _jSRuntime.InvokeVoid("addSprite", scene.GetName(), sprite, imageName, x, y, scale, interactive);
        //     return this;
        // }

        // public IPhaserClient AddHandler(Scene scene, string spriteName, string handlerName)
        // {
        //     _jSRuntime.InvokeVoid("addHandler", scene.GetName(), spriteName, handlerName);
        //     return this;
        // }
    }
}