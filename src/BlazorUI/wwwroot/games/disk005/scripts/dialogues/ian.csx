DialogueTree ianDialogue = AddDialogueTree("ian", builder => builder
    .WithTopic(DialogueTree.StartTopicName, dialogue =>
    {
        ian.FaceCamera();
        ian.SayLine("What do you want, human?");
        
        return dialogue.Continue("main");
    })
    .WithTopic("main", dialogue => new[]
    {
        new DialogueOption(
            "My name is Guy Scotthrie and I want to be a space pirate!",
            "pirate",
            () => !IsFlagSet("talked-about-pirate")),
        new DialogueOption(
            "What's going on here?",
            "whats-going-on",
            () => !IsFlagSet("talked-about-goingons")),
        new DialogueOption(
            "Why abduct all the tech nerds?",
            "nerds",
            () => IsFlagSet("talked-about-goingons") && !IsFlagSet("talked-about-nerds")),
        new DialogueOption(
            "Must be hard making sure you've got everybody!",
            "hard",
            () => IsFlagSet("talked-about-goingons") && !IsFlagSet("talked-about-hard")),
        new DialogueOption(
            "Are you done with the mission yet?",
            "check",
            () => IsFlagSet("talked-about-goingons") && IsFlagSet("talked-about-hard")),
        new DialogueOption(
            "I'm selling these fine red polo shirts.",
            "shirts",
            () => !IsFlagSet("talked-about-shirts")),
        new DialogueOption("Bye!", "bye")
    })
    .WithTopic("pirate", dialogue =>
    {
        ian.SayLine("Excellent, we can always use a new recruit!");

        SetFlag("talked-about-pirate");
        return dialogue.Continue("main");
    })
    .WithTopic("whats-going-on", dialogue =>
    {
        ian.SayLine("Not much.");
        ian.SayLine("We're on this top-secret mission to abduct all the tech nerds from this planet.");

        SetFlag("talked-about-goingons");
        return dialogue.Continue("main");
    })
    .WithTopic("nerds", dialogue =>
    {
        ian.SayLine("We don't need any super intelligence popping up here!");
        ian.SayLine("Those Azure data centers are doing something!");
        guy.SayLine("But they're just hosting Blazor apps for 8-bit adventure games!");
        ian.SayLine("Nice try!");
        ian.SayLine("Nobody plays adventure games anymore!");

        SetFlag("talked-about-nerds");
        return dialogue.Continue("main");
    })
    .WithTopic("hard", dialogue =>
    {
        ian.SayLine("Actually, it's super easy.");
        ian.SayLine("Barely an inconvenience!");
        guy.SayLine("Oh, really?");
        ian.SayLine("Yes, we've got these top-secret mission orders that explain everything.");

        SetFlag("talked-about-hard");
        return dialogue.Continue("main");
    })
    .WithTopic("check", dialogue =>
    {
        ian.SayLine("Let's check the top-secret mission orders.");

        if (IsMissionAccomplished())
        {
            return dialogue.End();
        }
        else
        {
            return dialogue.Continue("main");
        }
    })
    .WithTopic("shirts", dialogue =>
    {
        ian.SayLine("Not interested, we all wear these same uniforms.");
        ian.SayLine("Less sprites to draw.");

        SetFlag("talked-about-shirts");
        return dialogue.Continue("main");
    })
    .WithTopic("bye", dialogue =>
    {
        ian.SayLine("Yes, go away.");
        ian.FaceAwayFromCamera();
        return dialogue.End();
    }));

bool IsMissionAccomplished()
{
    var ianPosition = ian.Position;
    Follow(ian);

    if (guy.Has(groceryList))
    {
        ian.MoveTo(todoList);
        guy.FaceCamera();
        ian.SayLine("Hmm, we still need to find this Hans Scottleman guy...");
    }
    else
    {
        ian.MoveTo(groceryList);
        if (!IsFlagSet("mission-changed"))
        {
            ian.SayLine("Hmm, looks like the mission orders have changed.");
            ian.FaceCamera();
            ian.SayLine("Al, did you know we need to collect earthly food products?");
            al.SayLine("Too bad those hot dogs are burned!");
            al.SayLine("Maybe there's something left in the fridge.");
            SetFlag("mission-changed");
        }
        else
        {
            ian.SayLine("Still looking for those earthly food products.");
        }

        ian.MoveTo(fridge);

        if (fridge.Status == "open")
        {
            ian.SayLine("Damnit Al, you left the fridge open again!");
            al.SayLine("It wasn't me!");
        }
        else
        {
            fridge.ChangeStatus("open");
        }

        if (guy.Has(groceries))
        {
            ian.SayLine("Hmm, we're not quite there yet!");
            fridge.ChangeStatus("closed");
        }
        else
        {
            ian.SayLine("Excellent, looks like we're all set!");
            fridge.ChangeStatus("closed");
            ian.FaceCamera();
            ian.SayLine("Er, what are those humans still doing in my ship?");
            Delay(1000);
            ChangeRoom(beach);
            return true;
        }
    }

    ian.MoveTo(ianPosition, WellKnownStatus.FaceCamera);
    guy.FaceAwayFromCamera();
    Follow(guy);

    return false;
}
