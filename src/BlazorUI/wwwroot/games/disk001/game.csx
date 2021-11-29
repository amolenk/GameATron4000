Item cheesegrater = AddItem(nameof(cheesegrater), builder => builder
    .Named("cheese grater")
    .CanBeUsedWithOtherObject()
    .When.Use(with =>
    {
        protagonist.RemoveFromInventory(blockOfCheese);
        protagonist.AddToInventory(gratedCheese);
    }));

Item blockOfCheese = AddItem(nameof(blockOfCheese), builder => builder
    .Named("block of cheese")
    .CanBeUsedWithOtherObject()
    .When.Use(with => cheesegrater.ActionHandlers.HandleUse(blockOfCheese)));

Item gratedCheese = AddItem(nameof(gratedCheese), builder => builder
    .Named("grated cheese"));

Actor protagonist = AddActor(nameof(protagonist));

Room emptyRoom = AddRoom(nameof(emptyRoom), builder => builder
    .WithWalkboxArea(
        new Point(0, 0),
        new Point(1, 0),
        new Point(1, 1)));

OnGameStart(() =>
{
    protagonist.AddToInventory(cheesegrater);
    protagonist.AddToInventory(blockOfCheese);

    SetProtagonist(protagonist);

    ChangeRoom(emptyRoom);
});
