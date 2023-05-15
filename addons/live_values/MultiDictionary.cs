using System.Collections.Generic;

public partial class MultiDictionary<TKey, TVal>
{
    Dictionary<TKey, List<TVal>> _dictionary = new Dictionary<TKey, List<TVal>>();
    
    public void Add(TKey key, TVal value)
    {
        // Add a key.
        List<TVal> list;
        if (_dictionary.TryGetValue(key, out list))
        {
            list.Add(value);
        }
        else
        {
            list = new List<TVal>();
            list.Add(value);
            _dictionary[key] = list;
        }
    }
    
    public IEnumerable<TKey> Keys => _dictionary.Keys;

    public IEnumerable<TVal> Values
    {
        get
        {
            foreach (var pair in _dictionary)
            {
                foreach (TVal val in pair.Value)
                {
                    yield return val;
                }
            }
        }
    }

    public List<TVal> this[TKey key]
    {
        get
        {
            // Get list at a key.
            List<TVal> list;
            if (!_dictionary.TryGetValue(key, out list))
            {
                list = new List<TVal>();
                _dictionary[key] = list;
            }
            return list;
        }
    }
}