DialogueTree knockKnockDialogue = AddDialogueTree(nameof(knockKnockDialogue), builder => builder
    .WithTopic(DialogueTree.StartTopicName, dialogue =>
    {
        guy.SayLine("Knock, knock!");
        richard.SayLine("Who's there?");
        
        return dialogue.Continue("main");
    })
    .WithTopic("main", dialogue => new[]
    {
        new DialogueOption("Spell.", "spell"),
        new DialogueOption("Boo.", "boo"),
        new DialogueOption("Alec.", "alec")
    })
    .WithTopic("spell", dialogue =>
    {
        richard.SayLine("Spell who?");
        guy.SayLine("Okay...");
        guy.SayLine("'W'");
        guy.SayLine("'H'");
        guy.SayLine("'O'");
        shipComputer.SayLine("*drumroll*");

        return dialogue.End();
    })
    .WithTopic("boo", dialogue =>
    {
        richard.SayLine("Boo who?");
        guy.SayLine("No need to cry, it's only a joke!");
        carl.SayLine("He's got you there, Richard!");

        return dialogue.End();
    })
    .WithTopic("alec", dialogue =>
    {
        richard.SayLine("Alec who?");
        guy.SayLine("Alec-tricity.");
        guy.SayLine("Ain't that a shocker?");
        carl.SayLine("That's a high voltage joke!");

        return dialogue.End();
    }));
