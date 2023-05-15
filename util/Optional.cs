using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// An optional type, which may or may not contain a value.
/// </summary>
/// <typeparam name="T">The type potentially stored in this optional.</typeparam>
public partial class Optional<T> : IEnumerable<T>
{
    private T _value;
    private bool _hasValue = false;
    
    /// <summary>
    /// Returns true if this Optional has a value.
    /// </summary>
    public bool IsSome => _hasValue;

    /// <summary>
    /// Returns true if this Optional does not have a value.
    /// </summary>
    public bool IsNone => !_hasValue;

    /// <summary>
    /// Returns the value of this Optional.
    /// This should be avoided in mot cases - try GetOr, Match..., or a foreach loop.
    /// </summary>
    /// <exception cref="NullReferenceException">If there is no value</exception>
    public T Value
    {
        get
        {
            if (!_hasValue) throw new NullReferenceException();
            return _value;
        }
    }
    
    /// <summary>
    /// An new Optional without a value.
    /// </summary>
    public static Optional<T> None => new Optional<T>();
    
    /// <summary>
    /// Creates a new Optional with the given value.
    /// </summary>
    /// <param name="value">The value of the created Optional. Should not be null.</param>
    /// <returns>The created Optional.</returns>
    public static Optional<T> Some(T value) => new Optional<T>(value);

    private Optional() {}
    
    private Optional(T value)
    {
        _value = value;
        _hasValue = true;
    }

    /// <summary>
    /// Returns the value of this Optional if it exists, or a default otherwise.
    /// </summary>
    /// <param name="defaultVal">The value to return if this Optional has no value.</param>
    /// <returns>Either the value of this Optional or defaultVal</returns>
    public T GetOr(T defaultVal) => _hasValue ? _value : defaultVal;

    /// <summary>
    /// Calls one of the two provided functions, based on the value of this Optional.
    /// </summary>
    /// <param name="some">The function to call if this Optional has a value.</param>
    /// <param name="none">The function to call if this Optional has no value.</param>
    /// <typeparam name="TResult">The type to return.</typeparam>
    /// <returns>The return value of either the some or none function.</returns>
    public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none) => _hasValue ? some(_value) : none();

    /// <summary>
    /// Calls the given function if this Optional has a value.
    /// </summary>
    /// <param name="some">The function to call</param>
    public void MatchSome(Action<T> some)
    {
        if (_hasValue)
            some(_value);
    }

    /// <summary>
    /// Calls the given function if this Optional has no value.
    /// </summary>
    /// <param name="none">The function to call</param>
    public void MatchNone(Action none)
    {
        if (!_hasValue)
            none();
    }

    public IEnumerator<T> GetEnumerator()
    {
        if (_hasValue)
            yield return _value;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public override bool Equals(object obj)
    {
        if (obj is Optional<T> opt)
        {
            return _hasValue ? opt.IsSome && opt._value.Equals(_value) : opt.IsNone;
        }
        
        return false;
    }

    public override int GetHashCode() => _hasValue ? _value.GetHashCode() : 0;
}
