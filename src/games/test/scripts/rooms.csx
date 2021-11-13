// beach = {
//     id = "beach",
//     type = "room",
//     scale = {
//         min = 75,
//         max = 75,
//         start_pos = 370,
//         end_pos = 450
//     },
//     walkbox = {
//         { 0, 370 },
//         { 900, 370 },
//         { 900, 449 },
//         { 0, 449}
//     },
//     before_enter = function(room)
//         put_actor(guy, 490, 420)
//         put_object(beam_park, 490, 425)
//     end,
//     after_enter = function(room)
//         wait(1500)
//         remove_object(beam_park)
//         wait(1000)
//         say_line("That's just great!")
//         say_line("I hope there's a supermarket around here...")
//         wait(1000)
//         walk_to(850, 430)
//         say_line("Thanks for playing!", narrator)
//         say_line("GAME OVER", narrator)
//     end
// }

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
    .When.BeforeEnter(() => bridge.Place(guy, 65, 440))
    // .When.AfterEnter(() =>
    // {
//         if not world.visited_bridge then
//             wait(1000)
//             say_line("No, you can't have access to the reactor core!", ian)
//             say_line("Can we at least have a look at the warp drive?", carl)
//             say_line("NO!", ian)
//             walk_to(455, 430, face_front, ian)
//             say_line("Jeez, these earthlings are nosy!", ian)
//             walk_to(810, 300, face_back, ian)
//             world.visited_bridge = true
    // })
    );
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
            terminal.Place(guy, 440, 363);
        }
        else
        {
            terminal.Place(guy, 665, 365);
        }
    })
    // .When.AfterEnter(() =>
    // {
    //     if (PreviousRoom == park)
    //     {
    //         Delay(1000);
    //         SayLine("GET ME OUT OF HERE!");
    //     }
    // })
    );
//     scale = {
//         min = 80,
//         max = 100,
//         start_pos = 325,
//         end_pos = 400
//     },
