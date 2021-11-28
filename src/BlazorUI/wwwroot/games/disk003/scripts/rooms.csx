Room beach = AddRoom("beach", room => room
    .WithWalkboxArea(
        new Point(0, 370),
        new Point(900, 370),
        new Point(900, 449),
        new Point(0, 449))
    .WithScaleSettings(75, 75, 370, 450)
    .When.BeforeEnter(() =>
    {
        beach.Place(guy, 490, 420);
        beach.Place(beamPark, 490, 425);
        beach.Place(narrator, 400, 150);

        guy.FaceCamera();
    })
    .When.AfterEnter(() =>
    {
        Delay(1500);

        beach.Remove(beamPark);
        Delay(1000);

        SayLine("That's just great!");
        SayLine("I hope there's a supermarket around here...");
        Delay(1000);

        guy.MoveTo(850, 530);
    
        narrator.SayLine("Thanks for playing!");
        narrator.SayLine("GAME OVER");

        GameOver();
    }));

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
        bridge.Place(shipComputer, 400, 150);
        bridge.Place(guy, 65, 440);
        guy.FaceCamera();
    })
    .When.AfterEnter(() =>
    {
        if (!IsFlagSet("visited-bridge"))
        {
            Delay(1000);
            ian.SayLine("No, you can't have access to the reactor core!");
            carl.SayLine("Can we at least have a look at the warp drive?");
            ian.SayLine("NO!");

            ian.MoveTo(455, 430);
            //ian.FaceCamera();
            ian.SayLine("Jeez, these earthlings are nosy!");
            ian.MoveTo(810, 300, WellKnownStatus.FaceAwayFromCamera);

            SetFlag("visited-bridge");
        }
    })
    );

Room park = AddRoom("park", room => room
    .WithWalkboxArea(
        new Point(935, 295),
        new Point(935, 318),
        new Point(710, 375),
        new Point(710, 385),
        new Point(935, 442),
        new Point(935, 449),
        new Point(591, 449),
        new Point(393, 375),
        new Point(108, 336),
        new Point(0, 330),
        new Point(0, 312),
        new Point(130, 315),
        new Point(366, 338),
        new Point(470, 355),
        new Point(804, 316))
    .WithScaleSettings(70, 100, 270, 400)
    .When.BeforeEnter(() =>
    {
        park.Place(guy, 600, 430);
    }));

Room terminal = AddRoom("terminal", builder => builder
    .WithWalkboxArea(
        new Point(0, 0),
        new Point(800, 0),
        new Point(800, 600),
        new Point(0, 600))
    .WithScaleSettings(70, 100, 325, 400)
    .When.BeforeEnter(() =>
    {
        terminal.Place(shipComputer, 400, 150);

        if (PreviousRoom == park)
        {
            terminal.Place(guy, 442, 363);
        }
        else
        {
            terminal.Place(guy, 665, 365);
        }
    })
    .When.AfterEnter(() =>
    {
        if (PreviousRoom == park)
        {
           Delay(1000);
           guy.SayLine("GET ME OUT OF HERE!");
        }
    }));
