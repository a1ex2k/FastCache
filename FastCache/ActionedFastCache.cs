using System;


namespace FastCache;

/// <summary>
/// <see cref="FastCache{TKey,TValue}"/> with onRemove action
/// </summary>
public class ActionedFastCache<TKey, TValue> : FastCache<TKey, TValue>
{
    private Action<TKey, TValue> _action;

    /// <summary>
    /// Initializes a new empty instance of <see cref="ActionedFastCache{TKey,TValue}"/>
    /// </summary>
    /// <param name="cleanupJobInterval">cleanup interval in milliseconds, default is 10000</param>
    public ActionedFastCache(Action<TKey, TValue> actionOnRemove, TimeSpan cleanupJobInterval, TimeSpan slidingInterval) : base(cleanupJobInterval, slidingInterval)
    {
        _action = actionOnRemove;
    }

    /// <summary>
    /// Tries to remove item with the specified key, also returns the object removed in an "out" var.
    /// <br>
    /// Invokes the action specified in constructor after successful removing
    /// </summary>
    /// <param name="key">The key of the element to remove</param>
    /// <param name="value">Contains the object removed or the default value if not found</param>
    public override bool TryRemove(TKey key, out TValue value)
    {
        var success = base.TryRemove(key, out value);
        if (success)
            _action.Invoke(key, value);
        return success;
    }
}