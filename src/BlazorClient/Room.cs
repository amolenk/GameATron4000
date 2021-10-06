//public class Room
//{
//    public Room(string id, IEnumerable<string> imageUrls)
//    {
//        Id = id;
//        ImageUrls = imageUrls;
//    }

//    public string Id { get; }

//    public IEnumerable<string> ImageUrls { get; }

//    public async Task EnterRoomAsync(PhaserSceneInterop scene)//, IRenderingEngine renderingEngine)
//    {
////        await renderingEngine.SetRenderLockAsync(true);

//        //try
//        //{
//            // Clear all current actors & objects from the room.
//            //await ClearAsync();


//            await scene.AddImage(Id, 750, 150);


//            //                    this.game.lockRender = true;
//            //                    if (this.room != null) {
//            //                        this.room.kill();
//            //                    }

//            //                    const walkbox = new Phaser.Polygon(
//            //                        event.room.walkbox.map((p: any) => new Phaser.Point(p.x, p.y)));

//            //                    this.room = new Room(event.room.id, event.room.scale, walkbox);
//            //                    this.room.create(this.game, this, this.layers);

//            //                    for (let actor of event.actors) {
//            //                        this.room.addActor(
//            //                            new Actor(
//            //                                actor.id,
//            //                                actor.name,
//            //                                actor.classes,
//            //                                actor.usePosition,
//            //                                actor.useDirection,
//            //                                actor.faceDirection,
//            //                                actor.textColor),
//            //                            actor.x,
//            //                            actor.y);
//            //                    }

//            //                    for (let obj of event.objects) {
//            //                        this.room.addObject(
//            //                            new RoomObject(obj.id, obj.name, obj.classes, obj.state, obj.usePosition, obj.useDirection),
//            //                            obj.x,
//            //                            obj.y,
//            //                            obj.z_offset);
//            //                    }

//            //                    this.game.lockRender = false;
//            //                    break;
//            //                }

//        //}
//        //finally
//        //{
//        //    await renderingEngine.SetRenderLockAsync(false);
//        //}


//    }

//    //private async Task ClearAsync()
//    //{
//    //    //for (var object of this.roomObjects)
//    //    //{
//    //    //    object.kill();
//    //    //}
//    //    //for (var actor of this.actors)
//    //    //{
//    //    //    actor.kill();
//    //    //}
//    //}
//}

