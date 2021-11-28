var cheeseGrater = AddItem("cheese-grater", "cheese grater");
    // .AllowUseWithOtherObjects()
    // .When
    //     .Use(with =>
    //     {
    //         protagonist.RemoveFromInventory(blockOfCheese);
    //         protagonist.AddToInventory(gratedCheese);
    //     });

var blockOfCheese = AddItem("block-of-cheese", "block of cheese");
    // .AllowUseWithOtherObjects()
    // .When
    //     .Use(with => cheeseGrater.ActionHandlers.HandleUse(blockOfCheese));

var gratedCheese = AddItem("grated-cheese", "grated cheese");

var bottle = AddItem("bottle")
    .DependsOn(fridge, "open");

var fridge = (Item)AddItem("fridge").WithStatus("open");



var protagonist = AddActor("guy");

var emptyRoom = AddRoom("park")
    .WithWalkboxArea(
        new Point(0, 0),
        new Point(1, 0),
        new Point(1, 1));

OnGameStart(() =>
{
    protagonist.AddToInventory(cheeseGrater);
    protagonist.AddToInventory(blockOfCheese);

    emptyRoom.Place(protagonist, 600, 400);
    emptyRoom.Place(fridge, 400, 400);
    emptyRoom.Place(bottle, 400, 200);

    SetProtagonist(protagonist);

    ChangeRoom(emptyRoom);
});
