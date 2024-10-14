OnGameStart(() =>
{
    AddCannedResponse("I can't do that.");
    AddCannedResponse("Why?");
    AddCannedResponse("Hmm, better not.");
    AddCannedResponse("I don't think that will work.");
    AddCannedResponse("That will probably crash the game!");

    // Inventory
    guy.AddToInventory(passport);
    guy.AddToInventory(membershipCard);

    // check-in
    checkIn.Place(counter, 285, 353);
    checkIn.Place(gatesArea, 691, 450);
    checkIn.Place(monitors, 400, 100);
    checkIn.Place(plantCheckIn, 670, 435);
    checkIn.Place(richard, 334, 275);

    // security
    security.Place(accessDeniedSign, 895, 380);
    security.Place(plantSecurity, 100, 500);
    security.Place(poster, 600, 250);
    security.Place(priorityLaneEntrance, 900, 372);
    security.Place(securityCrowd, 400, 240);
    security.Place(securityRope, 360, 220);
    security.Place(securitySign, 440, 245);
    security.Place(securityWall, 99, 450);

    // lobby
    lobby.Place(loungeSign, 200, 190);
    lobby.Place(plant, 240, 390);
    lobby.Place(chairs1, 50, 400);
    lobby.Place(chairs2, 800, 400);
    lobby.Place(loungeDoorClosed, 391, 231);
    lobby.Place(plantSmall, 820, 158);
    lobby.Place(carl, 650, 173);
    lobby.Place(narrator, 400, 125);

    SetProtagonist(guy);

    ChangeRoom(checkIn);
});
