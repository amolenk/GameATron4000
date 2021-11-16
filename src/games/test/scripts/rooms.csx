Room beach = AddRoom(nameof(beach), room => room
    .WithWalkboxArea(
        new Point(0, 370),
        new Point(900, 370),
        new Point(900, 449),
        new Point(0, 449))
    .When.BeforeEnter(() =>
    {
        beach.Place(guy, 490, 420);
        beach.Place(beamPark, 490, 425);
        beach.Place(narrator, 450, 150);
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
    }));
//     scale = {
//         min = 75,
//         max = 75,
//         start_pos = 370,
//         end_pos = 450

Room bridge = AddRoom(nameof(bridge), room => room
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
    .When.BeforeEnter(() =>
    {
        bridge.Place(guy, 65, 440);
        guy.FaceCamera();
    })
    .When.AfterEnter(() =>
    {
        if (!IsFlagSet("visitedBridge"))
        {
            Delay(1000);
            ian.SayLine("No, you can't have access to the reactor core!");
            carl.SayLine("Can we at least have a look at the warp drive?");
            ian.SayLine("NO!");

            ian.MoveTo(455, 430);
            ian.SayLine("Jeez, these earthlings are nosy!");
            ian.MoveTo(810, 300, WellKnownStatus.FaceAwayFromCamera);

            SetFlag("visitedBridge");
        }
    }));
//     scale = {
//         min = 40,
//         max = 100,
//         start_pos = 275,
//         end_pos = 400
//     },


Room park = AddRoom(nameof(park), room => room
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
    .When.BeforeEnter(() =>
    {
        park.Place(guy, 600, 430);
    }));
//     scale = {
//         min = 70,
//         max = 100,
//         start_pos = 270,
//         end_pos = 400
//     },
// }

Room terminal = AddRoom(nameof(terminal), builder => builder
    .WithWalkboxArea(
        new Point(0, 430),
        new Point(180, 335),
        new Point(600, 335),
        new Point(884, 430),
        new Point(884, 449),
        new Point(360, 449),
        new Point(360, 400),
        new Point(80, 400),
        new Point(80, 449),
        new Point(0, 449))
    .When.BeforeEnter(() =>
    {
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
//            Delay(1000);
            SayLine("GET ME OUT OF HERE!");
        }
    }));
//     scale = {
//         min = 80,
//         max = 100,
//         start_pos = 325,
//         end_pos = 400
//     },
