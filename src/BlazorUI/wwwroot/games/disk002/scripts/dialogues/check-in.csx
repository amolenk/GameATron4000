DialogueTree checkInDialogue = AddDialogueTree("check-in", builder => builder
    .WithTopic(DialogueTree.StartTopicName, dialogue =>
    {
        guy.SayLine("Can I check-in?");
        
        if (guy.Has(boardingPass))
        {
            richard.SayLine("I just checked you in, sir!");
            richard.SayLine("Please proceed to the gate.");
            return dialogue.End();
        }
        
        richard.SayLine("Well, are you a ticket holder?");

        return new[]
        {
            new DialogueOption("Yes! (include role claim)", "ticket-holder"),
            new DialogueOption("Eh? Not sure, can't you check the system? (no role claim)", "no-ticket")
        };
    })
    .WithTopic("ticket-holder", dialogue => 
    {
        richard.SayLine("Ok, let me check you in.");
        
        CallApi(
            "https://localhost:7074/check-in/oc815",
            new Dictionary<string, string>
            {
                { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "guy" },
                { "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "ticket_holder" }
            },
            (content) =>
            {
                Delay(1000);
                richard.SayLine(content);
                guy.AddToInventory(boardingPass);
                guy.SayLine("Thanks!");
            },
            (content) =>
            {
                richard.SayLine("Hmm, I think the system is down.");
            });
        
        return dialogue.End();            
    })
    .WithTopic("no-ticket", dialogue => 
    {
        richard.SayLine("Hmmm, let me see what I can do...");
        
        CallApi(
            "https://localhost:7074/check-in/oc815",
            new Dictionary<string, string>
            {
                { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "guy" }
            },
            (content) =>
            {
                Delay(1000);
                richard.SayLine(content);
                guy.AddToInventory(boardingPass);
                guy.SayLine("Thanks!");
            },
            (content) =>
            {
                richard.SayLine("Sorry buddy, I can't check you in.");
            });
    
        return dialogue.End();            
    }));
