// TODO Nameof

Actor narrator = AddActor("narrator", builder => builder
    .FixedToCamera()
    .Untouchable()
    .WithTextColor("lightblue")
    .WithStatus("transparent"));

Actor shipComputer = AddActor(nameof(shipComputer), builder => builder
    .FixedToCamera()
    .Untouchable()
    .WithTextColor("red")
    .WithStatus("transparent"));

Actor al = AddActor("al", builder => builder
    .Named("Al")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .WithTextColor("Yellow")
    .When.LookAt(() =>
    {
        SayLine("I guess it's an alien. Hard to see at this resolution.");
    })
    .When.TalkTo(() =>
    {
        if (IsFlagSet("alarm"))
        {
            al.SayLine("MY HOT DOGS!");
        }
        else
        {
            //StartDialogue("meetAl");
        }
    }));

Actor carl = AddActor("carl", builder => builder
    .Named("Richard & Carl")
    .WithActorInteraction(RelativePosition.None)
    .WithTextColor("#48CC1A")
    .When.TalkTo(() => richard.ActionHandlers.HandleTalkTo())
    .When.WalkTo(() => richard.ActionHandlers.HandleWalkTo()));

Actor guy = AddActor("guy", builder => builder
    .Untouchable());

Actor ian = AddActor("ian", builder => builder
    .Named("Ian")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .WithTextColor("magenta")
    .When.LookAt(() =>
    {
        SayLine("It's an alien looking fellow!");
        SayLine("He looks a bit more important than the other one!");
        al.SayLine("Hey, I'm right here!");
    })
    .When.TalkTo(() =>
    {
        if (IsFlagSet("alarm"))
        {
            ian.SayLine("Not now, can't you see we're dealing with an emergency?");
        }
        else
        {
            //StartDialogue("meetIan");
        }
    }));

Actor richard = AddActor("richard", builder => builder
    .Named("Richard & Carl")
    .WithActorInteraction(RelativePosition.None)
    .WithTextColor("#F4FF6B")
    .When.TalkTo(() =>
    {
        guy.MoveTo(340, 430, WellKnownStatus.FaceAwayFromCamera);
        StartDialogue(knockKnockDialogue);
    })
    .When.WalkTo(() =>
    {
        guy.MoveTo(340, 430, WellKnownStatus.FaceAwayFromCamera);
    }));
