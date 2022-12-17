using System;
using Component = UnityEngine.Component;

public class ObjectPool<T> : ObjectPoolBase<T> where T : Component
{
    private readonly Func<T> _createNew; 

    public ObjectPool(Func<T> createNew,
        Action<T> actionOnGot = null,
        Action<T> actionOnReleased = null,
        Action<T> actionOnDestroyed = null,
        uint defaultCapacity = DefaultCapacity,
        uint maxSize = DefaultMaxSize,
        bool collectionCheckDefault = DefaultCollectionCheck) : 
        base(collectionCheckDefault, actionOnGot, actionOnReleased, actionOnDestroyed, defaultCapacity, maxSize)
    {
        _createNew = createNew ?? throw new ArgumentNullException(nameof(createNew));
    }

    public sealed override T CreateNew()
    {
        return _createNew();
    }
}