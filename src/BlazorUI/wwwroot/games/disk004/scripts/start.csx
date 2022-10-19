OnGameStart(() =>
{
    // terminal
    terminal.Place(crateLeft, 148, 450);
    terminal.Place(crateRight, 256, 450);
    terminal.Place(crateTop, 204, 390);
    terminal.Place(terminalDoor, 774, 332);

     // bridge
    bridge.Place(al, 850, 375);
    bridge.Place(carl, 305, 361);
    bridge.Place(richard, 185, 361);

    bridge.Place(counterTop, 1460, 331);
    bridge.Place(bridgeDoor, 22, 444);
    bridge.Place(fridge, 1291, 330);
    bridge.Place(onAir, 246, 262);
    bridge.Place(outletBooth, 429, 356);
    bridge.Place(outletKitchen, 1562, 399);
    bridge.Place(podcastBooth, 246, 410);
    bridge.Place(powerCord, 390, 392);

    SetProtagonist(guy);

    ChangeRoom(terminal);
});
