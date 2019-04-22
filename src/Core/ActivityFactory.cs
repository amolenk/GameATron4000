using System;
using System.Collections.Generic;
using System.Linq;
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

namespace GameATron4000.Core
{
    // TODO GameInfo no longer needed (save all data in action objects)
    // TODO Pass dialogContext as ctor parameter (lazy instantiate in Begin/Continue/Resume methods of RoomDialog)
    public class ActivityFactory
    {
        private readonly ITurnContext _context;
        private readonly Random _random;

        public ActivityFactory(ITurnContext context)
        {
            _context = context;
            _random = new Random();
        }

        public Activity RoomEntering(string roomId)
        {
            return CreateEventActivity("RoomEntering", new
            {
                room = new
                {
                    id = roomId
                }
            });
        }

        public Activity ActorDirectionFacedChanged(string direction, IActor actor)
        {
            return CreateEventActivity("ActorDirectionFacedChanged", new
            {
                actor = new
                {
                    id = actor.Id,
                    direction = direction
                }
            });
        }

        public Activity ActorMoved(IActor actor)
        {
            return CreateEventActivity("ActorMoved", new
            {
                actor = new
                {
                    id = actor.Id,
                    x = actor.PositionX,
                    y = actor.PositionY,
                    faceDirection = actor.FaceDirection // TODO Set defaults in properties
                }
            });
        }

        public Activity ActorPlacedInRoom(IActor actor)
        {
            return CreateEventActivity("ActorPlacedInRoom", new
            {
                actor = new
                {
                    id = actor.Id,
                    name = actor.Name,
                    classes = actor.Classes,
                    x = actor.PositionX,
                    y = actor.PositionY,
                    textColor = actor.TextColor
                }
            });
        }

        public Activity CannedResponse(IGameScript script)
        {
            var selectedActor = script.World.GetSelectedActor();

            // TODO
            return LineSpoken("(canned response)", selectedActor);

//                _gameInfo.CannedResponses[_random.Next(0, _gameInfo.CannedResponses.Count)]);
        }

        public Activity ErrorOccured(Exception ex)
        {
            return CreateEventActivity("ErrorOccured", new
            {
                message = ex.Message,
                stackTrace = ex.StackTrace // TODO Only debug
            });
        }

        public Activity Halted(int milliseconds)
        {
            return CreateEventActivity("Halted", new
            {
                time = milliseconds
            });
        }

        public Activity GameStarted(IGameScript script)
        {
            var selectedActor = script.World.GetSelectedActor();

            return CreateEventActivity("GameStarted", new
            {
                actor = new
                {
                    id = selectedActor.Id
                },
                camera = new
                {
                    actorId = script.World.CameraFollow ?? selectedActor.Id
                },
                inventory = selectedActor.GetInventory()
                    .Select(o =>
                    {
                        return new
                        {
                            id = o.Id,
                            name = o.Name,
                            classes = o.Classes
                        };
                    })
            });
        }

        public Activity Idle()
        {
            return CreateEventActivity("Idle");
        }

        public Activity ObjectPickedUp(IObject obj)
        {
            return CreateEventActivity("ObjectPickedUp", new
            {
                @object = new
                {
                    id = obj.Id,
                    owner = obj.Owner
                }
            });
        }

        public Activity InventoryItemAdded(IObject obj)
        {
            return CreateEventActivity("InventoryItemAdded", new
            {
                item = new
                {
                    id = obj.Id,
                    name = obj.Name,
                    classes = obj.Classes
                }
            });
        }

        public Activity InventoryItemRemoved(IObject obj)
        {
            return CreateEventActivity("InventoryItemRemoved", new
            {
                item = new
                {
                    id = obj.Id
                }
            });
        }

        public Activity ObjectPlacedInRoom(IObject obj)
        {
            return CreateEventActivity("ObjectPlacedInRoom", new
            {
                @object = ToDto(obj)
            });
        }

        public Activity ObjectRemovedFromRoom(IObject obj)
        {
            return CreateEventActivity("ObjectRemovedFromRoom", new
            {
                @object = new
                {
                    id = obj.Id,
                }
            });
        }

        public Activity ObjectStateChanged(IObject obj, IEnumerable<IObject> objectsToAdd,
            IEnumerable<IObject> objectsToRemove)
        {
            return CreateEventActivity("ObjectStateChanged", new
            {
                @object = new
                {
                    id = obj.Id,
                    state = obj.State
                },
                add = objectsToAdd.Select(o => ToDto(o)),
                // TODO Too much data.
                remove = objectsToRemove.Select(o => new
                {
                    id = o.Id,
                    name = o.Name,
                    x = o.PositionX,
                    y = o.PositionY,
                    classes = o.Classes,
                    z_offset = o.ZOffset ?? 0,
                    state = o.State
                })
            });
        }

        // TODO Would love to have an IGameScript or something here.
        public Activity RoomEntered(IGameScript script)
        {
            var room = script.World.GetSelectedRoom();

            return CreateEventActivity("RoomEntered", new
            {
                room = new
                {
                    id = room.Id,
                    walkbox = room.Walkbox.Select(p => new
                    {
                        x = p.X,
                        y = p.Y
                    })
                },
                actors = script.Actors
                    .Where(a => a.RoomId == room.Id)
                    .Select(a => ToDto(a)),
                objects = room.GetObjects()
                    .Where(o => o.IsVisible)
                    .Select(o => ToDto(o))
            });
        }

        public Activity LineSpoken(string text)
        {
            var messageActivity = MessageFactory.Text(text);
            messageActivity.Properties = JObject.FromObject(new {
                narrator = true
            });

            return messageActivity;
        }

        public Activity LineSpoken(string text, IActor actor)
        {
            var messageActivity = MessageFactory.Text($"{actor.Name} > {text}");
            messageActivity.Properties = JObject.FromObject(new {
                actor = new
                {
                    id = actor.Id
                }
            });

            return messageActivity;
        }
        
        private Activity CreateEventActivity(string name, object properties = null)
        {
            var eventActivity = _context.Activity.CreateReply();
            eventActivity.Type = "event";
            eventActivity.Name = name;

            if (properties != null)
            {
                eventActivity.Properties = JObject.FromObject(properties);
            }

            return eventActivity;
        }

        private object ToDto(IActor actor)
        {
            return new
            {
                id = actor.Id,
                name = actor.Name,
                x = actor.PositionX,
                y = actor.PositionY,
                classes = actor.Classes,
                textColor = actor.TextColor.Length > 0 ? actor.TextColor : "white",
                usePosition = actor.UsePosition.Length > 0 ? actor.UsePosition : "center",
                useDirection = actor.UseDirection.Length > 0 ? actor.UseDirection : "front",
                faceDirection = actor.FaceDirection.Length > 0 ? actor.FaceDirection : "front"
            };
        }

        private object ToDto(IObject obj)
        {
            return new
            {
                id = obj.Id,
                name = obj.Name,
                x = obj.PositionX,
                y = obj.PositionY,
                z_offset = obj.ZOffset ?? 0,
                classes = obj.Classes,
                state = obj.State,
                usePosition = obj.UsePosition.Length > 0 ? obj.UsePosition : "center",
                useDirection = obj.UseDirection.Length > 0 ? obj.UseDirection : "front"
            };
        }
    }
}