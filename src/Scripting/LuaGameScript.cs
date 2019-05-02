using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GameATron4000.Configuration;
using GameATron4000.Core;
using GameATron4000.Models;
using GameATron4000.Scripting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Configuration;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using NLua;
using NLua.Exceptions;

namespace GameATron4000.Scripting
{
    // TODO Dispose!!!
    public class LuaGameScript : IGameScript
    {
        private const string FUNCTION_NO_RESULT = "no_result";

        private readonly ActivityFactory _activityFactory;
        private readonly Lua _lua;
        private readonly LuaFunctions _luaFunctions;
        private readonly LuaGameScriptState _initialScriptState;

        public object DebugStuff
        {
            get
            {
                return _lua["_G"];
            }
        }

        public LuaGameScript(ActivityFactory activityFactory)
        {
            _activityFactory = activityFactory;
            _lua = new Lua();
            _lua.State.Encoding = Encoding.UTF8;

            // Create sandbox by redefining the 'import' function.
            _lua.DoString ($@"
                import = function () end
                
                -- Constants
                class_fixed_to_camera = ""class_fixed_to_camera""
                class_invisible = ""class_invisible""
                class_untouchable = ""class_untouchable""
                class_use_with = ""class_use_with""
                
                face_back = ""back""
                face_front = ""front""

                pos_infront = ""infront""
                pos_center = ""center""
                pos_above = ""above""

                narrator = {{
                    type = ""narrator""
                }}

                world = {{
                    type = ""world""
                }}

                start_cutscene = function(cutscene)
                    selected_cutscene = cutscene
                    cutscene.start()
                end

                end_cutscene = function()
                    selected_cutscene = nil
                end

                ", "initialization");

            // Make the game engine functions available to the Lua script.
            _luaFunctions = new LuaFunctions(this, activityFactory);
            LuaRegistrationHelper.TaggedInstanceMethods(_lua, _luaFunctions);

            _lua.DoFile("Gameplay/scripts/game.lua");
            _lua.DoFile("Gameplay/scripts/actors.lua");
            _lua.DoFile("Gameplay/scripts/objects.lua");
            _lua.DoFile("Gameplay/scripts/rooms.lua");
            
            foreach (var path in Directory.GetFiles("Gameplay/scripts/conversations", "*.lua"))
            {
                _lua.DoFile(path);
            }

            _initialScriptState = LuaGameScriptState.Save(_lua);
        }

        public IEnumerable<IActor> Actors
        {
            get
            {
                return GetTables()
                    .Where(t => t.GetString(LuaConstants.Tables.Type) == LuaConstants.Tables.Types.Actor)
                    .Select(t => LuaActor.FromTable(t, this));
            }
        }

        public IEnumerable<IObject> Objects
        {
            get
            {
                return GetTables()
                    .Where(t => t.GetString(LuaConstants.Tables.Type) == LuaConstants.Tables.Types.Object)
                    .Select(t => LuaObject.FromTable(t, this));
            }
        }

        public IEnumerable<IRoom> Rooms
        {
            get
            {
                return GetTables()
                    .Where(t => t.GetString(LuaConstants.Tables.Type) == LuaConstants.Tables.Types.Room)
                    .Select(t => LuaRoom.FromTable(t, this));
            }
        }

        public LuaWorld World
        {
            get
            {
                var worldTable = GetTables()
                    .Single(t => t.GetString(LuaConstants.Tables.Type) == LuaConstants.Tables.Types.World);

                return LuaWorld.FromTable(worldTable, this);
            }
        }

        public IGameScriptResult OnInitializeGame()
        {
            _luaFunctions.Reset();

            _lua.DoString("initialize_game()");

            return _luaFunctions.Result;
        }

        public IGameScriptResult OnBeforeEnterRoom()
        {
            return RunRoomFunction("before_enter");
        }

        public IGameScriptResult OnAfterEnterRoom()
        {
            return RunRoomFunction("after_enter");
        }

        public IGameScriptResult OnPlayerInput(string input)
        {
            var parsedInput = ParsePlayerInput(input);
            if (parsedInput.HasValue)
            {
                _luaFunctions.Reset();

                var verbs = parsedInput.Value.ObjectOrActorTable.GetTable(LuaConstants.Tables.Keys.Verbs);
                if (verbs != null)
                {
                    var func = verbs[parsedInput.Value.Verb] as LuaFunction;
                    if (func != null)
                    {
                        func.Call(parsedInput.Value.FunctionParameters);
                    }
                }
            }

            if (!_luaFunctions.Result.Activities.Any()
                && _luaFunctions.Result.NextDialogId.Length == 0
                && (!parsedInput.HasValue || parsedInput.Value.Verb != "walk_to")
                && _lua[FUNCTION_NO_RESULT] is LuaFunction)
            {
                ((LuaFunction)_lua[FUNCTION_NO_RESULT]).Call();
            }

            return _luaFunctions.Result;
        }

        public IGameScriptResult OnConversationStarted(string conversationId)
        {
            _luaFunctions.Reset();

            var conversation = GetTable("id", conversationId, "conversation");
            var function = conversation["start"] as LuaFunction;
            if (function != null)
            {
                function.Call(conversation);
            }

            return _luaFunctions.Result;
        }

        public IGameScriptResult OnConversationContinued(string conversationId, string action)
        {
            _luaFunctions.Reset();

            // TODO Extract to RunTableFunction (along with room?)

            var conversation = GetTable("id", conversationId, "conversation");
            var function = conversation[action] as LuaFunction;
            if (function != null)
            {
                function.Call(conversation);
            }

            return _luaFunctions.Result;
        }

        public IGameScriptState SaveScriptState()
        {
            var result = new Dictionary<string, object>();
            var currentScriptState = LuaGameScriptState.Save(_lua);

            return currentScriptState.Except(_initialScriptState);
        }

        public void LoadScriptState(IGameScriptState scriptState)
        {
            if (!(scriptState is LuaGameScriptState))
            {
                throw new ArgumentException(
                    $"Provided script state must be of type {typeof(LuaGameScriptState)}.",
                    nameof(scriptState));
            }

            ((LuaGameScriptState)scriptState).Load(_lua);

            UpdateGlobalWorldVariables();
        }

        public void UpdatePlayerPosition(int x, int y)
        {
            var selectedActor = World.GetSelectedActor();
            selectedActor.PositionX = x;
            selectedActor.PositionY = y;
        }

        public void UpdateGlobalWorldVariables()
        {
            _lua[LuaConstants.Tables.Keys.SelectedActor] = GetTables().FirstOrDefault(
                t => t.GetString(LuaConstants.Tables.Type) == LuaConstants.Tables.Types.Actor
                    && t.GetString(LuaConstants.Tables.Id) == World.SelectedActorId);

            _lua[LuaConstants.Tables.Keys.CurrentRoom] = GetTables().FirstOrDefault(
                t => t.GetString(LuaConstants.Tables.Type) == LuaConstants.Tables.Types.Room
                    && t.GetString(LuaConstants.Tables.Id) == World.CurrentRoomId);

            if (World.PreviousRoomId.Length > 0)
            {
                _lua[LuaConstants.Tables.Keys.PreviousRoom] = GetTables().FirstOrDefault(
                    t => t.GetString(LuaConstants.Tables.Type) == LuaConstants.Tables.Types.Room
                        && t.GetString(LuaConstants.Tables.Id) == World.PreviousRoomId);
            }
        }

        private (string Verb, LuaTable ObjectOrActorTable, object[] FunctionParameters)? ParsePlayerInput(string input)
        {
            var selectedRoomId = World.CurrentRoomId;

            // "Use With"
            var useWithPattern = @"^(use)\s(?<arg1>.*?)\swith\s(?<arg2>.*)$";
            var match = Regex.Match(input, useWithPattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var arg1 = GetTableInScope(selectedRoomId, LuaConstants.Tables.Name, match.Groups["arg1"].Value,
                    LuaConstants.Tables.Types.Actor, LuaConstants.Tables.Types.Object);
                var arg2 = GetTableInScope(selectedRoomId, LuaConstants.Tables.Name, match.Groups["arg2"].Value,
                    LuaConstants.Tables.Types.Object);

                return ("use_with", arg1, new object[] { arg1, arg2 });
            }

            // "Give To"
            var giveToPattern = @"^(give)\s(?<arg1>.*?)\sto\s(?<arg2>.*)$";
            match = Regex.Match(input, giveToPattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var arg1 = GetTableInScope(selectedRoomId, LuaConstants.Tables.Name, match.Groups["arg1"].Value,
                    LuaConstants.Tables.Types.Actor, LuaConstants.Tables.Types.Object);
                var arg2 = GetTableInScope(selectedRoomId, LuaConstants.Tables.Name, match.Groups["arg2"].Value,
                    LuaConstants.Tables.Types.Actor);

                return ("give_to", arg1, new object[] { arg1, arg2 });
            }

            // Get the name of the Lua function to invoke from the player's command.
            var otherPattern = @"^(?<verb>open|close|pick up|look at|talk to|use|push|pull|walk to)\s(?<arg>.*)$";
            match = Regex.Match(input, otherPattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var arg = GetTableInScope(selectedRoomId, LuaConstants.Tables.Name, match.Groups["arg"].Value,
                    LuaConstants.Tables.Types.Actor, LuaConstants.Tables.Types.Object);

                return (match.Groups["verb"].Value.ToLowerInvariant().Replace(' ', '_'), arg, new object[] { arg });
            }

            return null;
        }

        private LuaGameScriptResult RunRoomFunction(string functionName)
        {
            _luaFunctions.Reset();

            var room = GetTable(LuaConstants.Tables.Id, World.CurrentRoomId, LuaConstants.Tables.Types.Room);
            
            var function = room[functionName] as LuaFunction;
            if (function != null)
            {
                function.Call(room);
            }

            return _luaFunctions.Result;
        }

        private LuaTable GetTableInScope(string roomId, string key, string value, params string[] types)
        {
            return GetTables()
                .Where(t => types.Contains(t.GetString(LuaConstants.Tables.Type))
                    && (t.GetString(LuaConstants.Tables.Object.RoomId) == roomId
                    || t.GetString(LuaConstants.Tables.Object.Owner) == World.SelectedActorId))
                // TODO Weird that we need to use an Object constant.
                .FirstOrDefault(t => string.Equals(t.GetString(key), value, StringComparison.OrdinalIgnoreCase));
        }

        private LuaTable GetTable(string key, string value, params string[] types)
        {
            return GetTables()
                .FirstOrDefault(t => types.Contains(t.GetString(LuaConstants.Tables.Type))
                    && string.Equals(t.GetString(key), value, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<LuaTable> GetTables()
        {
            // Ignore keys for the selected actor, and current/previous room.
            // Otherwise, the result will contain duplicates.
            var ignoreTableKeys = new string[]
            {
                LuaConstants.Tables.Keys.CurrentRoom,
                LuaConstants.Tables.Keys.PreviousRoom,
                LuaConstants.Tables.Keys.SelectedActor
            };

            return ((LuaTable)_lua[LuaConstants.Tables.Keys.Globals])
                .ToEnumerable()
                .Where(g => g.Value is LuaTable && !ignoreTableKeys.Contains(g.Key))
                .Select(g => (LuaTable)g.Value);
        }
    }
}