using System;
namespace Amolenk.GameATron4000.Engine.Scripting.Model
{
    public class ScriptGlobals
    {
        public WorldScript World { get; } = new();

        public RoomScript AddRoom(string id) =>
            World.AddRoom(id);
    }
}

