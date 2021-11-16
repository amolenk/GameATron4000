namespace Amolenk.GameATron4000.BlazorUI.Graphics;

public class ObjectSpriteCollection : KeyedCollection<string, IObjectSprite>
{
    public void ClearAndDisposeSprites()
    {
        foreach (var sprite in Items)
        {
            sprite.Dispose();
        }

        base.Clear();
    }

    public bool TryGetSprite<TSprite>(
        string id,
        [MaybeNullWhen(false)] out TSprite sprite)
        where TSprite : IObjectSprite
    {
        if (TryGetValue(id, out IObjectSprite objectSprite))
        {
            if (objectSprite is TSprite concreteSprite)
            {
                sprite = concreteSprite;
                return true;
            }
        }

        sprite = default(TSprite);
        return false;
    }

    protected override string GetKeyForItem(IObjectSprite sprite) =>
        sprite.Id;
}
