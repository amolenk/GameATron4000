namespace Amolenk.GameATron4000.Model;

public class StateManager
{
    private readonly Dictionary<string, object> _dynamicState;
    
    public StateManager()
    {
        _dynamicState = new();
    }

    public T? Get<T>(string key)
    {
        if (_dynamicState.TryGetValue(key, out object value))
        {
            return (value is T typedValue) ? typedValue : default(T);
        }

        return default(T);
    }

    public void Set<T>(string key, T value)
    {
        if (value == null || value.Equals(default(T)))
        {
            _dynamicState.Remove(key);
        }
        else
        {
            _dynamicState[key] = value;
        }
    }
}
