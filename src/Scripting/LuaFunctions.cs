using System;
using System.Collections.Generic;
using System.Linq;
using GameATron4000.Core;
using GameATron4000.Models;
using GameATron4000.Scripting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using NLua;

namespace GameATron4000.Scripting
{
    public class LuaFunctions
    {
        private LuaGameScript _script;
        private ActivityFactory _activityFactory;

        public LuaFunctions(LuaGameScript script, ActivityFactory activityFactory)
        {
            _script = script;
            _activityFactory = activityFactory;
        }

        public LuaGameScriptResult Result { get; private set; } = new LuaGameScriptResult();

        public void Reset()
        {
            Result = new LuaGameScriptResult();
        }

        [LuaGlobal(Name = "camera_follow")]
        public void CameraFollow(LuaTable actorTable)
        {
            var actor = LuaActor.FromTable(actorTable, _script);

            _script.World.CameraFollow = actor.Id;
        }

        [LuaGlobal(Name = "change_room")]
        public void ChangeRoom(LuaTable roomTable)
        {
            var room = LuaRoom.FromTable(roomTable, _script);

            _script.World.PreviousRoomId = _script.World.CurrentRoomId;
            _script.World.CurrentRoomId = room.Id;
            _script.UpdateGlobalWorldVariables();

            Result.NextDialogId = room.Id;
            Result.NextDialogReplace = true;
        }

        [LuaGlobal(Name = "change_state")]
        public void ChangeState(LuaTable objectTable, string state)
        {
            var obj = LuaObject.FromTable(objectTable, _script);
            var room = _script.World.GetSelectedRoom();

            // Don't need to do anything if the state stays the same.
            if (obj.State == state)
            {
                return;
            }

            // Check if any objects will become invisible due to this state change.
            var objectsToRemove = room.GetObjects().Where(o => o.DependsOn != null
                && o.DependsOn.Object.Id == obj.Id
                && o.DependsOn.State == obj.State).ToList();

            // Apply the state change.
            obj.State = state;

            // Check if any objects will become visible due to this state change.
            var objectsToAdd = room.GetObjects().Where(o => o.DependsOn != null
                && o.DependsOn.Object.Id == obj.Id
                && o.DependsOn.State == state);

            Result.Activities.Add(_activityFactory.ObjectStateChanged(obj, objectsToAdd, objectsToRemove));
        }

        [LuaGlobal(Name = "option")]
        public void ConversationOption(string text, string action, bool? predicate = null)
        {
            if (!predicate.HasValue || predicate.Value)
            {
                Result.ConversationOptions.Add(action, text);
            }
        }

        [LuaGlobal(Name = "face_dir")]
        public void FaceDirection(string direction, LuaTable actorTable = null)
        {
            var actor = actorTable != null
                ? LuaActor.FromTable(actorTable, _script)
                : _script.Actors.First(a => a.Id == _script.World.SelectedActorId);

            actor.FaceDirection = direction;

            Result.Activities.Add(_activityFactory.ActorDirectionFacedChanged(direction, actor));
        }

        [LuaGlobal(Name = "owned_by")]
        public bool OwnedBy(LuaTable objectTable, LuaTable actorTable)
        {
            var obj = LuaObject.FromTable(objectTable, _script);
            var actorId = actorTable != null
                ? LuaActor.FromTable(actorTable, _script).Id
                : _script.World.SelectedActorId;

            return obj.Owner == actorId;
        }

        [LuaGlobal(Name = "put_actor")]
        public void PutActor(LuaTable actorTable, int x, int y, LuaTable roomTable = null)
        {
            var roomId = (roomTable != null) ? LuaRoom.FromTable(roomTable, _script).Id : _script.World.CurrentRoomId;
            var actor = LuaActor.FromTable(actorTable, _script);

            actor.RoomId = roomId;
            actor.PositionX = x;
            actor.PositionY = y;

            Result.Activities.Add(_activityFactory.ActorPlacedInRoom(actor));
        }

