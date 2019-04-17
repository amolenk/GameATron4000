using System;
using System.Collections;
using System.Collections.Generic;
using GameATron4000.Scripting;

namespace GameATron4000.Core
{
    public interface IGameScript
    {
        IEnumerable<IActor> Actors { get; }

        IEnumerable<IObject> Objects { get; }

        IEnumerable<IRoom> Rooms { get; }

        LuaWorld World { get; }

        IGameScriptResult OnInitializeGame();

        IGameScriptResult OnBeforeEnterRoom();

        IGameScriptResult OnAfterEnterRoom();

        IGameScriptResult OnPlayerInput(string input);

        void LoadScriptState(IGameScriptState state);

        IGameScriptState SaveScriptState();

        void UpdateGlobalWorldVariables();
    }
}