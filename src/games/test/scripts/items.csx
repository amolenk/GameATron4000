Item counterTop; // TODO Powercord seems to work fine out of order

// TODO Use nameof() everywhere

Item alarm = AddItem(nameof(alarm), builder => builder
    .Untouchable());

Item beamButton = AddItem(nameof(beamButton), builder => builder
    .Named("big button")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .WithStatus("off") // TODO
    .When.LookAt(() =>
    {
        SayLine("Now that's a big button!");
    })
    .When.Push(() => beamButton.ActionHandlers.HandleUse(null))
    .When.Use(_ =>
    {
        if (beamButton.Status == "on")
        {
            shipComputer.SayLine("* DEACTIVATING BEAM SECURITY PROTOCOL *");
            beamButton.ChangeStatus("off");
        }
        else
        {
            shipComputer.SayLine("* ACTIVATING BEAM SECURITY PROTOCOL *");
            beamButton.ChangeStatus("on");
            terminalDoor.ChangeStatus("closed");
        }
    }));


Item beamGlow = AddItem(nameof(beamGlow), builder => builder
    .DependsOn(beamButton, "on")
    .WithDepthOffset(1)
    .Untouchable());

Item beamPark = AddItem(nameof(beamPark), builder => builder
    .Untouchable());

Item beamTerminal = AddItem(nameof(beamTerminal), builder => builder
    .DependsOn(beamButton, "on")
    .Untouchable());

Item bottle = AddItem(nameof(bottle), builder => builder
    .CanBeUsedWithOtherObject()
    .DependsOn(fridge, "open")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .WithDepthOffset(91)
    .When.LookAt(() =>
    {
        SayLine("It's a bottle with some liquid in it.");
        SayLine("'Canes Venatici Cosmic Scotch. Drink at your own risk!'");
        guy.FaceCamera();
        SayLine("Strong stuff!");
    })
    .When.Give(to =>
    {
        if (to == richard || to == carl)
        {
            guy.MoveTo(245, 420, WellKnownStatus.FaceAwayFromCamera);
            carl.SayLine("No thanks, we're recording right now.");
            richard.SayLine("Maybe after the show.");
        }
    })
    .When.PickUp(() =>
    {
        guy.AddToInventory(bottle);
    })
    .When.Use(gameObject =>
    {
        if (gameObject == cooker)
        {
            if (cooker.Status != "on")
            {
                SayLine("I should turn it on first.");
            }
            else
            {
                SayLine("Ok, I guess it's practically the same thing as water.");
                SayLine("Wow, it's boiling already!");
                SetFlag("cookerBoiling");
                guy.RemoveFromInventory(bottle);
            }
        }
    }));

Item bridgeDoor = AddItem(nameof(bridgeDoor), builder => builder
    .Named("door")
    .WithDepthOffset(-10)
    .When.LookAt(() =>
    {
        SayLine("It's the doorway to the terminal.");
    })
    .When.Close(() =>
    {
        SayLine("It doesn't seem to close from this side.");
    })
    .When.Open(() =>
    {
        SayLine("It's already open.");
    })
    .When.WalkTo(() =>
    {
        ChangeRoom(terminal);
    }));

Item clawHammer = AddItem(nameof(clawHammer), builder => builder
    .Named("claw hammer")
    .CanBeUsedWithOtherObject()
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .When.LookAt(() =>
    {
        SayLine("From wikipedia:");
        SayLine("1. a hammer with one side of the head split and curved, used for extracting nails.");
        SayLine("2. a style of banjo playing in which the thumb and fingers strum or pluck the strings in a downward motion.");
        guy.FaceCamera();
        SayLine("I'd say option 1 is applicable here.");
    })
    .When.Give(_ =>
    {
        SayLine("I think I'll keep it for now.");
    })
    .When.PickUp(() =>
    {
        carl.SayLine("Hey, we need that for the show!");
    })
    .When.Use(gameObject =>
    {
        if (gameObject == podcastBooth)
        {
            StartDialogue("knockKnock");
        }
        else if (gameObject == crateLeft || gameObject == crateTop)
        {
            SayLine("The nails in this crate are stuck.");
        }
        else if (gameObject == crateRight)
        {
            crateRight.ChangeStatus("open");
            SayLine("There's something inside!");
        }
    }));

