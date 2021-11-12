namespace Amolenk.GameATron4000.Model;

public record GameObjectCondition(GameObject Other, string FrameName)
{
    public bool IsTrue => Other.Frame == FrameName;
}
