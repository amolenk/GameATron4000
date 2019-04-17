using System;
using System.Collections.Generic;
using System.Linq;
using GameATron4000.Core;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using NLua;

namespace GameATron4000.Scripting
{
    public class LuaWorld : IWorld
    {
        private readonly LuaTable _luaTable;
        private readonly LuaGameScript _script;

        private LuaWorld(LuaTable luaTable, LuaGameScript script)
        {
            _luaTable = luaTable;
            _script = script;
        }

        public static LuaWorld FromTable(LuaTable table, LuaGameScript script)
        {
            return new LuaWorld(table, script);
        }

        public string CurrentRoomId
        {
            get { return _luaTable.GetString(LuaConstants.Tables.World.CurrentRoomId); }
            set { _luaTable[LuaConstants.Tables.World.CurrentRoomId] = value; }
        }

        public string PreviousRoomId
        {
            get { return _luaTable.GetString(LuaConstants.Tables.World.PreviousRoomId); }
            set { _luaTable[LuaConstants.Tables.World.PreviousRoomId] = value; }
        }

        public string SelectedActorId
        {
            get { return _luaTable.GetString(LuaConstants.Tables.World.SelectedActorId); }
            set { _luaTable[LuaConstants.Tables.World.SelectedActorId] = value; }
        }

        public string CameraFollow
        {
            get { return _luaTable.GetString(LuaConstants.Tables.World.CameraFollow); }
            set { _luaTable[LuaConstants.Tables.World.CameraFollow] = value; }
        }

        public IActor GetSelectedActor()
        {
            if (SelectedActorId.Length == 0)
            {
                // TODO
                throw new Exception("No selected actor set");
            }

            var result = _script.Actors.FirstOrDefault(a => a.Id == SelectedActorId);
            if (result == null)
            {
                // TODO
                throw new Exception("Invalid id set for selectedACtor");
            }

            return result;
        }

        public IRoom GetSelectedRoom()
        {
            if (CurrentRoomId.Length == 0)
            {
                // TODO
                throw new Exception("No selected room set");
            }

            var result = _script.Rooms.FirstOrDefault(a => a.Id == CurrentRoomId);
            if (result == null)
            {
                // TODO
                throw new Exception("Invalid id set for selected room");
            }

            return result;
        }
    }
}