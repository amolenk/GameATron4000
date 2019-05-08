using System.Collections.Generic;
using System.Linq;
using GameATron4000.Core;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using NLua;

namespace GameATron4000.Core
{
    public interface IScale
    {
        int? Min { get; }

        int? Max { get; }

        int? StartPos { get; }

        int? EndPos { get; }
    }
}