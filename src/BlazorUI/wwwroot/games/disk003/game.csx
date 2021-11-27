var cheeseGrater = AddItem("cheese-grater", "cheese grater")
    .CanBeUsedWithOtherObjects()
    .When
        .Use(with =>
        {
            protagonist.RemoveFromInventory(blockOfCheese);
            protagonist.AddToInventory(gratedCheese);
        });

var blockOfCheese = AddItem("block-of-cheese", "block of cheese")
    .CanBeUsedWithOtherObject()
    .When
        .Use(with => cheeseGrater.ActionHandlers.HandleUse(blockOfCheese));

var gratedCheese = AddItem("grated-cheese", "grated cheese");

var fridge = AddItem("fridge");

var bottle = AddItem("bottle")
    .DependsOn(fridge, "open");

fridge.WithStatus("open");

var protagonist = AddActor("guy");

var emptyRoom = AddRoom("park");
emptyRoom.SetWalkBox(
        new Point(0, 0),
        new Point(1, 0),
        new Point(1, 1));

OnGameStart(() =>
{
    protagonist.AddToInventory(cheeseGrater);
    protagonist.AddToInventory(blockOfCheese);

    emptyRoom.Place(protagonist, 600, 400);
    emptyRoom.Place(bottle, 200, 100);
    emptyRoom.Place(fridge, 200, 400);

    SetProtagonist(protagonist);

    ChangeRoom(emptyRoom);
});
