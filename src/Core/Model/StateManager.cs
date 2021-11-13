namespace Amolenk.GameATron4000.Model;

public class StateManager
{
    private readonly Dictionary<string, object> _state;
    
    public StateManager()
    {
        _state = new();
    }

    public T? Get<T>(string key)
    {
        if (_state.TryGetValue(key, out object value))
        {
            return (value is T typedValue) ? typedValue : default(T);
        }

        return default(T);
    }

    public bool TryGetValue<T>(string key, [MaybeNullWhen(false)] out T value)
    {
        if (_state.TryGetValue(key, out object state))
        {
            value = (T)state;
            return true;
        }

        value = default(T);
        return false;
    }

    public void Set<T>(string key, T value)
    {
        if (value == null || value.Equals(default(T)))
        {
            _state.Remove(key);
        }
        else
        {
            _state[key] = value;
        }
    }
}
