using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameATron4000.Infrastructure.Phaser
{
    public interface IPhaserSpriteInterop
    {
        bool Exists();

        IPhaserSpriteInterop Crop(double x, double y, double width, double height);

        IPhaserSpriteInterop Move(double x, double y);

        IPhaserSpriteInterop Scale(double scale);

        IPhaserSpriteInterop Tint(string color);

        IPhaserSpriteInterop OnPointerUp(string handlerName);

        IPhaserSpriteInterop OnPointerUpAsync(string handlerName);

        IPhaserSpriteInterop OnPointerDown(string handlerName);

        IPhaserSpriteInterop OnPointerDownAsync(string handlerName);

        IPhaserSpriteInterop OnPointerMove(string handlerName);

        IPhaserSpriteInterop OnPointerMoveAsync(string handlerName);
    }
}