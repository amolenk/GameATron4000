// using System.Collections.Generic;
// using System.Linq;
// using Microsoft.Bot.Builder.Dialogs;
// using Newtonsoft.Json;
// using NLua;

// namespace GameATron4000.Models
// {
//     public class GameInfo
//     {
//         public GameInfo()
//         {
//             Actors = new Dictionary<string, GameActor>();
//             Objects = new Dictionary<string, GameObject>();
//             InventoryItems = new Dictionary<string, GameObject>();
//             InitialInventory = new List<string>();
//             CannedResponses = new List<string>();
//             InitialRoomStates = new Dictionary<string, RoomState>();
//             RoomScripts = new Dictionary<string, string>();
//             ConversationScripts = new Dictionary<string, string>();
//         }

//         public string InitialRoom { get; set; }

//         public Dictionary<string, GameActor> Actors { get; set; }

//         public Dictionary<string, GameObject> Objects { get; set; }

//         public Dictionary<string, GameObject> InventoryItems { get; set; }

//         public List<string> InitialInventory { get; set; }

//         public List<string> CannedResponses { get; set; }

//         public Dictionary<string, RoomState> InitialRoomStates { get; set; }

//         public Dictionary<string, string> RoomScripts { get; set; }

//         public Dictionary<string, string> ConversationScripts { get; set; }

//         [LuaGlobal(Name = "register_actor")]
//         public void RegisterActor(string id, string description, string textColor)
//         {
//             Actors.Add(id, new GameActor { Description = description, TextColor = textColor });
//         }

//         [LuaGlobal(Name = "register_object")]
//         public void RegisterObject(string id, string description)
//         {
//             Objects.Add(id, new GameObject { Description = description });
//         }

//         [LuaGlobal(Name = "register_inventory_item")]
//         public void RegisterInventoryItem(string id, string description, bool inInitialInventory = false)
//         {
//             InventoryItems.Add(id, new GameObject { Description = description });

//             if (inInitialInventory)
//             {
//                 InitialInventory.Add(id);
//             }
//         }

//         [LuaGlobal(Name = "register_canned_response")]
//         public void RegisterCannedResponse(string response)
//         {
//             CannedResponses.Add(response);
//         }

//         [LuaGlobal(Name = "set_initial_room")]
//         public void SetInitialRoom(string roomId)
//         {
//             this.InitialRoom = roomId;
//         }
//     }
// }