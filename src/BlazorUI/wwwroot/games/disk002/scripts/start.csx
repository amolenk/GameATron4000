Actor guy = AddActor("guy", builder => builder
    .Untouchable());

Item newspaper = AddItem(nameof(newspaper), builder => builder
    .Named("old newspaper")
    .Untouchable());

Room park = AddRoom("park", room => room
    .WithWalkboxArea(
        new Point(0, 0),
        new Point(1, 0),
        new Point(1, 1)));

OnGameStart(() =>
{
    park.Place(guy, 600, 430);
    park.Place(newspaper, 410, 420);

    SetProtagonist(guy);

    ChangeRoom(park);
});
