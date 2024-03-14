using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ListDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
{
    private readonly List<KeyValuePair<TKey, TValue>> _list = new();
    private readonly Dictionary<TKey, TValue> _dictionary = new();

    public int Count => _list.Count;

    public IEnumerable<TKey> Keys => _list.Select(pair => pair.Key);
    public IEnumerable<TValue> Values => _list.Select(pair => pair.Value);
    
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(TKey key, TValue value)
    {
        KeyValuePair<TKey, TValue> pair = new KeyValuePair<TKey, TValue>(key, value);
        _dictionary.Add(key, value);
        _list.Add(pair);
    }
    
    public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> collection)
    {
        foreach (var pair in collection)
        {
            _dictionary.Add(pair.Key, pair.Value);
            _list.Add(pair);
        }
    }

    public void Remove(TKey key)
    {
        _dictionary.Remove(key);
        _list.RemoveAll(pair => pair.Key.Equals(key));
    }

    public void Clear()
    {
        _list.Clear();
        _dictionary.Clear();
    }

    public TValue ByKey(TKey key)
    {
        if (_dictionary.TryGetValue(key, out var value))
        {
            return value;
        }

        return default;
    }

    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public bool TryByKey(TKey key, out TValue value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    public TValue ByIndex(int index)
    {
        return _list[index].Value;
    }
    
    public int IndexOf(TKey key)
    {
        return _list.FindIndex(pair => pair.Key.Equals(key));
    }

    public void Swap(int first, int second)
    {
        (_list[first], _list[second]) = (_list[second], _list[first]);
    }

    public ImmutableListDictionary<TKey, TValue> ToImmutableListDictionary()
    {
        return new ImmutableListDictionary<TKey, TValue>(this);
    }
}

public class ImmutableListDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
{
    private readonly ListDictionary<TKey, TValue> _from;
    
    public int Count => _from.Count;

    public bool IsEmpty => Count <= 0;

    public IEnumerable<TKey> Keys => _from.Keys;
    public IEnumerable<TValue> Values => _from.Values;

    public ImmutableListDictionary(ListDictionary<TKey, TValue> from)
    {
        _from = from;
    }
    
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _from.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public TValue ByKey(TKey key)
    {
        return _from.ByKey(key);
    }

    public bool TryByKey(TKey key, out TValue value)
    {
        return _from.TryByKey(key, out value);
    }

    public TValue ByIndex(int index)
    {
        return _from.ByIndex(index);
    }

    public int IndexOf(TKey key)
    {
        return _from.IndexOf(key);
    }
}