using System.Collections.Generic;

namespace GameATron4000.Models
{
    public class DefaultValueIfMissingDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _innerDictionary;

        public DefaultValueIfMissingDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _innerDictionary = dictionary;
        }

        public TValue this[TKey key]
        {
            get
            {
                if (_innerDictionary.ContainsKey(key))
                {
                    return _innerDictionary[key];
                }

                return default(TValue);
            }
            set
            {
                _innerDictionary[key] = value;
            }
        }
    }
}