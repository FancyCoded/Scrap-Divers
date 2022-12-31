using System;
using Component = UnityEngine.Component;

public class ObjectPool<T> : ObjectPoolBase<T> where T : Component
{
    public ObjectPool(Func<T> createNew,
        Action<T> actionOnDestroyed,
        Action<T> actionOnGot = null,
        Action<T> actionOnReleased = null,
        uint defaultCapacity = DefaultCapacity,
        uint maxSize = DefaultMaxSize) :
        base(actionOnDestroyed, actionOnGot, actionOnReleased, defaultCapacity, maxSize)
    {
        _createNew = createNew ?? throw new ArgumentNullException(nameof(createNew));
    }

    private readonly Func<T> _createNew;

    public sealed override T CreateNew() => _createNew();
}