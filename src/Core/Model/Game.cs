namespace Amolenk.GameATron4000.Model;

public class Game
{
    private readonly List<GameObject> _objects;
    private readonly List<Room> _rooms;
    private Action? _onStart;

    public Actor? Protagonist => State.Protagonist;

    public Room? PreviousRoom { get; private set; } // TODO Move to state
    public Room? CurrentRoom { get; private set; } // TODO Move to state
    public GlobalState State { get; private set; }

    internal EventQueue EventQueue { get; private set; }

    internal Game(EventQueue eventQueue)
    {
        _objects = new();
        _rooms = new();

        EventQueue = eventQueue;
        State = new();
    }

    public ActorBuilder Actor(string id) =>
        new ActorBuilder(id, this);

    public GameObjectBuilder Object(string id) =>
        new GameObjectBuilder(id, this);

    public RoomBuilder Room(string id) =>
        new RoomBuilder(id, this);

    public void OnGameStart(Action onStart) => _onStart = onStart;

    public void Delay(int value) =>
        EventQueue.Enqueue(new DelayRequested(TimeSpan.FromMilliseconds(value)));

    public void SayLine(string line) => State.Protagonist?.SayLine(line);

    public void SetProtagonist(Actor actor)
    {
        State.Protagonist = actor;

        EventQueue.Enqueue(new ProtagonistChanged(actor));
    }

    internal void Start()
    {
        if (_onStart is not null)
        {
            _onStart();
        }
    }

    

    // internal void NotifyRoomAdded(Room room)
    // {
    //     _rooms.Add(room);
    // }

    internal void NotifyRoomEntered(Room room)
    {
        PreviousRoom = CurrentRoom;
        CurrentRoom = room;
    }
}

// TODO Port!

//         [LuaGlobal(Name = "rand")]
//         public int Random(int min, int max)
//         {
//             return _random.Next(min, max + 1);
//         }

//         [LuaGlobal(Name = "camera_follow")]
//         public void CameraFollow(LuaTable actorTable)
//         {
//             var actor = LuaActor.FromTable(actorTable, _script);

// //            _script.World.CameraFollow = actor.Id;

//             Result.Activities.Add(_activityFactory.CameraFocusChanged(actor));
//         }

//         [LuaGlobal(Name = "option")]
//         public void ConversationOption(string text, string action, bool? predicate = null)
//         {
//             if (!predicate.HasValue || predicate.Value)
//             {
//                 Result.ConversationOptions.Add(action, text);
//             }
//         }

//         [LuaGlobal(Name = "face_dir")]
//         public void FaceDirection(string direction, LuaTable actorTable = null)
//         {
//             var actor = actorTable != null
//                 ? LuaActor.FromTable(actorTable, _script)
//                 : _script.Actors.First(a => a.Id == _script.World.SelectedActorId);

//             actor.FaceDirection = direction;

//             Result.Activities.Add(_activityFactory.ActorDirectionFacedChanged(direction, actor));
//         }

//         [LuaGlobal(Name = "owned_by")]
//         public bool OwnedBy(LuaTable objectTable, LuaTable actorTable)
//         {
//             var obj = LuaObject.FromTable(objectTable, _script);
//             var actorId = actorTable != null
//                 ? LuaActor.FromTable(actorTable, _script).Id
//                 : _script.World.SelectedActorId;

//             return obj.Owner == actorId;
//         }

//         [LuaGlobal(Name = "start_talking")]
//         public void StartTalking(string conversationId)
//         {
//             Result.NextDialogId = conversationId;
//             Result.NextDialogReplace = false;
//         }

//         [LuaGlobal(Name = "wait")]
//         public void Wait(int milliseconds)
//         {
//             Result.Activities.Add(_activityFactory.Halted(milliseconds));
//         }

//         [LuaGlobal(Name = "walk_to")]
//         public void WalkTo(int x, int y, string faceDirection = null, LuaTable actorTable = null)
//         {
//             var actor = GetActor(actorTable);
//             actor.PositionX = x;
//             actor.PositionY = y;
//             actor.FaceDirection = faceDirection; 

//             Result.Activities.Add(_activityFactory.ActorMoved(actor));
//         }

//         // TODO Make sure everything uses this
//         private IActor GetActor(LuaTable actorTable = null)
//         {
//             return actorTable != null
//                 ? LuaActor.FromTable(actorTable, _script)
//                 : _script.Actors.First(a => a.Id == _script.World.SelectedActorId);
//         }
//     }
// }