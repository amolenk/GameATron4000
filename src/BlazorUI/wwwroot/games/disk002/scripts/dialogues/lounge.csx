DialogueTree loungeDialogue = AddDialogueTree("lounge", builder => builder
    .WithTopic(DialogueTree.StartTopicName, dialogue => new []
    {
        new DialogueOption("Which lounges do I have access to?", "list"),
        new DialogueOption("Can you let me in the Azure Airways lounge?", "enter"),
        new DialogueOption("Nevermind...", "end")
    })
    .WithTopic("list", dialogue =>
    {
        if (!IsFlagSet("asked_about_lounges"))
        {
            SetFlag("asked_about_lounges");
            carl.SayLine("I'd guess none, but I'll have a look...");
        }
        else
        {
            carl.SayLine("Short memory, huh?");
        }  

        CallApi(
            "https://localhost:7074/lounges",
            new Dictionary<string, string>
            {
                { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "guy" }
            },
            (content) =>
            {
                Delay(1000);
                carl.SayLine(content);
            },
            (content) =>
            {
                carl.SayLine("Sorry, system isn't working at the moment.");
            });
            
        return dialogue.Continue(DialogueTree.StartTopicName);
    })
    .WithTopic("enter", dialogue =>
    {
        CallApi(
            "https://localhost:7074/lounge",
            new Dictionary<string, string>
            {
                { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "guy" }
            },
            (content) =>
            {
                Delay(1000);
                carl.SayLine("Yes, sir!.\nEnjoy the lounge!");
                guy.MoveTo(391, 250);   
                guy.FaceAwayFromCamera();        
                Delay(1000);
                lobby.Remove(loungeDoorClosed);
                lobby.Place(loungeDoorOpen, 394, 320);
                Delay(500);
                guy.SayLine("It's so beautiful!");
                Delay(1000);
                narrator.SayLine("** LAST CALL FOR FLIGHT 815 TO PORTO **");
                narrator.SayLine("** PROCEED TO GATE IMMEDIATELY **");
                lobby.Place(loungeDoorClosed, 391, 231);
                lobby.Remove(loungeDoorOpen);
                Delay(1000);
                guy.FaceCamera();
                guy.SayLine("Oh, well...");
                guy.MoveTo(950, 280);   
                Delay(1000);
                carl.SayLine("Weird guy...");
            },
            (content) =>
            {
                carl.SayLine("No");
            });
            
        return dialogue.End();
    })
    .WithTopic("end", dialogue => dialogue.End()));
