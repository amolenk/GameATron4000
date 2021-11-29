Room bridge = AddRoom("bridge", room => room
    .WithWalkboxArea(
        new Point(0, 420),
        new Point(390, 420),
        new Point(390, 375),
        new Point(485, 375),
        new Point(520, 430),
        new Point(613, 430),
        new Point(600, 380),
        new Point(685, 332),
        new Point(715, 332),
        new Point(730, 300),
        new Point(760, 290),
        new Point(910, 290),
        new Point(940, 300),
        new Point(955, 332),
        new Point(985, 332),
        new Point(1070, 380),
        new Point(1057, 430),
        new Point(1150, 430),
        new Point(1150, 415),
        new Point(1740, 415),
        new Point(1740, 449),
        new Point(0, 449))
    .WithScaleSettings(40, 100, 275, 400)
    .When.BeforeEnter(() =>
    {
        bridge.Place(guy, 65, 440);
        guy.FaceCamera();
    }));

Room terminal = AddRoom("terminal", builder => builder
    .WithWalkboxArea(
        new Point(0, 430),
        new Point(180, 335),
        new Point(600, 335),
        new Point(884, 430),
        new Point(884, 449),
        new Point(360, 449),
        new Point(360, 390),
        new Point(80, 390),
        new Point(80, 449),
        new Point(0, 449))
    .WithScaleSettings(80, 100, 325, 400)
    .When.BeforeEnter(() =>
    {
        if (PreviousRoom == bridge)
        {
            terminal.Place(guy, 665, 365);
        }
        else
        {
            terminal.Place(guy, 442, 363);
        }
    }));
