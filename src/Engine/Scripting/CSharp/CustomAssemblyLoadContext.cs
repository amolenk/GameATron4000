namespace Amolenk.GameATron4000.Engine.Scripting.CSharp;

public class CustomAssemblyLoadContext : AssemblyLoadContext
{
    public CustomAssemblyLoadContext() : base(isCollectible: true)
    {
    }

    protected override Assembly Load(AssemblyName name)
    {
        return null!;
    }
}
