using System;
using GameATron4000.Models;
using GameATron4000.Scripting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json.Linq;
using NLua;

namespace src.Core
{
    public class GameEngineFunctions
    {
        private readonly RoomState _roomState;
        private readonly Action<string, object> _addEventActivity;
        private readonly Action<string, object> _addMessageActivity;

        public GameEngineFunctions(RoomState roomState, Action<string, object> addEventActivity,
            Action<string, object> addMessageActivity)
        {
            _roomState = roomState;
            _addEventActivity = addEventActivity;
            _addMessageActivity = addMessageActivity;
        }

        public void StartGame()
        {
            _addEventActivity("GameStarted", null);
            // , new
            // {
            //     inventoryItems = new string[0];
            //     //  _gameInfo.InitialInventory.Select(inventoryItemId => new
            //     // {
            //     //     inventoryItemId = inventoryItemId,
            //     //     description = _gameInfo.InventoryItems[inventoryItemId].Description
            //     // })
            // });
        }

        [LuaGlobal(Name = "switch_room")]
        public void SwitchRoom(string roomId)
        {
            _addEventActivity("RoomSwitching", new 
            {
                room = new { id = roomId }
            });
        }

        [LuaGlobal(Name = "delay")]
        public void Delay(int milliseconds)
        {
            _addEventActivity("Delayed", new
            {
                time = milliseconds
            });
        }



        [LuaGlobal(Name = "put_actor")]
        public void PutActor(LuaTable actor, int x, int y)
        {
            var id = actor["id"]?.ToString() ?? throw new ArgumentException("Actor must have an id.");

            // Save the actor's new position in the room state.
            var actorState = new ActorState
            { 
                X = x,
                Y = y
            };
            //_roomState.Actors[id] = actorState;

            _addEventActivity("ActorPlacedInRoom", new
            {
                actor = new
                {
                    id = actor["id"]?.ToString(),
                    name = actor["name"]?.ToString(),
                    x = x,
                    y = y,
                    textColor = actor["text_col"]?.ToString()
                }
            });
        }

        [LuaGlobal(Name = "put_object")]
        public void PutObject(LuaTable @object, int x, int y)
        {
            var id = @object["id"]?.ToString() ?? throw new ArgumentException("Object must have an id.");

            // Save the object's new position in the room state.
            var objectState = new ObjectState
            { 
                X = x,
                Y = y,
                State = @object["state"]?.ToString()
            };
            //_roomState.Objects[id] = objectState;

            _addEventActivity("ObjectPlacedInRoom", new
            {
                @object = new
                {
                    id = @object["id"]?.ToString(),
                    name = @object["name"]?.ToString(),
                    x = objectState.X,
                    y = objectState.Y,
                    classes = @object["classes"] != null ? ((LuaTable)@object["classes"]).Values : new object[0]
                }
            });
        }

        [LuaGlobal(Name = "change_state")]
        public void ChangeObjectState(LuaTable @object, string state)
        {
            var objectId = @object["id"]?.ToString() ?? throw new ArgumentException("Object must have an id.", "object");

            @object["state"] = state;

            //_roomState.Objects[objectId].State = state;

            _addEventActivity("ObjectStateChanged", new
            {
                @object = new
                {
                    id = objectId,
                    state = state
                }
            });
        }

        [LuaGlobal(Name = "walk_to")]
        public void WalkTo(string actorId, int x, int y)
        {
            // var position = new ActorPosition(x, y);

            // // Save the actor's new position in the room state.
            // _roomState.ActorPositions[actorId] = new ActorPosition(x, y);

            // AddEventActivity("ActorMoved", new
            // {
            //     actorId = actorId,
            //     x = position.X,
            //     y = position.Y,
            //     direction = position.Direction.ToString()
            // });
        }

        // public void ActorMoved(ActorInstance actorInstance)
        // {
        //     _addEventActivity("ActorMoved", new
        //     {
        //         actorId = actorInstance.Definition.Id,
        //         x = actorInstance.State.X,
        //         y = actorInstance.State.Y,
        //         direction = "Front" // TODO
        //     });
        // }

        [LuaGlobal(Name = "face_direction")]
        public void FaceDirection(string actorId, ActorDirection direction)
        {
            _addEventActivity("ActorDirectionChanged", new
            {
                actorId = actorId,
                direction = direction.ToString()
            });
        }

        [LuaGlobal(Name = "say_line")]
        public void SayLine(LuaTable actor, string text)
        {
            var actorId = actor["id"];
            var actorName = actor["name"];

            _addMessageActivity($"{actorName} > {text}", new {
                actor = new 
                {
                    id = actorId
                }
            });
        }

        // public void ActorSpoke(Actor actor, string text)
        // {
        //     var messageActivity = MessageFactory.Text($"{actor.Description} > {text}");
        //     messageActivity.Properties = JObject.FromObject(new {
        //         actorId = actor.Id
        //     });

        //     _activities.Add(messageActivity);
        // }

        // public void SayCannedResponse()
        // {
        //     var line = _gameInfo.CannedResponses[_random.Next(0, _gameInfo.CannedResponses.Count)];

        //     SayLine("player", line);
        // }

        [LuaGlobal(Name = "add_object")]
        public void AddObject(string objectId, int x, int y)
        {
            // var gameObject = _gameInfo.Objects[objectId];

            // // Save the object's new position in the room state.
            // _roomState.ObjectPositions[objectId] = new ObjectPosition(x, y);

            // AddEventActivity("ObjectPlacedInRoom", new
            // {
            //     objectId = objectId,
            //     description = gameObject.Description,
            //     x = x,
            //     y = y,
            //     foreground = false // TODO
            // });
        }

        [LuaGlobal(Name = "remove_object")]
        public void RemoveObject(string objectId)
        {
            // // Remove the object from the room state.
            // _roomState.ObjectPositions.Remove(objectId);

            // AddEventActivity("ObjectRemovedFromRoom", new
            // {
            //     objectId = objectId
            // });
        }

        [LuaGlobal(Name = "add_to_inventory")]
        public void AddToInventory(string inventoryItemId)
        {
            // if (!_inventoryItems.Contains(inventoryItemId))
            // {
            //     _inventoryItems.Add(inventoryItemId);

            //     var inventoryItem = _gameInfo.InventoryItems[inventoryItemId];

            //     AddEventActivity("InventoryItemAdded", new
            //     {
            //         inventoryItemId = inventoryItemId,
            //         description = inventoryItem.Description
            //     });
            // }
        }

        [LuaGlobal(Name = "remove_from_inventory")]
        public void RemoveFromInventory(string inventoryItemId)
        {
            // if (_inventoryItems.Contains(inventoryItemId))
            // {
            //     _inventoryItems.Remove(inventoryItemId);
            // }

            // AddEventActivity("InventoryItemRemoved", new
            // {
            //     inventoryItemId = inventoryItemId
            // });
        }

        [LuaGlobal(Name = "narrate")]
        public void Narrate(string text)
        {
            _addEventActivity("Narrated", new
            {
                text = text
            });
        }

        [LuaGlobal(Name = "describe")]
        public void Describe(string text)
        {
//                _activities.Add(MessageFactory.Text(text));
        }
    }
}