Item accessDeniedSign = AddItem(nameof(accessDeniedSign), "access_denied_sign", item => item
    .Named("access denied sign")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .When.LookAt(() =>
    {
        if (guy.Has(accessDeniedSign))
        {
            SayLine("I'm glad the priority lane didn't stay closed for too long.");
        }
        else
        {
            guy.SayLine("''Priority lane is currently closed.''");
            guy.FaceCamera();
            guy.MoveTo(840, 420);
            guy.SayLine("That's pretty inconvenient.");
        }
    })
    .When.PickUp(() =>
    {
        guy.FaceAwayFromCamera();
        Delay(500);
        guy.AddToInventory(accessDeniedSign);
        guy.FaceCamera();
        guy.SayLine("Fixed it!");
    }));
    
Item boardingPass = AddItem(nameof(boardingPass), item => item
    .Named("boarding pass")
    .When.LookAt(() =>
    {
        SayLine("It's my boarding pass for Oceanic Airways flight 815!");
    }));
    
Item chairs1 = AddItem(nameof(chairs1), "chairs", item => item
    .Untouchable());
    
Item chairs2 = AddItem(nameof(chairs2), "chairs", item => item
    .Untouchable());

Item counter = AddItem(nameof(counter), item => item
    .Untouchable());

Item gatesArea = AddItem(nameof(gatesArea), "check_in_wall", item => item
    .Named("gates area")
    .WithActorInteraction(RelativePosition.None)
    .When.WalkTo(() => 
    {
        guy.MoveTo(700, 300);
    
        if (!guy.Has(boardingPass))
        {
            richard.SayLine("Sir!");
            richard.SayLine("You can't go in there\nwithout a boarding pass!");
            guy.MoveTo(550, 300);
            guy.FaceCamera();
        }
        else
        {
            ChangeRoom(security);
        }
    }));
    
    
Item loungeSign = AddItem("lounge_sign", item => item
    .Named("lounge sign")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .When.LookAt(() =>
    {
        SayLine("It's a sign for the swanky Azure Airways lounge!");
    }));

Item loungeDoorClosed = AddItem("lounge_door_closed", item => item
    .Named("lounge door")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .When.LookAt(() =>
    {
        SayLine("It's a sign for the swanky Azure Airways lounge!");
    })
    .When.Open(() =>
    {
        carl.SayLine("Not so fast there, buddy!");
        carl.SayLine("You can't go in without the proper authorization!");
        guy.FaceCamera();
        guy.SayLine("Drat!");
    }));
  
Item loungeDoorOpen = AddItem("lounge_door_open", item => item
    .Named("lounge door")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera, 0, -100)
    .WithDepthOffset(-100)
    .When.LookAt(() =>
    {
        SayLine("It's so beautiful!");
    })
    .When.Close(() =>
    {
        lobby.Place(loungeDoorClosed, 391, 231);
        lobby.Remove(loungeDoorOpen);
    }));

Item membershipCard = AddItem(nameof(membershipCard), item => item
    .Named("membership card")
    .CanBeUsedWithOtherObject()
    .When.LookAt(() =>
    {
        SayLine("It's my Jetsetter frequent flyer card!");
    })
    .When.Give(gameObject =>
    {
        if (gameObject == richard)
        {
            richard.SayLine("Wow, you're a Jetsetter member, huh?");
            richard.SayLine("You better hold on to that.");
            richard.SayLine("*cough* Show-off *cough*");
        }
    })
    .When.Use(gameObject =>
    {
        if (gameObject == priorityLaneEntrance)
        {
            CallApi(
                "https://localhost:7074/priority-lane",
                new Dictionary<string, string>
                {
                    { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "guy" },
                    { "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "passenger" },
                    { "frequent_flyer_status", "jetsetter" }
                },
                (content) =>
                {
                    priorityLaneEntrance.ChangeStatus("open");
                },
                (content) =>
                {
                    guy.SayLine("Hmmm, I don't think my card is accepted!");
                });
        }
    }));
    
Item monitors = AddItem("monitors", item => item
    .Named("monitors")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .When.LookAt(() =>
    {
        SayLine("Whoops, I'd better hurry up and check in!");
    }));

Item passport = AddItem(nameof(passport), item => item
    .Named("passport")
    .CanBeUsedWithOtherObject()
    .When.LookAt(() =>
    {
        SayLine("It's my passport!");
        SayLine("It's got a lot of stamps in it!");
    })
    .When.Give(gameObject =>
    {
        if (gameObject == richard)
        {
            if (guy.Has(boardingPass))
            {
                richard.SayLine("You're all set!");
                richard.SayLine("Enjoy your flight!");
            }
            else
            {
                richard.SayLine("Ok, let's see if this works with OpenFGA...");
                        
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
                        
                        SetFlag("speedrun");
                    },
                    (content) =>
                    {
                        richard.SayLine("Hmm, are you sure you've configured OpenFGA correctly?");
                    }); 
            }  
        }
    }));

