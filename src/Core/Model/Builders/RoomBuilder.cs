// namespace Amolenk.GameATron4000.Model.Builders;

// public class RoomBuilder
// {
//     public Polygon? WalkboxArea { get; private set; }
    
//     public RoomHandlersBuilder When { get; private set; }

//     internal RoomBuilder()
//     {
//         When = new(this);
//     }

//     public RoomBuilder WithWalkboxArea(params Point[] vertices)
//     {
//         WalkboxArea = new Polygon(vertices);
//         return this;
//     }

//     // internal Room Build()
//     // {
//     //     if (WalkboxArea is null)
//     //     {
//     //         throw new InvalidOperationException(
//     //             "Walkbox area must be set for a room.");
//     //     }

//     //     return new Room(
//     //         Id,
//     //         Game,
//     //         new Walkbox(WalkboxArea),
//     //         new RoomHandlers(
//     //             When.HandleBeforeEnter,
//     //             When.HandleAfterEnter));
//     // }
// }