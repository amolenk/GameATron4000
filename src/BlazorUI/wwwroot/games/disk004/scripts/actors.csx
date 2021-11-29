Actor al = AddActor("al", builder => builder
    .Named("Al")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .WithTextColor("Yellow")
    .When.LookAt(() =>
    {
        SayLine("I guess it's an alien. Hard to see at this resolution.");
    }));

Actor carl = AddActor("carl", builder => builder
    .Named("Richard & Carl")
    .WithActorInteraction(RelativePosition.None)
    .WithTextColor("#48CC1A")
    .When.WalkTo(() => richard.ActionHandlers.HandleWalkTo()));

Actor guy = AddActor("guy", builder => builder
    .Untouchable());

Actor richard = AddActor("richard", builder => builder
    .Named("Richard & Carl")
    .WithActorInteraction(RelativePosition.None)
    .WithTextColor("#F4FF6B")
    .When.WalkTo(() =>
    {
        guy.MoveTo(340, 430, WellKnownStatus.FaceAwayFromCamera);
    }));