Item priorityLaneEntrance = AddItem(nameof(priorityLaneEntrance), "priority_door", item => item
    .Named("priority lane entrance")
    .WithStatus("closed")
    .WithActorInteraction(status: WellKnownStatus.FaceAwayFromCamera)
    .When.LookAt(() => 
    {
        guy.SayLine("It's the entry to the security priority lane");
        guy.FaceCamera();
        guy.SayLine("I've got to get in there!'");
    })
    .When.Close(() =>
    {
        priorityLaneEntrance.ChangeStatus("closed");
    })
    .When.Open(() =>
    {
        CallApi(
            "https://localhost:7074/priority-lane",
            new Dictionary<string, string>
            {
                { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "guy" },
                { "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "passenger" },
                { "frequent_flyer_status", "noob" }
            },
            (content) =>
            {
                priorityLaneEntrance.ChangeStatus("open");
            },
            (content) =>
            {
                guy.SayLine("I don't think I'm authorized to just walk in!");
            });
    })
    .When.WalkTo(() =>
    {
        if (priorityLaneEntrance.Status == "open")
        {
            ChangeRoom(lobby);
        }
    }));
    
Item plantCheckIn = AddItem(nameof(plantCheckIn), "plant", item => item
    .Named("plant")
    .WithActorInteraction(RelativePosition.Above, WellKnownStatus.FaceCamera)
    .WithDepthOffset(200)
    .When.LookAt(() =>
    {
        SayLine("It's so beautiful!");
    }));

Item plantSecurity = AddItem(nameof(plantSecurity), "plant", item => item
    .Named("plant")
    .WithActorInteraction(RelativePosition.Above, WellKnownStatus.FaceCamera)
    .WithDepthOffset(200)
    .When.LookAt(() =>
    {
        SayLine("It's so beautiful!");
    }));

Item plant = AddItem("plant", item => item
    .Named("plant")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .When.LookAt(() =>
    {
        SayLine("It's so beautiful!");
    }));

Item plantSmall = AddItem("plant_small", item => item
    .Named("plant")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .When.LookAt(() =>
    {
        SayLine("It's an adorable little plant");
    }));

Item poster = AddItem(nameof(poster), item => item
    .Named("porto poster")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera, -20, 50)
    .When.LookAt(() =>
    {
        SayLine("Ah! Beautiful Porto!");
    }));

Item securityCrowd = AddItem(nameof(securityCrowd), "crowd", item => item
    .Named("crowd of frustrated passengers")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .WithDepthOffset(-100)
    .When.LookAt(() =>
    {
        SayLine("Yep, they're pretty frustrated");
    }));

Item securityRope = AddItem(nameof(securityRope), "rope", item => item
    .Named("security rope")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .When.LookAt(() =>
    {
        SayLine("That looks like a pretty sturdy security rope");
    }));

Item securitySign = AddItem(nameof(securitySign), "sign", item => item
    .Named("security sign")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .When.LookAt(() =>
    {
        guy.SayLine("''6 hours waiting time from this point''");
    }));
    
Item securityWall = AddItem(nameof(securityWall), "security_wall", item => item
    .Untouchable());
