DialogueTree knockKnock = AddDialogueTree(nameof(knockKnock), builder => builder
    .WithTopic("start", dialogue =>
    {
        guy.SayLine("Knock, knock!");
        richard.SayLine("Who's there?");
        dialogue.Topic("main");
    })
    .WithTopic("main", dialogue =>
    {
        dialogue.Option("Spell.", "spell");
        dialogue.Option("Boo.", "boo");
        dialogue.Option("Alec.", "alec");
    })
    .WithTopic("spell", dialogue =>
    {
        richard.SayLine("Spell who?");
        guy.SayLine("Okay...");
        guy.SayLine("'W'");
        guy.SayLine("'H'");
        guy.SayLine("'O'");
        shipComputer.SayLine("*drumroll*");
    })
    .WithTopic("spell", dialogue =>
    {
        richard.SayLine("Boo who?");
        guy.SayLine("No need to cry, it's only a joke!");
        carl.SayLine("He's got you there, Richard!");
    })
    .WithTopic("alec", dialogue =>
    {
        richard.SayLine("Alec who?");
        guy.SayLine("Alec-tricity.");
        guy.SayLine("Ain't that a shocker?");
        carl.SayLine("That's a high voltage joke!");
    }));
