//using System;
//using System.Threading.Tasks;
//using MoonSharp.Interpreter;
//using GameATron4000.Core.Messages;
//using GameATron4000.Core.Services;
//using System.Text;

//namespace GameATron4000.Infrastructure.Lua
//{
//    public class LuaGameScriptInterpreter : IGameScriptInterpreter
//    {
//        private const string FUNCTION_INITIALIZE_GAME = "initialize_game";
//        private const string FUNCTION_BEFORE_ENTER = "before_enter";
//        private const string FUNCTION_AFTER_ENTER = "after_enter";

//        private readonly IMediator _mediator;
//        private readonly Script _script;

//        public IWorldState World => throw new NotImplementedException();

//        //private readonly LuaFunctions _luaFunctions;

//        public LuaGameScriptInterpreter(IMediator mediator)
//        {
//            _mediator = mediator;
//            _script = new Script();

//            // TODO Create sandbox: https://www.moonsharp.org/sandbox.html
//            var baseScriptCode = $@"                
//                -- Constants
//                class_fixed_to_camera = ""class_fixed_to_camera""
//                class_invisible = ""class_invisible""
//                class_untouchable = ""class_untouchable""
//                class_use_with = ""class_use_with""
                
//                face_back = ""back""
//                face_front = ""front""

//                pos_infront = ""infront""
//                pos_center = ""center""
//                pos_above = ""above""
//                pos_none = ""none""

//                narrator = {{
//                    type = ""narrator""
//                }}

//                world = {{
//                    type = ""world""
//                }}

//                start_cutscene = function(cutscene)
//                    selected_cutscene = cutscene
//                    cutscene.start()
//                end

//                end_cutscene = function()
//                    selected_cutscene = nil
//                end

//                initialize_game = function()
//                    camera_follow()
//                end

//                ";

//            _script.DoString(baseScriptCode);

//            // Make the game engine functions available to the Lua script.
//            //_script.Globals["camera_follow"] = (Action)Callback;

//            //_luaFunctions = new LuaFunctions(_mediator, this);
//            //LuaRegistrationHelper.TaggedInstanceMethods(_lua, _luaFunctions);

//            //_lua.DoFile("Gameplay/scripts/game.lua");
//            //_lua.DoFile("Gameplay/scripts/actors.lua");
//            //_lua.DoFile("Gameplay/scripts/objects.lua");
//            //_lua.DoFile("Gameplay/scripts/rooms.lua");

//            //foreach (var path in Directory.GetFiles("Gameplay/scripts/conversations", "*.lua"))
//            //{
//            //    _lua.DoFile(path);
//            //}

//            //_initialScriptState = LuaGameScriptState.Save(_lua);
//        }

//        public event ChangeRoomCallback ChangeRoomCallback;

//        public event PutItemCallback PutItemCallback;

//        public void StartGame()
//        {
//            throw new NotImplementedException();
//        }

//        public void AfterEnterRoom(string roomId)
//        {
//            throw new NotImplementedException();
//        }

//        public void BeforeEnterRoom(string roomId)
//        {
//            throw new NotImplementedException();
//        }

//        public void Close()
//        {
//            throw new NotImplementedException();
//        }

//        public void Give()
//        {
//            throw new NotImplementedException();
//        }

//        public void LookAt()
//        {
//            throw new NotImplementedException();
//        }

//        public void Open()
//        {
//            throw new NotImplementedException();
//        }

//        public void PickUp()
//        {
//            throw new NotImplementedException();
//        }

//        public void Pull()
//        {
//            throw new NotImplementedException();
//        }

//        public void Push()
//        {
//            throw new NotImplementedException();
//        }

//        public void TalkTo()
//        {
//            throw new NotImplementedException();
//        }

//        public void Use()
//        {
//            throw new NotImplementedException();
//        }



//        //private void RunRoomFunction(string functionName)
//        //{
//        //    //var room = GetTable(LuaConstants.Tables.Id, World.CurrentRoomId, LuaConstants.Tables.Types.Room);

//        //    //var function = room[functionName] as LuaFunction;
//        //    //if (function != null)
//        //    //{
//        //    //    function.Call(room);
//        //    //}

//        //    //return _luaFunctions.Result;

//        //    _script.Call(_script.Globals[functionName]);

//        //    // TODO Use this code below to guard code above.
//        //    //if (_lua[functionName] is LuaFunction function)
//        //    //{
//        //    //    function.Call();
//        //    //}
//        //}
//    }
//}
