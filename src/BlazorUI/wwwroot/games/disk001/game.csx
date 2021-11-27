var cheeseGrater = AddItem("cheese-grater");
cheeseGrater.Configure(options => options
    .Named("cheese grater")
    .CanBeUsedWithOtherObject()
    .When.Use(with =>
    {
        protagonist.RemoveFromInventory(blockOfCheese);
        protagonist.AddToInventory(gratedCheese);
    }));

var blockOfCheese = AddItem("block-of-cheese");
blockOfCheese.Configure(options => options
    .Named("block of cheese")
    .CanBeUsedWithOtherObject()
    .When.Use(with => cheeseGrater.ActionHandlers.HandleUse(blockOfCheese)));

var gratedCheese = AddItem("grated-cheese");
gratedCheese.Configure(options => options
    .Named("grated cheese"));

var protagonist = AddActor("protagonist");

var emptyRoom = AddRoom("park");
emptyRoom.Configure(options => options
    .WithWalkboxArea(
        new Point(0, 0),
        new Point(1, 0),
        new Point(1, 1)));

OnGameStart(() =>
{
    protagonist.AddToInventory(cheeseGrater);
    protagonist.AddToInventory(blockOfCheese);

    SetProtagonist(protagonist);

    ChangeRoom(emptyRoom);
});
