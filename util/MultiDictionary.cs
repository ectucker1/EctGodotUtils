using System;
using System.Collections.Generic;
using System.Collections.Immutable;

public class MultiDictionary<TKey, TVal>
{
    private readonly Dictionary<TKey, HashSet<TVal>> _dictionary = new();
        
    public void Add(TKey key, TVal value)
    {
        // Add a key.
        HashSet<TVal> set;
        if (_dictionary.TryGetValue(key, out set))
        {
            set.Add(value);
        }
        else
        {
            set = new HashSet<TVal>();
            set.Add(value);
            _dictionary[key] = set;
        }
    }

    public void Remove(TKey key, TVal value)
    {
        if (_dictionary.TryGetValue(key, out var set))
        {
            set.Remove(value);
        }
    }

    public int RemoveWhereValue(Predicate<TVal> predicate)
    {
        int total = 0;
        foreach (HashSet<TVal> pair in _dictionary.Values)
        {
            total += pair.RemoveWhere(predicate);
        }
        return total;
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
    
    public ImmutableHashSet<TVal> this[TKey key]
    {
        get
        {
            // Get list at a key.
            HashSet<TVal> set;
            if (!_dictionary.TryGetValue(key, out set))
            {
                set = new HashSet<TVal>();
                _dictionary[key] = set;
            }
            return set.ToImmutableHashSet();
        }
    }

    public void Clear()
    {
        _dictionary.Clear();
    }
}
