using System.Collections.Generic;
using System.Linq;
using GameATron4000.Core;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using NLua;

namespace GameATron4000.Scripting
{
    public static class LuaConstants
    {
        public static class Tables
        {
            public const string Id = "id";
            public const string Type = "type";
            public const string Name = "name";

            public static class Actor
            {
                public const string Classes = "classes";
                public const string RoomId = "room_id";
                public const string PositionX = "x";
                public const string PositionY = "y";
                public const string TextColor = "text_col";
            }

            public static class Keys
            {
                public const string CurrentRoom = "current_room";
                public const string Globals = "_G";
                public const string PreviousRoom = "prev_room";
                public const string SelectedActor = "selected_actor";
                public const string Verbs = "verbs";
            }

            public static class Object
            {
                public const string Classes = "classes";
                public const string Owner = "owner";
                public const string RoomId = "room_id";
                public const string PositionX = "x";
                public const string PositionY = "y";
                public const string ZOffset = "z_offset";
                public const string State = "state";
                public const string UsePosition = "use_pos";
                public const string UseDirection = "use_dir";
                public const string FaceDirection = "face_dir";
                public const string DependsOn = "depends_on";
            }

            public static class ObjectDependency
            {
                public const string Object = "object";
                public const string State = "state";
            }

            public static class Room
            {
                public const string Scale = "scale";
                public const string Walkbox = "walkbox";
            }

            public static class Scale
            {
                public const string Min = "min";
                public const string Max = "max";
                public const string Start = "start_pos";
                public const string End = "end_pos";
            }

            public static class Types
            {
                public const string Actor = "actor";        
                public const string Narrator = "narrator";        
                public const string Object = "object";
                public const string Room = "room";
                public const string World = "world";
            }

            public static class World
            {
                public const string CurrentRoomId = "current_room_id";
                public const string PreviousRoomId = "prev_room_id";
                public const string SelectedActorId = "selected_actor_id";
                public const string CameraFollow = "camera_follow";
            }
        }
    }
}