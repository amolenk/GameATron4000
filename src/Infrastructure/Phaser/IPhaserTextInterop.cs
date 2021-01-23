using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameATron4000.Infrastructure.Phaser
{
    public interface IPhaserTextInterop
    {
        IPhaserTextInterop Value(string text);

        IPhaserTextInterop WithOrigin(double x, double y);
    }
}