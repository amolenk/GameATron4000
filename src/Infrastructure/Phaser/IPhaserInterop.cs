using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameATron4000.Infrastructure.Phaser
{
    public interface IPhaserInterop
    {
        //void RegisterScene(Scene scene);

        void Start(string containerElement, string title);

        //IPhaserSceneInterop Scene(Scene scene);
    }
}