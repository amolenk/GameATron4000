Actor narrator = AddActor("narrator", builder => builder
    .FixedToCamera()
    .Untouchable()
    .WithTextColor("red")
    .WithStatus("transparent"));

Actor carl = AddActor("carl", builder => builder
    .Named("Azure Airways agent")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera, -30, 60)
    .WithTextColor("#48CC1A")
    .When.Push(() =>
    {
        CallApi(
            "https://localhost:7074/lounges",
            new Dictionary<string, string>
            {
                { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "guy" }
            },
            (content) =>
            {
                carl.SayLine(content); 
            },
            (content) =>
            {
                carl.SayLine("No");
            });
    })
    .When.TalkTo(() => StartDialogue(loungeDialogue)));
//     {
//         guy.SayLine("Can I get in the lounge?");
//         carl.SayLine("Let me check my records...");
//     
//         narrator.SayLine("*bleep* *boop* *bleep*");
//             
//         CallApi(
//             "https://localhost:7074/lounge",
//             new Dictionary<string, string>
//             {
//                 { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "guy" }
//             },
//             (content) =>
//             {
//                 Delay(1000);
//                 lobby.Remove(loungeDoorClosed);
//                 lobby.Place(loungeDoorOpen, 394, 320);
//                 guy.SayLine("It's beautiful!");
//             },
//             (content) =>
//             {
//                 Delay(1000);
//                 carl.SayLine("No");
//             });
//     }));

Actor guy = AddActor("guy", builder => builder
    .Untouchable());

Actor richard = AddActor("richard", builder => builder
    .Named("aviation guru")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .WithTextColor("#F4FF6B")
    .When.TalkTo(() => StartDialogue(checkInDialogue))
    .When.WalkTo(() =>
    {
        guy.MoveTo(320, 330, WellKnownStatus.FaceAwayFromCamera);
    }));
