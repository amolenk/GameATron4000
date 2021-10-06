namespace Amolenk.GameATron4000.Engine;

public abstract class Scene
{
    public string Id { get; }

    protected Scene(string id)
    {
        Id = id;
    }

    public virtual Task PreloadAsync(IAssetLoader loader) =>
        Task.CompletedTask;

    public virtual Task CreateAsync(IGraphics graphics) =>
        Task.CompletedTask;

    public virtual Task UpdateAsync(IGraphics graphics) =>
        Task.CompletedTask;
}
