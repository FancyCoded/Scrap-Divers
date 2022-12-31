using System;
using UnityEngine;

public interface IObjectPool<T> where T : Component
{
    event Action<T> Got;
    event Action<T> Released;
    event Action<T> Destroyed;

    int CountInactive { get; }

    T Get();
    void Release(T entity);
}