        [LuaGlobal(Name = "put_object")]
        public void PutObject(LuaTable objectTable, int x, int y, LuaTable roomTable = null, bool? addCameraOffset = null)
        {
            var roomId = (roomTable != null) ? LuaRoom.FromTable(roomTable, _script).Id : _script.World.CurrentRoomId;
            var obj = LuaObject.FromTable(objectTable, _script);

            // If the object is currently owned by an actor, remove
            // it from the actor.
            if (obj.Owner.Length > 0)
            {
                // If the owner is the selected actor, remove the object
                // from the player's inventory.
                if (obj.Owner == _script.World.SelectedActorId)
                {
                    Result.Activities.Add(_activityFactory.InventoryItemRemoved(obj));
                }

                obj.Owner = string.Empty;
            }

            // Add the item to the room.
            obj.RoomId = roomId;
            obj.PositionX = x;
            obj.PositionY = y;

            // Only send an event to the GUI if the object is being placed in
            // the current room.
            if (obj.RoomId == _script.World.CurrentRoomId)
            {
                Result.Activities.Add(_activityFactory.ObjectPlacedInRoom(obj));
            }
        }

        [LuaGlobal(Name = "remove_object")]
        public void RemoveObject(LuaTable objectTable)
        {
            var obj = LuaObject.FromTable(objectTable, _script);

            // Only send an event to the GUI if the object is removed from
            // the current room.
            if (obj.RoomId == _script.World.CurrentRoomId)
            {
                Result.Activities.Add(_activityFactory.ObjectRemovedFromRoom(obj));
            }

            // Remove the object from the room.
            obj.RoomId = string.Empty;
            obj.PositionX = null;
            obj.PositionY = null;
        }

        [LuaGlobal(Name = "say_line")]
        public void SayLine(string text, LuaTable actorTable = null)
        {
            if (actorTable?.GetString(LuaConstants.Tables.Type) == LuaConstants.Tables.Types.Narrator)
            {
                Result.Activities.Add(_activityFactory.LineSpoken(text));
            }
            else
            {
                var actor = actorTable != null
                    ? LuaActor.FromTable(actorTable, _script)
                    : _script.Actors.First(a => a.Id == _script.World.SelectedActorId);

                Result.Activities.Add(_activityFactory.LineSpoken(text, actor));
            }
        }

        [LuaGlobal(Name = "select_actor")]
        public void SelectActor(LuaTable actorTable)
        {
            var actor = LuaActor.FromTable(actorTable, _script);

            _script.World.SelectedActorId = actor.Id;
            _script.UpdateGlobalWorldVariables();
        }

        [LuaGlobal(Name = "set_owner")]
        public void SetOwner(LuaTable objectTable, LuaTable actorTable)
        {
            var obj = LuaObject.FromTable(objectTable, _script);
            var actor = LuaActor.FromTable(actorTable, _script);

            // If the object is currently placed in a room, remove it.
            if (obj.RoomId.Length > 0)
            {
                obj.RoomId = string.Empty;
                obj.PositionX = null;
                obj.PositionY = null;

                Result.Activities.Add(_activityFactory.ObjectRemovedFromRoom(obj));
            }

            // If the current owner is the selected actor, remove it from the
            // player's inventory.
            if (obj.Owner == _script.World.SelectedActorId)
            {
                Result.Activities.Add(_activityFactory.InventoryItemRemoved(obj));
            }

            // Change the owner.
            obj.Owner = actor.Id;

            // If the new owner is the selected actor, add the object to the
            // player's inventory.
            if (obj.Owner == _script.World.SelectedActorId)
            {
                Result.Activities.Add(_activityFactory.InventoryItemAdded(obj));
            }
        }

        [LuaGlobal(Name = "start_talking")]
        public void StartTalking(string conversationId)
        {
            Result.NextDialogId = conversationId;
            Result.NextDialogReplace = false;
        }

        [LuaGlobal(Name = "wait")]
        public void Wait(int milliseconds)
        {
            Result.Activities.Add(_activityFactory.Halted(milliseconds));
        }

        [LuaGlobal(Name = "walk_to")]
        public void WalkTo(int x, int y, string faceDirection = null, LuaTable actorTable = null)
        {
            var actor = GetActor(actorTable);
            actor.PositionX = x;
            actor.PositionY = y;
            actor.FaceDirection = faceDirection; 

            Result.Activities.Add(_activityFactory.ActorMoved(actor));
        }

        // TODO Make sure everything uses this
        private IActor GetActor(LuaTable actorTable = null)
        {
            return actorTable != null
                ? LuaActor.FromTable(actorTable, _script)
                : _script.Actors.First(a => a.Id == _script.World.SelectedActorId);
        }
    }
}