Item cheeseGrater = AddItem(nameof(cheeseGrater), builder => builder
    .Named("cheese grater")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .When.LookAt(() =>
    {
        SayLine("It's an alien cheese grater.");
    })
    .When.Give(to =>
    {
        if (to == richard || to == carl)
        {
            guy.MoveTo(245, 420, WellKnownStatus.FaceAwayFromCamera);
            SayLine("Can you use this for the show?");
            carl.SayLine("What is it?");
            SayLine("It's a highly advanced alien artifact with holes edged by slightly raised cutting edges, used for grating a certain type of yellow nutritious substance.");
            richard.SayLine("That's just what we need for the show!");
            carl.SayLine("We'll trade you for the claw hammer.");
            carl.SayLine("Er, *alien* claw hammer.");
            SayLine("Sure!");
            guy.AddToInventory(clawHammer);
            bridge.Place(cheeseGrater, 245, 350);
//            SetFlag("tradedHammer");
            guy.FaceCamera();
        }
        else
        {
            SayLine("I think I'll keep it for now.");
        }
    })
    .When.PickUp(() =>
    {
//        if (IsFlagSet("tradedHammer"))
        carl.SayLine("Hey, we need that for the show!");
    }));

Item cooker = AddItem(nameof(cooker), builder => builder
    .Named("cooker")
    .CanBeUsedWithOtherObject()
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .WithStatus("off")
    .When.LookAt(() =>
    {
        if (cooker.Status == "smoke")
        {
            SayLine("Never trust hot dogs with green smoke coming off them.");
        }
        else if (cooker.Status == "burned")
        {
            SayLine("They look a bit overcooked.");
        }
        else
        {
            SayLine("It's an electrical cooker.");
        }
    })
    .When.Give(_ =>
    {
        SayLine("I think I'll keep it for now.");
    })
    .When.Use(gameObject =>
    {
        if (gameObject == counterTop)
        {
            bridge.Place(cooker, 1485, 327);
        }
    }));

Item cookerInCrate = AddItem(nameof(cookerInCrate), builder => builder
    .Named("thing in crate")
    .DependsOn(crateRight, "open")
    .WithActorInteraction(RelativePosition.Above, WellKnownStatus.FaceCamera)
    .When.LookAt(() =>
    {
        SayLine("It looks like some kind of cooking device.");
    })
    .When.PickUp(() =>
    {
        terminal.Remove(cookerInCrate);
        guy.AddToInventory(cooker);
    }));

counterTop = AddItem(nameof(counterTop), builder => builder
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .WithDepthOffset(-1000)
    .When.LookAt(() =>
    {
        SayLine("It's a countertop, not much to add here.");
    }));

Item crateLeft = AddItem(nameof(crateLeft), builder => builder
    .Named("left crate")
    .WithActorInteraction(RelativePosition.Above, WellKnownStatus.FaceCamera)
    .When.LookAt(() =>
    {
        SayLine("It's a decidedly low-tech wooden crate.");
    }));

Item crateRight = AddItem(nameof(crateRight), builder => builder
    .Named("right crate")
    .WithActorInteraction(RelativePosition.Above, WellKnownStatus.FaceCamera)
    .WithStatus("closed")
    .When.LookAt(() =>
    {
        SayLine("It's a decidedly low-tech wooden crate.");
        if (crateRight.Status == "closed")
        {
            SayLine("The label says: 'don't open until mission is complete!'");
        }
    })
    .When.Open(() =>
    {
        SayLine("It's nailed shut.");
    }));

Item crateRightFront = AddItem(nameof(crateRightFront), builder => builder
    .DependsOn(crateRight, "open")
    .WithDepthOffset(10)
    .Untouchable());

