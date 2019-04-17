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

        public Activity ActorDirectionChanged(string actorId, ActorDirection direction)
        {
            return CreateEventActivity("ActorDirectionChanged", new
            {
                actorId = actorId,
                direction = direction.ToString()
            });
        }

        public Activity ActorMoved(string actorId, ActorPosition position)
        {
            return CreateEventActivity("ActorMoved", new
            {
                actorId = actorId,
                x = position.X,
                y = position.Y,
                direction = position.Direction.ToString()
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
                @object = new
                {
                    id = obj.Id,
                    name = obj.Name,
                    x = obj.PositionX,
                    y = obj.PositionY,
                    classes = obj.Classes,
                    z_offset = obj.ZOffset ?? 0,
                    state = obj.State
                }
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

        public Activity ObjectStateChanged(IObject obj)
        {
            return CreateEventActivity("ObjectStateChanged", new
            {
                @object = new
                {
                    id = obj.Id,
                    state = obj.State
                }
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
                    id = room.Id
                },
                actors = script.Actors
                    .Where(a => a.RoomId == room.Id)
                    .Select(a => new
                    {
                        id = a.Id,
                        name = a.Name,
                        x = a.PositionX,
                        y = a.PositionY,
                        classes = a.Classes,
                        direction = "Front", // TODO
                        textColor = a.TextColor
                    }),
                objects = room.GetObjects()
                    .Where(o => o.IsVisible)
                    .Select(o => new
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
    }
}