Item bridgeDoor = AddItem(nameof(bridgeDoor), builder => builder
    .Named("door")
    .WithDepthOffset(-100)
    .When.WalkTo(() =>
    {
        ChangeRoom(terminal);
    }));

Item counterTop = AddItem(nameof(counterTop), builder => builder
    .Named("counter top")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .WithDepthOffset(-10)
    .When.LookAt(() =>
    {
        SayLine("It's a countertop, not much to add here.");
    }));

Item crateLeft = AddItem(nameof(crateLeft), builder => builder
    .Named("left crate")
    .Untouchable());

Item crateRight = AddItem(nameof(crateRight), builder => builder
    .Named("right crate")
    .WithStatus("closed")
    .Untouchable());

Item crateTop = AddItem(nameof(crateTop), builder => builder
    .Named("top crate")
    .WithDepthOffset(20)
    .Untouchable());

Item fridge = AddItem(nameof(fridge), builder => builder
    .WithActorInteraction(status: WellKnownStatus.FaceAwayFromCamera)
    .WithDepthOffset(10)
    .WithStatus("closed"));

Item onAir = AddItem(nameof(onAir), builder => builder
    .WithStatus("on")
    .Untouchable());

Item outletBooth = AddItem(nameof(outletBooth), builder => builder
    .Untouchable());

Item outletKitchen = AddItem(nameof(outletKitchen), builder => builder
    .Untouchable());

Item podcastBooth = AddItem("podcastBooth", builder => builder
    .Named("podcast booth")
    .WithActorInteraction(status: WellKnownStatus.FaceAwayFromCamera)
    .WithDepthOffset(-400)
    .When.LookAt(() =>
    {
        SayLine("It's a podcast booth!");
    }));

Item powerCord = AddItem("powerCord", builder => builder
    .Named("power cord")
    .CanBeUsedWithOtherObject()
    .WithActorInteraction(RelativePosition.Center, WellKnownStatus.FaceAwayFromCamera)
    .WithDepthOffset(-35)
    .WithStatus("booth")
    .When.LookAt(() =>
    {
        SayLine("Looks like the podcast booth gets it electricity from the ship.");
        guy.FaceCamera();
        SayLine("I wonder how the aliens feel about that.");
    }));

Item terminalDoor = AddItem(nameof(terminalDoor), builder => builder
    .Named("door")
    .WithStatus("open")
    .WithActorInteraction(status: WellKnownStatus.FaceAwayFromCamera)
    .When.WalkTo(() =>
    {
        ChangeRoom(bridge);
    }));