Item crateTop = AddItem(nameof(crateTop), builder => builder
    .Named("top crate")
    .WithActorInteraction(RelativePosition.Above, WellKnownStatus.FaceCamera)
    .WithDepthOffset(50)
    .When.LookAt(() =>
    {
        SayLine("It's a decidedly low-tech wooden crate.");
    }));

Item fridge = AddItem(nameof(fridge), builder => builder
    .WithActorInteraction(status: WellKnownStatus.FaceAwayFromCamera)
    .WithDepthOffset(10)
    .WithStatus("closed")
    .When.LookAt(() =>
    {
        if (fridge.Status == "closed")
        {
            SayLine("It's a big fridge!");
            SayLine("The badge on the side reads 'Brrrr-a-tron 9000™'");
            guy.FaceCamera();
            SayLine("Never heard of it!");
            al.SayLine("It's top of the line!");
        }
        else if (guy.Has(groceries))
        {
            SayLine("It's empty!");
            guy.FaceCamera();
            SayLine("I wonder where these guys do their shopping!");
        }
        else
        {
            SayLine("All my groceries are now in the fridge!");
        }
    })
    .When.Close(() =>
    {
        fridge.ChangeStatus("close");
    })
    .When.Open(() =>
    {
        fridge.ChangeStatus("open");
    }));

Item groceries = AddItem("groceries", builder => builder
    .CanBeUsedWithOtherObject()
    .DependsOn(fridge, "open")
    .WithDepthOffset(30)
    .Untouchable()
    .When.LookAt(() =>
    {
        if (guy.Has(groceries))
        {
            SayLine("It's my shopping bag with groceries.");
            SayLine("There's even a bottle of Info Support Awesome Sauce™ in there!");
        }
    })
    .When.Give(_ =>
    {
        SayLine("I think I'll keep it for now.");
    })
    .When.Use(gameObject =>
    {
        if (gameObject == groceryList)
        {
            SayLine("Yep, I've got everything on the list!");
        }
        else if (gameObject == fridge)
        {
            if (fridge.Status == "closed")
            {
                SayLine("What do you expect me to do? Throw them at the fridge door?");
            }
            else
            {
                bridge.Place(groceries, fridge.Position.X, fridge.Position.Y - 16);
            }
        }
    }));

Item groceryList = AddItem("groceryList", builder => builder
    .Named("grocery list")
    .CanBeUsedWithOtherObject()
    .When.LookAt(() =>
    {
        if (guy.Has(groceryList))
        {
            SayLine("It's my grocery list!");
            SayLine("I've bought everything that's on it!");
        }
        else
        {
            SayLine("It's my old grocery list!");
            SayLine("I guess these guys have a new to-do list now!");
            
            
        }
    })
    .When.Give(_ =>
    {
        SayLine("I think I'll keep it for now.");
    })
    .When.Use(gameObject =>
    {
        if (gameObject == groceries)
        {
            SayLine("Yep, I've got everything on the list!");
        }
        else if (gameObject == todoList)
        {
            if (!IsFlagSet("alarm"))
            {
                var ianPosition = new Point(ian.Position.X, ian.Position.Y);
                var todoListPosition = new Point(todoList.Position.X, todoList.Position.Y);

                bridge.Remove(todoList);
                ian.MoveTo(750, 325, WellKnownStatus.FaceCamera);
                ian.SayLine("Hey, what are you doing there?");
                Delay(500);

                bridge.Place(todoList, todoListPosition);
                SayLine("Er, nothing!");
                // TODO Position? Face direction???
                ian.MoveTo(ianPosition);
                
                guy.FaceCamera();
                SayLine("I wish he was more distracted.");
            }
            else
            {
                guy.AddToInventory(todoList);
                Delay(500);

                bridge.Place(groceryList, 660, 250);
                guy.FaceCamera();
                SayLine("Nothing better than a surprise mission change!");
                shipComputer.SayLine("* KITCHEN ZONE CLEAR: ALARM DEACTIVATED *");

                ian.MoveTo(810, 300, WellKnownStatus.FaceAwayFromCamera);
                al.MoveTo(990, 375, WellKnownStatus.FaceCamera);

                bridge.Remove(alarm);
                bridge.Remove(powerCord);

                cooker.ChangeStatus("burned");

                ClearFlag("alarm");
            }
        }
    }));

