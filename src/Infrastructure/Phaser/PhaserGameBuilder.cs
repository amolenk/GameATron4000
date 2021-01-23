//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace GameATron4000.Infrastructure.Phaser
//{
//    public class PhaserGameBuilder
//    {
//        private readonly IPhaserInterop _phaser;
//        private readonly IServiceProvider _serviceProvider;
//        private string _playerName;
//        private List<Type> _sceneTypes; 
//        private Dictionary<Type, Action<object, IStateManager>> _handlers;

//        public PhaserGameBuilder(IPhaserInterop phaser, IServiceProvider serviceProvider)
//        {
//            _phaser = phaser;
//            _serviceProvider = serviceProvider;
//            _sceneTypes = new List<Type>();
//            _handlers = new Dictionary<Type, Action<object, IStateManager>>();
//        }

//        public PhaserGameBuilder WithPlayerName(string playerName)
//        {
//            _playerName = playerName;
//            return this;
//        }

//        public PhaserGameBuilder WithScene<T>() where T : Scene
//        {
//            _sceneTypes.Add(typeof(T));
//            return this;
//        }

//        public PhaserGameBuilder WithEventHandler<T>(IEventHandler<T> handler)
//        {
//            _handlers.Add(typeof(T), (@event, stateManager) => handler.Handle((T)@event, stateManager));
//            return this;
//        }

//        public PhaserGame Build()
//        {
//            var stateManager = new StateManager(_playerName);

//            return new PhaserGame(
//                _phaser,
//                stateManager,
//                _sceneTypes.Select(sceneType => RegisterSceneInstance(sceneType, stateManager)),
//                _handlers);
//        }

//        private Scene RegisterSceneInstance(Type sceneType, IStateManager stateManager)
//        {
//            var scene = (Scene)_serviceProvider.GetService(sceneType);
//            if (scene == null)
//            {
//                throw new InvalidOperationException($"Scene of type '{sceneType}' could not be instantiated."
//                    + " Make sure it's properly configured with the dependency injection framework.");
//            }

//            scene.Initialize(_phaser, stateManager);

//            _phaser.RegisterScene(scene);

//            return scene;
//        }
//    }
//}