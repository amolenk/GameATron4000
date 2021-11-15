OnGameStart(() =>
{
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
//     put_actor(al, 990, 375, bridge)
//     put_actor(ian, 250, 420, bridge)
//     put_actor(carl, 305, 361, bridge)
//     put_actor(richard, 185, 361, bridge)
//     face_dir(face_back, ian)

//     put_object(bottle, 1360, 288, bridge)
//     put_object(cheese_grater, 1390, 328, bridge)
//     put_object(countertop, 1460, 331, bridge)
//     put_object(door_bridge, 22, 444, bridge)
//     put_object(claw_hammer, 245, 350, bridge)
//     put_object(fridge, 1291, 330, bridge)
//     put_object(onair, 246, 262, bridge)
//     put_object(outlet_booth, 429, 356, bridge)
//     put_object(outlet_kitchen, 1562, 399, bridge)
//     put_object(podcast_booth, 246, 410, bridge)
//     put_object(power_cord, 390, 392, bridge)
//     put_object(saucages, 1225, 296, bridge)
//     put_object(todolist, 660, 250, bridge)

    ChangeRoom(park);
});