Item newspaper = AddItem("newspaper", builder => builder
    .When.Give(_ =>
    {
        SayLine("I think I'll keep it for now.");
    })
    .When.LookAt(() =>
    {
        if (guy.Has(newspaper))
        {
            SayLine("It's yesterday's paper.");
        }
        else
        {
            SayLine("It looks like an old newspaper.");
        }
    })
    .When.PickUp(() =>
    {
        guy.AddToInventory(newspaper);
        CutScene_BeamUp();
    }));

Item newspaperHeadline = AddItem(nameof(newspaperHeadline), builder => builder
    .Untouchable()
    .FixedToCamera());

Item onAir = AddItem(nameof(onAir), builder => builder
    .WithStatus("on")
    .Untouchable());

Item outletBooth = AddItem(nameof(outletBooth), builder => builder
    .Untouchable());

Item outletKitchen = AddItem(nameof(outletKitchen), builder => builder
    .Untouchable());

Item parkBench = AddItem(nameof(parkBench), builder => builder
    .Untouchable());

Item podcastBooth = AddItem("podcastBooth", builder => builder
    .Named("podcast booth")
    .WithActorInteraction(status: WellKnownStatus.FaceAwayFromCamera)
    .WithDepthOffset(-400)
    .When.LookAt(() =>
    {
        SayLine("It's a podcast booth!");
    }));

Item powerCord = AddItem("powerCord", builder => builder
    .Named("power cord")
    .CanBeUsedWithOtherObject()
    .WithActorInteraction(RelativePosition.Center, WellKnownStatus.FaceAwayFromCamera)
    .WithDepthOffset(-35)
    .WithStatus("booth")
    .When.LookAt(() =>
    {
        SayLine("Looks like the podcast booth gets it electricity from the ship.");
        guy.FaceCamera();
        SayLine("I wonder how the aliens feel about that.");
    })
    .When.Give(_ =>
    {
        SayLine("I think I'll keep it for now.");
    })
    .When.PickUp(() =>
    {
        SayLine("I'm sure they won't mind me borrowing this power cord.");
        richard.SayLine("...flux capacity of this ship is enormous...");
        carl.SayLine("Hmm, interesting...");

        guy.MoveTo(435, 365, WellKnownStatus.FaceAwayFromCamera);
        Delay(1000);

        guy.AddToInventory(powerCord);
        SetFlag("boothDisconnected");
        Delay(500);

        for (var i = 0; i < 4; i++)
        {
            onAir.ChangeStatus("off");
            Delay(200);
            onAir.ChangeStatus("on");
            Delay(200);
        }
        onAir.ChangeStatus("off");
        Delay(1000);

        richard.SayLine("Er...Carl...");
        carl.SayLine("Yeah?");
        richard.SayLine("Why is the microphone off?");
        carl.SayLine("Hm, seems like all power is gone!");
        richard.SayLine("Maybe the solar radiation is interfering with the ships primary capacitators.");

        guy.MoveTo(430, 405, WellKnownStatus.FaceCamera);
    })
    .When.Pull(() => powerCord.ActionHandlers.HandlePickUp())
    .When.Use(with =>
    {
        if (with == cooker)
        {
            if (guy.Has(cooker))
            {
                SayLine("I really should put it on a stable surface first.");
            }
            else
            {
                powerCord.ChangeStatus("kitchen");
                bridge.Place(powerCord, 1540, 414);
                cooker.ChangeStatus("on");
            }
        }
    }));

