namespace Amolenk.GameATron4000.Engine.Scripting;

[Serializable()]
public class ScriptException : Exception
{
    public ImmutableArray<ScriptError> Errors { get; } =
        ImmutableArray.Create<ScriptError>();

    protected ScriptException() : base()
    {
    }

    public ScriptException(string message) : base(message)
    {
    }

    public ScriptException(
        string message,
        IEnumerable<ScriptError> errors) :
        base(message)
    {
        Errors = ImmutableArray.Create<ScriptError>(errors.ToArray());
    }

    public ScriptException(string message, Exception innerException) :
       base(message, innerException)
    {
    }

    protected ScriptException(
        SerializationInfo info,
        StreamingContext context) :
        base(info, context)
    {
    }
}
