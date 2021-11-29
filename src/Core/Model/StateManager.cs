// using System.Collections;
// using System.Text.Json;
// using System.Text.Json.Serialization;

// namespace Amolenk.GameATron4000.Model;

// public class StateConverter : JsonConverter<Dictionary<string, object>>
// {
//     public override Dictionary<string, object>? Read(
//         ref Utf8JsonReader reader,
//         Type typeToConvert,
//         JsonSerializerOptions options)
//     {
//         return new Dictionary<string, object>();
//     }

//     public override void Write(
//         Utf8JsonWriter writer,
//         Dictionary<string, object> value,
//         JsonSerializerOptions options)
//     {
//         Console.WriteLine("Write");

//         writer.WriteStartArray();

//         foreach (var entry in value)
//         {
//             var entryType = entry.Value.GetType();

//             writer.WriteStartObject();
//             writer.WriteString("key", entry.Key);
//             writer.WriteString("type", entryType.FullName);
//             writer.WritePropertyName("value");
//             writer.WriteStringValue("foo");
//             // JsonSerializer.Serialize(writer, entry.Value, entryType, options);
//             writer.WriteEndObject();
//         }

//         writer.WriteEndArray();
//     }
// }

// public class StateManager
// {
//     [JsonConverter(typeof(StateConverter))]
//     private readonly Dictionary<string, object> _state;
    
//     public StateManager()
//     {
//         _state = new();
//     }

//     public void Save(Utf8JsonWriter writer)
//     {
//         writer.WriteStartArray();

//         foreach (var entry in _state)
//         {
//             var entryType = entry.Value.GetType();

//             writer.WriteStartObject();
//             writer.WriteString("key", entry.Key);
//             writer.WriteString("type", entryType.FullName);
//             writer.WritePropertyName("value");

//             Serialize(writer, entry.Value, entryType);
//             writer.WriteEndObject();
//         }

//         writer.WriteEndArray();
//     }

//     public T? Get<T>(string key)
//     {
//         if (_state.TryGetValue(key, out object value))
//         {
//             return (value is T typedValue) ? typedValue : default(T);
//         }

//         return default(T);
//     }

//     public bool TryGetValue<T>(string key, [MaybeNullWhen(false)] out T value)
//     {
//         if (_state.TryGetValue(key, out object state))
//         {
//             value = (T)state;
//             return true;
//         }

//         value = default(T);
//         return false;
//     }

//     public void Set<T>(string key, T value)
//     {
//         if (value == null || value.Equals(default(T)))
//         {
//             _state.Remove(key);
//         }
//         else
//         {
//             _state[key] = value;
//         }
//     }

//     public void AddToList<T>(string listKey, T value)
//     {
//         List<T> list;
//         if (!TryGetValue<List<T>>(listKey, out list))
//         {
//             list = new();
//             Set(listKey, list);
//         }

//         list.Add(value);
//     }

//     public void RemoveFromList<T>(string listKey, T value)
//     {
//         List<T> list;
//         if (TryGetValue<List<T>>(listKey, out list))
//         {
//             list.Remove(value);
//         }
//     }

//     private void Serialize(Utf8JsonWriter writer, object value, Type type)
//     {
//         if (type.IsGenericType)
//         {
//             var genericTypeDefinition = type.GetGenericTypeDefinition();
//             if (genericTypeDefinition == typeof(List<>))
//             {
//                 var itemType = type.GetGenericArguments().Single();
//                 writer.WriteStartArray();

//                 foreach (var item in (IList)value)
//                 {
//                     Serialize(writer, item, itemType);
//                 }

//                 writer.WriteEndArray();
//             }
//         }
//         // TODO Custom interface or JSON Converter
//         else if (type.IsAssignableTo(typeof(GameObject)))
//         {
//             ((GameObject)value).Save(writer);
//             // writer.WriteStartObject();
//             // writer.WriteString("foo", "bar");
//             // writer.WriteEndObject();
//         }
//         else if (type.Equals(typeof(Room)))
//         {
//             ((Room)value).Save(writer);
//         }
//         else
//         {
//             Console.WriteLine("Serializing " + type);
//             JsonSerializer.Serialize(writer, value, type);
// //            writer.WriteStringValue("foo");
//         }

//     }
// }
