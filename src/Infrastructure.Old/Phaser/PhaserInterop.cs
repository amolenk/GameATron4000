//namespace Amolenk.GameATron4000.Infrastructure.Phaser;

//public class PhaserInterop
//{
//    private readonly IJSRuntime _js;

//    public PhaserInterop(IJSRuntime js)
//    {
//        _js = js;
//    }

//    public async Task RegisterSceneAsync<T>(T scene, bool active = false)
//        where T : PhaserScene
//    {
//        var phaserScene = new PhaserSceneRenderer(_js, scene);

//        var imageAssets = scene.ImageUrls.Select(url =>
//            new ImageAsset(
//                "image",
//                // TODO Scope to scene
//                $"{Path.GetFileNameWithoutExtension(url)}",
//                url));

//        Console.WriteLine(JsonSerializer.Serialize(imageAssets));

//        await _js.InvokeVoidAsync(
//            "registerScene",
//            scene.Id,
//            DotNetObjectReference.Create(phaserScene),
//            imageAssets,
//            active);
//    }

//    //public async Task RegisterRoomSceneAsync(Room room)
//    //{
//    //    var scene = new RoomScene(room, this, _js);
//    //    var sceneKey = $"room-{room.Id}";

//    //    var imageAssets = room.ImageUrls.Select(url =>
//    //        new ImageAsset(
//    //            "image",
//    //            $"{sceneKey}-{Path.GetFileNameWithoutExtension(url)}",
//    //            url));

//    //    await _js.InvokeVoidAsync(
//    //        "registerScene",
//    //        sceneKey,
//    //        DotNetObjectReference.Create(scene),
//    //        imageAssets);
//    //}

//    public async Task StartPhaser()
//    {
//        await _js.InvokeVoidAsync("startPhaser", "phaser");
//    }

//    public async Task AddImage(string scene, int x, int y, string image)
//    {
//        await _js.InvokeVoidAsync(
//            "addImage",
//            scene,
//            x,
//            y,
//            image);
//    }
//}
