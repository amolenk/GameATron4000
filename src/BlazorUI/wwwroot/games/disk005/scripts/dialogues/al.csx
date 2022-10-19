DialogueTree alDialogue = AddDialogueTree("al", builder => builder
    .WithTopic(DialogueTree.StartTopicName, dialogue =>
    {
        al.SayLine("Hi there!");
        
        return dialogue.Continue("main");
    })
    .WithTopic("main", dialogue => new[]
    {
        new DialogueOption(
            "My name is Guy Scotthrie and I want to be a space pirate!",
            "pirate",
            () => !IsFlagSet("talked-about-pirate")),
        new DialogueOption(
            "Hey, beam me down back to the park, will you?",
            "beamdown"),
        new DialogueOption(
            "Is this an actual spaceship?",
            "spaceship",
            () => !IsFlagSet("talked-about-spaceship")),
        new DialogueOption(
            "I'm selling these fine red polo shirts.",
            "shirts",
            () => !IsFlagSet("talked-about-shirts")),
        new DialogueOption(
            "Sorry about your hot dogs!",
            "hotdogs",
            () => !IsFlagSet("talked-about-hotdogs") && cooker.Status == "burned"),
        new DialogueOption("Bye!", "bye")
    })
    .WithTopic("pirate", dialogue =>
    {
        al.SayLine("Excellent, we can always use a new recruit!");

        SetFlag("talked-about-pirate");
        return dialogue.Continue("main");
    })
    .WithTopic("beamdown", dialogue =>
    {
        var ianStatus = ian.Status;
        al.SayLine("Sure!");
        guy.SayLine("Great!");
        ian.FaceCamera();
        ian.SayLine("NO WAY!");
        ian.SayLine("We need to finish the mission!");
        ian.ChangeStatus(ianStatus);

        return dialogue.Continue("main");
    })
    .WithTopic("spaceship", dialogue =>
    {
        var ianStatus = ian.Status;
        al.SayLine("Sure is!");
        al.SayLine("It's an 8-bit stellar class beam-er-upper™ convertible!");
        ian.FaceCamera();
        ian.SayLine("Stop divulging top secret information!");
        ian.ChangeStatus(ianStatus);

        SetFlag("talked-about-spaceship");
        return dialogue.Continue("main");
    })
    .WithTopic("shirts", dialogue =>
    {
        al.SayLine("Not interested, we all wear these same uniforms.");
        al.SayLine("Less sprites to draw.");

        SetFlag("talked-about-shirts");
        return dialogue.Continue("main");
    })
    .WithTopic("hotdogs", dialogue =>
    {
        al.SayLine("I was really looking forward to those!");

        SetFlag("talked-about-hotdogs");
        return dialogue.Continue("main");
    })
    .WithTopic("bye", dialogue => dialogue.End()));
