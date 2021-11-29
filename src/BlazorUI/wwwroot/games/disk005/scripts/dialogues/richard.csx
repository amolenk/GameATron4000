DialogueTree richardDialogue = AddDialogueTree("richard", builder => builder
    .WithTopic(DialogueTree.StartTopicName, dialogue =>
    {
        if (!IsFlagSet("talked-to-richard"))
        {
            guy.SayLine("Hey guys!");
            richard.SayLine("Hey buddy!");
            guy.SayLine("So...");
            guy.SayLine("What are you doing here?");
            richard.SayLine("Well, we we're recording our podcast at CCCCCC™...");
            carl.SayLine("...when suddenly this bright red light appeared!");
            richard.SayLine("And now we're here...");
            carl.SayLine("...recording the ultimate UFO Geek Out episode!");
            richard.SayLine("With all this alien stuff lying around, we've got content for weeks!");

            SetFlag("talked-to-richard");
        }

        return dialogue.Continue("main");
    })
    .WithTopic("main", dialogue => new[]
    {
        new DialogueOption(
            "How is the show going?",
            "show"),
        new DialogueOption(
            "Can I see that hammer?",
            "see-hammer",
            () => IsFlagSet("talked-about-show") &&
                !IsFlagSet("asked-for-hammer") &&
                !IsFlagSet("traded-hammer")),
        new DialogueOption(
            "Aren't there more interesting things on this ship to talk about?",
            "interest",
            () => IsFlagSet("talked-about-show") &&
                !IsFlagSet("traded-hammer")),
        new DialogueOption(
            "CCCCCC™?",
            "cccccc",
            () => !IsFlagSet("talked-about-cccccc")),
        new DialogueOption("I'm off trying to escape!", "bye")
    })
    .WithTopic("show", dialogue =>
    {
        richard.SayLine("It's going great!");
        if (IsFlagSet("booth-power-cut"))
        {
            richard.SayLine("...well, until we lost power!");
        }
        carl.SayLine("We're examining this alien artifact here!");
        if (!IsFlagSet("traded-hammer"))
        {
            guy.SayLine("It kinda looks like a regular claw hammer.");
            richard.SayLine("It most definitely is not!");
            carl.SayLine("It's an *alien* claw hammer!");
        }

        return dialogue.Continue("main");
    })
    .WithTopic("see-hammer", dialogue =>
    {
        carl.SayLine("Sorry, we need it for the show.");
        SetFlag("asked-for-hammer");
        return dialogue.Continue("main");
    })
    .WithTopic("interest", dialogue =>
    {
        carl.SayLine("There are.");
        richard.SayLine("But El Capitan over there won't let us touch any of their stuff!");
        return dialogue.Continue("main");
    })
    .WithTopic("cccccc", dialogue =>
    {
        carl.SayLine("Conference Covering Crafting Critical Code, and, er, Crystallography.");
        SetFlag("talked-about-cccccc");
        return dialogue.Continue("main");
    })
    .WithTopic("bye", dialogue =>
    {
        richard.SayLine("Good luck with that!");
        return dialogue.End();
    }));
