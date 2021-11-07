// namespace Amolenk.GameATron4000.Scripting.Model;

// public class GiveCommand : Command
// {
// 	private readonly Item? _item;
// 	private readonly Actor? _actor;

// 	public override bool IsComplete => _item != null && _actor != null;

//     public GiveCommand() : this(null, null)
//     {
//     }

//     public GiveCommand(Item? item) : this(item, null)
//     {
//     }

//     public GiveCommand(Item? item, Actor? actor)
//     {
//         _item = item;
//         _actor = actor;
//     }

// 	public override bool TrySetObject(
//         GameObject obj,
//         [MaybeNullWhen(false)] out Command command)
// 	{
//         if (_item is null && obj is Item item) 
//         {
//             // TODO Check that item is in inventory
//             command = new GiveCommand(item);
//             return true;
//         }

//         if (_item is not null && obj is Actor actor)
//         {
//             command = new GiveCommand(_item, actor);
//             return true;
//         }

//         return base.TrySetObject(obj, out command);
// 	}

//     public override string ToString()
// 	{
// 		var stringBuilder = new StringBuilder("Give");
		
// 		if (_item != null)
// 		{
// 			stringBuilder.Append($" {_item} to");
// 		}

// 		if (_actor != null)
// 		{
// 			stringBuilder.Append($" {_actor}");
// 		}

// 		return stringBuilder.ToString();
// 	}
// }