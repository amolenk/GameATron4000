using System.Collections.Generic;
using System.Linq;
using GameATron4000.Core;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using NLua;

namespace GameATron4000.Core
{
    public interface IObjectDependency
    {
        IObject Object { get; }

        string State { get; }
    }
}