namespace Amolenk.GameATron4000.Infrastructure.Phaser;

public sealed class PhaserText : IText
{
    private readonly IJSInProcessRuntime _jsRuntime;

    public string Key { get; private set; }
    public Point Position { get; private set; }

    private PhaserText(
        string key,
        Point position,
        IJSInProcessRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;

        Key = key;
        Position = position;
    }

    public static IText Create(
        string text,
        Point position,
        TextOptions options,
        IJSInProcessRuntime jsRuntime)
    {
        var textKey = Guid.NewGuid().ToString();

        jsRuntime.InvokeVoid(
            PhaserConstants.Functions.AddText,
            textKey,
            position,
            text,
            options);

        return new PhaserText(
            textKey,
            position,
            jsRuntime);
    }

    public void Dispose() =>
        _jsRuntime.InvokeVoid(
            PhaserConstants.Functions.DestroyText,
            Key);
}
