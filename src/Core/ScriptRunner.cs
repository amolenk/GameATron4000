using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GameATron4000.Configuration;
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
using src.Core;

namespace GameATron4000.Core
{
    public class ScriptRunner
    {
        private readonly string _roomId;
        private readonly RoomState _roomState;
        private List<string> _inventoryItems;
        private readonly Dictionary<string, object> _customState;
        private readonly ITurnContext _turnContext;
        private readonly Lua _lua;
        private readonly List<IActivity> _activities;

        public ScriptRunner(string roomId, RoomState roomState, List<string> inventoryItems,
            Dictionary<string, object> customState, ITurnContext turnContext)
        {
            _roomId = roomId;
            _roomState = roomState;
            _inventoryItems = inventoryItems;
            _customState = customState;
            _turnContext = turnContext;
            _lua = new Lua();
            _activities = new List<IActivity>();

            // Create sandbox by redefining the 'import' function.
            _lua.DoString (@"
                import = function () end
                
                -- Constants
                player_actor = ""player""
                class_untouchable = ""class_untouchable""

                ", "initialization");

            // Make the custom state available to the Lua script.
            // Wrapping the dictionary in a DefaultValueIfMissingDictionary object
            // ensures that a default value is returned if a key doesn't exist
            // (instead of throwing an exception).
            _lua["state"] = new DefaultValueIfMissingDictionary<string, object>(customState);

            // Make the game engine functions available to the Lua script.
            LuaRegistrationHelper.TaggedInstanceMethods(_lua,
                new GameEngineFunctions(roomState, AddEventActivity, AddMessageActivity));

            _lua.DoFile($"Gameplay/scripts/actors.lua");
            _lua.DoFile($"Gameplay/scripts/objects.lua");
            _lua.DoFile($"Gameplay/scripts/rooms/{_roomId}.lua");

            var globals = (LuaTable)_lua["_G"];

            // Update object properties from room state.
            foreach (var objectState in roomState.Objects)
            {
                var @object = GetObjectById(objectState.Key);
                @object.Table["state"] = objectState.Value.State;

                // var script = $"{objectState.Key}.state = \"{objectState.Value.State}\"";
                // _lua.DoString(script, "update object state");
            }
        }

        public static async Task<ScriptRunner> CreateAsync(string roomId,
            IStatePropertyAccessor<Dictionary<string, RoomState>> roomStateAccessor,
            IStatePropertyAccessor<List<string>> inventoryItemsAccessor,
            IStatePropertyAccessor<Dictionary<string, object>> customStateAccessor,
            ITurnContext turnContext)
        {
            var inventoryItems = await inventoryItemsAccessor.GetAsync(
                turnContext, () => new List<string>());

            var roomStates = await roomStateAccessor.GetAsync(
                turnContext, () => new Dictionary<string, RoomState>());

            var customState = await customStateAccessor.GetAsync(
                turnContext, () => new Dictionary<string, object>());

            // If there's no room state found, use the initial room state.
            RoomState roomState;
            if (!roomStates.TryGetValue(roomId, out roomState))
            {
                roomState = new RoomState();
                roomStates.Add(roomId, roomState);
            }

            return new ScriptRunner(roomId, roomState, inventoryItems, customState, turnContext);
        }

        public Task EnterRoomAsync()
        {
            try
            {
                AddEventActivity("RoomSwitching");

                // Let the client know that the player has entered a new room.
                _lua.DoString($"{_roomId}.enter({_roomId})", "enter room");

                AddEventActivity("RoomEntered");
            }
            catch (LuaScriptException ex)
            {
                _activities.Clear();
                // TODO Error
                AddEventActivity("Narrated", new { text = $"Error at {ex.Source}: {ex.Message}" });
            }

            return FlushAsync();
        }

        public async Task<string> RunCommandAsync(string command)
        {
            // Get the name of the Lua function to invoke from the player's command.
            var pattern = @"^(?<verb>look at|pick up)\s(?<object>.*)$";
            var match = Regex.Match(command.ToLowerInvariant(), pattern);
            var verb = match.Groups["verb"].Value;
            var objectName = match.Groups["object"].Value;

            var room = (LuaTable)_lua["room"];
            var objects = (LuaTable)_lua["objects"];

            var @object = GetObjectByName(objectName);
            string result = null;

            try
            {
                var verbs = (LuaTable)@object.Table["verbs"];

                foreach (var verbKey in verbs.Keys)
                {
                    if (verbKey.ToString() == verb.Replace(" ", string.Empty))
                    {
                        var verbFunction = $"{@object.Key}.verbs.{verbKey}({@object.Key})";

                        var returnValues = _lua.DoString(verbFunction, command);

                        result = returnValues != null ? returnValues.First().ToString() : null;

                        break;
                    }
                }
            }
            catch (LuaScriptException ex)
            {
                // TODO Error
                AddEventActivity("Narrated", new { text = $"Error at {ex.Source}: {ex.Message}" });
            }

            await FlushAsync();

            return result ?? string.Empty;
        }

        
        private async Task FlushAsync()
        {
            // Add an Idle activity to let the client know that we're waiting for player interaction.
            AddEventActivity("Idle");

            await _turnContext.SendActivitiesAsync(_activities.ToArray());

            _activities.Clear();
        }

        private void AddEventActivity(string name, object properties = null)
        {
            var eventActivity = _turnContext.Activity.CreateReply();
            eventActivity.Type = "event";
            eventActivity.Name = name;

            if (properties != null)
            {
                eventActivity.Properties = JObject.FromObject(properties);
            }

            _activities.Add(eventActivity);
        }

        private void AddMessageActivity(string text, object properties = null)
        {
            var messageActivity = MessageFactory.Text(text);

            if (properties != null)
            {
                messageActivity.Properties = JObject.FromObject(properties);
            }

            _activities.Add(messageActivity);
        }

        private LuaTable GetActor(string id)
        {
            var actors = (LuaTable)_lua["actors"];

            foreach (dynamic actorEntry in actors)
            {
                var actor = (LuaTable)actorEntry.Value;
                if (actor["id"].ToString() == id)
                {
                    return actor;
                }
            }

            // TODO GameEngineException
            throw new Exception($"Unknown actor id '{id}.");
        }

        private (string Key, LuaTable Table) GetObjectById(string objectId)
        {
            return GetTable("object", (table) => table["id"]?.ToString() == objectId);
        }

        private (string Key, LuaTable Table) GetObjectByName(string objectName)
        {
            return GetTable("object", (table) => table["name"]?.ToString() == objectName);
        }

        private (string Key, LuaTable Table) GetTable(string type, Predicate<LuaTable> predicate)
        {
            var globals = (LuaTable)_lua["_G"];
            LuaTable table;

            foreach (dynamic global in globals)
            {
                if ((table = global.Value as LuaTable) != null
                    && table["type"]?.ToString() == type
                    && predicate(table))
                {
                    return (Key: global.Key, Table: table);
                }
            }

            // TODO
            throw new Exception($"Cannot find LuaTable with type '{type}' for the given predicate.");
        }
    }
}