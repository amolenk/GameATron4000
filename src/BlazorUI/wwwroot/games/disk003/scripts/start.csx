Actor guy = AddActor("guy", builder => builder
    .Untouchable());

Item terminalDoor = AddItem(nameof(terminalDoor), builder => builder
    .Named("door")
    .WithDepthOffset(-100)
    .WithStatus("open"));

Item crateLeft = AddItem(nameof(crateLeft), builder => builder
    .Untouchable());

Item crateRight = AddItem(nameof(crateRight), builder => builder
    .Untouchable()
    .WithStatus("closed"));

Item crateTop = AddItem(nameof(crateTop), builder => builder
    .Named("top crate")
    .When.LookAt(() =>
    {
        SayLine("It's a decidedly low-tech wooden crate.");
    }));

Room terminal = AddRoom("terminal", builder => builder
    .WithWalkboxArea(
        new Point(0, 0),
        new Point(800, 0),
        new Point(800, 600),
        new Point(0, 600))
    .WithScaleSettings(80, 100, 325, 400));

OnGameStart(() =>
{
    // terminal
    terminal.Place(crateLeft, 148, 450);
    terminal.Place(crateRight, 256, 450);
    terminal.Place(crateTop, 204, 390);
    terminal.Place(terminalDoor, 774, 332);
    terminal.Place(guy, 442, 363);

    SetProtagonist(guy);

    ChangeRoom(terminal);
});
