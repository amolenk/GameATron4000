//using System;
//using GameATron4000.Core.Messages;
//using GameATron4000.Core.Services;
//using Microsoft.Extensions.DependencyInjection;

//namespace GameATron4000.Application
//{
//    public class Game
//    {
//        private readonly IServiceScope _scope;

//        private Game(IServiceScope scope)
//        {
//            _scope = scope;
//        }

//        public static Game Load(IServiceProvider serviceProvider)
//        {
//            var scope = serviceProvider.CreateScope();

//            // TODO Register GameInfo object in scope.

//            return new Game(scope);
//        }

//        public void Start()
//        {
//            var mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
//            mediator.Send(new StartGameCommand());
//        }
//    }
//}
