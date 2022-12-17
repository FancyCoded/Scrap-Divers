using System;
using UnityEngine;

public interface IObjectSpawner<T> where T : Component
{
    event Action<T> ObjectSpawned;

    void Spawn(Vector3 position, Transform parent = null);
}