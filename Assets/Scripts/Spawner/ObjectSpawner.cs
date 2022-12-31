using System;
using UnityEngine;

public class ObjectSpawner<T> : IObjectSpawner<T> where T : Component
{
    public ObjectSpawner(Func<T> get, Action<T> actionOnSpawned)
    {
        _get = get;
        ObjectSpawned = actionOnSpawned;
    }

    private readonly Func<T> _get;

    public event Action<T> ObjectSpawned;

    public void Spawn(Vector3 position, Transform parent = null)
    {
        T entity = _get();

        entity.transform.position = position;

        if (parent)
            entity.transform.SetParent(parent);

        ObjectSpawned(entity);
    }
}