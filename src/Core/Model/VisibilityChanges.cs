namespace Amolenk.GameATron4000.Model;

public record VisibilityChanges(
    IEnumerable<GameObject> ObjectsToHide,
    IEnumerable<GameObject> ObjectsToShow)
{
    public bool Any() => ObjectsToHide.Any() && ObjectsToShow.Any();

    public static readonly VisibilityChanges Empty = new VisibilityChanges(
        Enumerable.Empty<GameObject>(),
        Enumerable.Empty<GameObject>());

    public static VisibilityChanges Combine(params VisibilityChanges[] changes)
    {
        List<GameObject> objectsToHide = new();
        List<GameObject> objectsToShow = new();

        foreach (var item in changes)
        {
            foreach (var objectToHide in item.ObjectsToHide)
            {
                if (!objectsToHide.Contains(objectToHide))
                {
                    objectsToHide.Add(objectToHide);
                }
            }

            foreach (var objectToShow in item.ObjectsToShow)
            {
                if (!objectsToShow.Contains(objectToShow))
                {
                    objectsToShow.Add(objectToShow);
                }
            }
        }

        return new VisibilityChanges(objectsToHide, objectsToShow);
    }
}
