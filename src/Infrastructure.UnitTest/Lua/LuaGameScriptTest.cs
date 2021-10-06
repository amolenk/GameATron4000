//using System;
//using FakeItEasy;
//using GameATron4000.Core.Messages;
//using GameATron4000.Core.Services;
//using GameATron4000.Infrastructure.Lua;
//using Xunit;

//namespace GameATron4000.Infrastructure.UnitTest.Lua
//{
//    public class LuaGameScriptTest
//    {
//        [Fact]
//        public void LuaGameScript()
//        {
//            // Arrange
//            var mockMediator = A.Fake<IMediator>();
//            var luaGameScript = new LuaGameScript(mockMediator);

//            luaGameScript.Handle(new BeforeRoomEnteredEvent());
//        }
//    }
//}
