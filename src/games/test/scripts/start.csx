OnGameStart(() =>
{
    AddCannedResponse("I can't do that.");
    AddCannedResponse("Why?");
    AddCannedResponse("Hmm, better not.");
    AddCannedResponse("I don't think that will work.");
    AddCannedResponse("That will probably crash the game!");

    SetProtagonist(guy);

    // TODO id casing

    // Inventory
    guy.AddToInventory(groceries);
    guy.AddToInventory(groceryList);

    // park
    park.Place(narrator, 450, 150);
    park.Place(newspaper, 410, 420);
    park.Place(parkBench, 323, 408);

    // terminal
    terminal.Place(shipComputer, 450, 150);
    terminal.Place(beamButton, 73, 281);
    terminal.Place(beamGlow, 442, 450);
    terminal.Place(beamTerminal, 442, 403);
    terminal.Place(cookerInCrate, 280, 450);
    terminal.Place(crateLeft, 148, 450);
    terminal.Place(crateRight, 256, 450);
    terminal.Place(crateRightFront, 256, 450);
    terminal.Place(crateTop, 204, 390);
    terminal.Place(terminalDoor, 774, 332);

     // bridge
    bridge.Place(al, 990, 375);
    bridge.Place(ian, 250, 420);
    bridge.Place(carl, 305, 361);
    bridge.Place(richard, 185, 361);
    ian.FaceAwayFromCamera();

    bridge.Place(bottle, 1360, 288);
    bridge.Place(cheeseGrater, 1390, 328);
    bridge.Place(counterTop, 1460, 331);
    bridge.Place(bridgeDoor, 22, 444);
    bridge.Place(clawHammer, 245, 350);
    bridge.Place(fridge, 1291, 330);
    bridge.Place(onAir, 246, 262);
    bridge.Place(outletBooth, 429, 356);
    bridge.Place(outletKitchen, 1562, 399);
    bridge.Place(podcastBooth, 246, 410);
    bridge.Place(powerCord, 390, 392);
    bridge.Place(saucages, 1225, 296);
    bridge.Place(todoList, 660, 250);

    ChangeRoom(park);
});