Item saucages = AddItem("saucages", builder => builder
    .Named("hot dogs")
    .CanBeUsedWithOtherObject()
    .DependsOn(fridge, "open")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .WithDepthOffset(91)
    .When.LookAt(() =>
    {
        SayLine("'100% premium mystery meat hot dogs!'");
        SayLine("'Heat up in boiled water for 10 minutes!'");
    })
    .When.Give(to =>
    {
        if (to == al)
        {
            // TODO Use lemonhead dialogue
            al.SayLine("No thanks, I've already got a package in the fridge.");
            al.SayLine("I'm saving them to celebrate when the mission is done!");
        }
        else
        {
            SayLine("I think I'll keep them for now.");
        }
    })
    .When.PickUp(() =>
    {
        guy.AddToInventory(saucages);
    })
    .When.Use(with =>
    {
        if (with == cooker)
        {
            if (!IsFlagSet("cookerBoiling"))
            {
                SayLine("According to the package, I should boil some water first.");
            }
            else
            {
                SayLine("Here goes nothing!");
                guy.RemoveFromInventory(saucages);
                cooker.ChangeStatus("full");
                Delay(1500);

                SayLine("I wonder if they're supposed to turn green...");
                cooker.ChangeStatus("smoke");
                SayLine("Yikes!");

                guy.MoveTo(1310, 425, WellKnownStatus.FaceCamera);
                SayLine("This can't be good!");

                bridge.Place(alarm, 1449, 330);
                shipComputer.SayLine("* WARNING: SMOKE DETECTED IN KITCHEN ZONE *");

                al.MoveTo(1490, 425, WellKnownStatus.FaceAwayFromCamera);
                al.SayLine("WHAT ARE YOU DOING TO MY HOT DOGS?");

                ian.MoveTo(1395, 425, WellKnownStatus.FaceAwayFromCamera);
                ian.SayLine("What's going on here?");
                al.SayLine("MY HOT DOGS!");
                ian.SayLine("I told you to wait until the mission was done!");

                ClearFlag("alarm");
            }
        }
    }));

Item terminalDoor = AddItem(nameof(terminalDoor), builder => builder
    .Named("door")
    .WithStatus("closed")
    .WithActorInteraction(status: WellKnownStatus.FaceAwayFromCamera)
    .When.LookAt(() => SayLine("It looks like a fancy high-tech door."))
    .When.Open(() =>
    {
        if (terminalDoor.Status == "open")
        {
            SayLine("It's already open!");
        }
        else if (beamButton.Status == "on")
        {
            SayLine("It won't budge!");
        }
        else
        {
            terminalDoor.ChangeStatus("open");
        }
    })
    .When.Close(() =>
    {
        if (terminalDoor.Status == "closed")
        {
            SayLine("It's already closed!");
        }
        else
        {
            terminalDoor.ChangeStatus("closed");
        }
    })
    .When.WalkTo(() =>
    {
        ChangeRoom(bridge);
    }));

Item todoList = AddItem(nameof(todoList), builder => builder
    .Named("to-do list")
    .WithActorInteraction(RelativePosition.InFront, WellKnownStatus.FaceAwayFromCamera)
    .When.LookAt(() =>
    {
        if (guy.Has(todoList))
        {
            SayLine("It's the alien to-do list I've replaced with my grocery list!");
        }
        else
        {
            SayLine("It's a list with names.");
            SayLine("The title says: 'Body-snatch list'.");
            guy.FaceCamera();
            SayLine("YIKES!!");
        }
    })
    .When.PickUp(() =>
    {
        // TODO Not needed!!!
//        if (!guy.Has(todoList))

        var ianPosition = new Point(ian.Position.X, ian.Position.Y);
        var todoListPosition = new Point(todoList.Position.X, todoList.Position.Y);

        bridge.Remove(todoList);
        guy.FaceCamera();
        SayLine("Got it!");

        if (IsFlagSet("alarm"))
        {
            ian.MoveTo(860, 430, WellKnownStatus.FaceAwayFromCamera);
        }
        else
        {
            ian.MoveTo(750, 325, WellKnownStatus.FaceCamera);
        }

        ian.SayLine("Hey, put that back!");
        ian.SayLine("We need that to finish the mission!");

        ian.MoveTo(ianPosition.X, ianPosition.Y);
        ian.FaceAwayFromCamera();
        Delay(500);

        bridge.Place(todoList, todoListPosition.X, todoListPosition.Y);

        guy.FaceCamera();
        SayLine("I guess I have to think of something sneakier!");

    }));
