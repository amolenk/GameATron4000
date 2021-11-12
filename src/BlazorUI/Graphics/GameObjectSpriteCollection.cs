namespace Amolenk.GameATron4000.BlazorUI.Graphics;

// public class GameObjectSpriteCollection<TObject, TSprite> :
//     KeyedCollection<string, TSprite>
//     where TObject : GameObject
//     where TSprite : GameObjectSprite<TObject>
// {
//     public void ClearAndDisposeItems()
//     {
//         foreach (var item in Items)
//         {
//             item.Dispose();
//         }

//         base.Clear();
//     }

//     protected override string GetKeyForItem(TSprite item) =>
//         item.GameObject.Id;
// }

// public class GameObjectSpriteCollection :
//     GameObjectSpriteCollection<GameObject, GameObjectSprite>
// {
// }

public class GameObjectSpriteCollection :
    KeyedCollection<string, GameObjectSprite>
{
    public void ClearAndDisposeItems()
    {
        foreach (var item in Items)
        {
            item.Dispose();
        }

        base.Clear();
    }

    protected override string GetKeyForItem(GameObjectSprite item) =>
        item.GameObject.Id;
}
