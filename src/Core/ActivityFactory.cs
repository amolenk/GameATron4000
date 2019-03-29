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

namespace GameATron4000.Core
{
    // TODO Interface
    public class ActivityFactory
    {
        private readonly GameInfo _gameInfo;
        private readonly Random _random;

        public ActivityFactory(GameInfo gameInfo)
        {
            _gameInfo = gameInfo;
            _random = new Random();
        }

        public Activity ActorDirectionChanged(DialogContext dc, string actorId,
            ActorDirection direction)
        {
            return CreateEventActivity(dc, "ActorDirectionChanged", new
            {
                actorId = actorId,
                direction = direction.ToString()
            });
        }

        public Activity ActorMoved(DialogContext dc, string actorId, ActorPosition position)
        {
            return CreateEventActivity(dc, "ActorMoved", new
            {
                actorId = actorId,
                x = position.X,
                y = position.Y,
                direction = position.Direction.ToString()
            });
        }

        public Activity ActorPlacedInRoom(DialogContext dc, string actorId, ObjectPosition position)
        {
            var gameActor = _gameInfo.Actors[actorId];

            return CreateEventActivity(dc, "ActorPlacedInRoom", new
            {
                actorId = actorId,
                description = gameActor.Description,
                x = position.X,
                y = position.Y,
                textColor = gameActor.TextColor
            });
        }

        public Activity CannedResponse(DialogContext dc)
        {
            return Speak(
                dc,
                "player",
                _gameInfo.CannedResponses[_random.Next(0, _gameInfo.CannedResponses.Count)]);
        }

        public Activity Delayed(DialogContext dc, int milliseconds)
        {
            return CreateEventActivity(dc, "Delayed", new
            {
                time = milliseconds
            });
        }

        public Activity GameStarted(DialogContext dc)
        {
            return CreateEventActivity(dc, "GameStarted", new
            {
                inventoryItems = _gameInfo.InitialInventory.Select(inventoryItemId => new
                {
                    inventoryItemId = inventoryItemId,
                    description = _gameInfo.InventoryItems[inventoryItemId].Description
                })
            });
        }

        public Activity Idle(DialogContext dc)
        {
            return CreateEventActivity(dc, "Idle");
        }

        public Activity InventoryItemAdded(DialogContext dc, string inventoryItemId)
        {
            var inventoryItem = _gameInfo.InventoryItems[inventoryItemId];

            return CreateEventActivity(dc, "InventoryItemAdded", new
            {
                inventoryItemId = inventoryItemId,
                description = inventoryItem.Description
            });
        }

        public Activity InventoryItemRemoved(DialogContext dc, string inventoryItemId)
        {
            return CreateEventActivity(dc, "InventoryItemRemoved", new
            {
                inventoryItemId = inventoryItemId
            });
        }

        public Activity Narrated(DialogContext dc, string text)
        {
            return CreateEventActivity(dc, "Narrated", new
            {
                text = text
            });
        }

        public Activity ObjectPlacedInRoom(DialogContext dc, string objectId, ObjectPosition position)
        {
            var gameObject = _gameInfo.Objects[objectId];

            return CreateEventActivity(dc, "ObjectPlacedInRoom", new
            {
                objectId = objectId,
                description = gameObject.Description,
                x = position.X,
                y = position.Y,
                foreground = position.Foreground
            });
        }

        public Activity ObjectRemovedFromRoom(DialogContext dc, string objectId)
        {
            return CreateEventActivity(dc, "ObjectRemovedFromRoom", new
            {
                objectId = objectId
            });
        }

        // public Activity RoomEntered(DialogContext dc, string roomId, RoomState roomState, Game game)
        // {
        //     return CreateEventActivity(dc, "RoomEntered", new
        //     {
        //         roomId = roomId,
        //         actors = roomState.ActorPositions.Select(position => new
        //         {
        //             actorId = position.Key,
        //             description = game.Actors[position.Key].Description,
        //             x = position.Value.X,
        //             y = position.Value.Y,
        //             direction = "Front",// position.Value.Direction.ToString(),
        //             textColor = game.Actors[position.Key].TextColor
        //         }),
        //         objects = new object[0]
        //         //  roomState.ObjectPositions.Select(position => new
        //         // {
        //         //     objectId = position.Key,
        //         //     description = _gameInfo.Objects[position.Key].Description,
        //         //     x = position.Value.X,
        //         //     y = position.Value.Y,
        //         //     foreground = position.Value.Foreground
        //         // })
        //     });
        // }

        public Activity Speak(DialogContext dc, string actorId, string text)
        {
            var gameActor = _gameInfo.Actors[actorId];

            var messageActivity = MessageFactory.Text($"{gameActor.Description} > {text}");
            messageActivity.Properties = JObject.FromObject(new {
                actorId = actorId
            });

            return messageActivity;
        }
        
        private static Activity CreateEventActivity(DialogContext dc, string name, object properties = null)
        {
            var eventActivity = dc.Context.Activity.CreateReply();
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