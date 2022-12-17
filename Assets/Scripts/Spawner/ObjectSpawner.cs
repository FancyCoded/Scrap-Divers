using UnityEngine;
using System;

public class ObjectSpawner<T> : IObjectSpawner<T> where T : Component
{
    private readonly Func<T> _get; 

    public event Action<T> ObjectSpawned;

    public ObjectSpawner(Func<T> get, Action<T> actionOnSpawned)
    {
        _get = get;
        ObjectSpawned = actionOnSpawned;
    }

    public void Spawn(Vector3 position, Transform parent = null) 
    {
        T entity = _get();

        entity.transform.SetPositionAndRotation(position, Quaternion.identity);
        entity.transform.SetParent(parent);

        ObjectSpawned(entity);
    }
}