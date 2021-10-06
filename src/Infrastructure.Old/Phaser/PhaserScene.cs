//namespace Amolenk.GameATron4000.Infrastructure.Phaser;

//public abstract class PhaserScene2
//{
//    protected PhaserScene2(string id)
//    {
//        Id = id;
//    }

//    public string Id { get; }

//    public virtual string[] ImageUrls => new string[0];

//    public virtual Task CreateAsync()
//    {
        
//    }

//    public virtual Task UpdateAsync(ISceneRenderer renderer) =>
//        Task.CompletedTask;


//    [JSInvokable()]
//    public async Task Create(string key)
//    {
//        //Console.WriteLine($"Scene called with key {key}, waiting a while...");

//        //await Task.Delay(2000);

//        //Console.WriteLine("Done waiting");

//        await _room.EnterRoomAsync(new PhaserSceneInterop(key, _js));
//    }
//}

