
// -- Dummy object to hold unused inventory items.
// nobody = {
//     id = "nobody",
//     type = "actor"
// }

Actor narrator = Actor("narrator")
    .Invisible()
    .WithScrollFactor(0)
    .WithTextColor("lightblue")
    .Build();

Actor al = Actor("al")
    .Named("Al")
    .WithTextColor("Yellow")
    .InteractFromPosition(RelativePosition.InFront)
    .InteractInState(WellKnownState.FaceAwayFromCamera)
    .When.LookAt(() =>
    {
        narrator.SayLine("I'm in the sky!!!!");

        SayLine("I guess it's an alien. Hard to see at this resolution.");
        SayLine("Hi, I'm selling these fine leather jackets.");
        SayLine("Look behind you, a three-headed monkey!");
        SayLine("Doh!");

        al.SayLine("Hi there!");

        // handlers.HandlePush();
    })
//     verbs = {
//         look_at = function(actor)
//             say_line("I guess it's an alien. Hard to see at this resolution.")
//         end,
//         talk_to = function(actor)
//             if not world.alarm then
//                 start_talking("meet_al")
//             else
//                 say_line("MY HOT DOGS!", al)
//             end
//         end
//     }
// }
    .Build();

var guy = Actor("guy")
    .Untouchable()
    .Build();

// ian = {
//     id = "ian",
//     type = "actor",
//     name = "Ian",
//     text_col = "Magenta",
//     use_pos = pos_infront,
//     use_dir = face_back,
//     verbs = {
//         look_at = function(actor)
//             say_line("It's an alien looking fellow!")
//             say_line("He looks a bit more important than the other one!")
//             say_line("Hey, I'm right here!", al)
//         end,
//         talk_to = function(actor)
//             if not world.alarm then
//                 start_talking("meet_ian")
//             else
//                 say_line("Not now, can't you see we're dealing with an emergency?", ian)
//             end
//         end
//     }
// }

// carl = {
//     id = "carl",
//     type = "actor",
//     name = "Richard & Carl",
//     text_col = "#48CC1A",
//     use_pos = pos_none,
//     verbs = {
//         talk_to = function(actor)
//             richard.verbs.talk_to(richard)
//         end,
//         walk_to = function(actor)
//             richard.verbs.walk_to(richard)
//         end
//     }
// }

// richard = {
//     id = "richard",
//     type = "actor",
//     name = "Richard & Carl",
//     text_col = "#f4ff6b",
//     use_pos = pos_none,
//     verbs = {
//         talk_to = function(actor)
//             walk_to(340, 430, face_back)
//             start_talking("talk_to_richard")
//         end,
//         walk_to = function(actor)
//             walk_to(340, 430, face_back)
//         end
//     }
// }