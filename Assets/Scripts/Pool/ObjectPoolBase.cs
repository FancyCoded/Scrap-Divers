using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Component = UnityEngine.Component;
using Random = UnityEngine.Random;

[Serializable]
public abstract class ObjectPoolBase<T> : IDisposable, IObjectPool<T> where T : Component
{
    public const uint DefaultMaxSize = 1000;
    public const uint DefaultCapacity = 10;

    [SerializeField] private uint _maxSize = DefaultMaxSize;

    private List<T> _list;

    protected ObjectPoolBase(Action<T> actionOnDestroyed = null,
       Action<T> actionOnGot = null,
       Action<T> actionOnReleased = null,
       uint defaultCapacity = DefaultCapacity,
       uint maxSize = DefaultMaxSize)
    {
        SetList(defaultCapacity);
        Destroyed = actionOnDestroyed;
        Got = actionOnGot;
        Released = actionOnReleased;
        MaxSize = maxSize;
    }

    public event Action<T> Destroyed;
    public event Action<T> Got;
    public event Action<T> Released;

    public int CountActive => CountAll - _list.Count;
    public int CountInactive => _list.Count;
    public int CountAll { get; private set; }
    public uint MaxSize
    {
        get => _maxSize;
        set
        {
            if (value == 0)
                throw new ArgumentOutOfRangeException(nameof(_maxSize));

            _maxSize = value;
        }
    }

    public void Dispose()
    {
        if(Destroyed != null)
            foreach (T entity in _list)
                Destroyed(entity);

        _list.Clear();
        CountAll = 0;
    }

    public virtual T Get()
    {
        if (_list.Count == 0)
            return GetNew();

        T entity = _list[_list.Count - 1];

        _list.Remove(entity);

        Got?.Invoke(entity);

        return entity;
    }

    public virtual T GetRandom()
    {
        int randomIndex;

        if (_list.Count == 0)
            return GetNew();

        randomIndex = Random.Range(0, _list.Count);
        T entity = _list[randomIndex];

        _list.RemoveAt(randomIndex);

        Got?.Invoke(entity);

        return entity;
    }

    protected void SetList(uint capacity)
    {
        Assert.IsNull(_list);
        _list = new List<T>((int)capacity);
    }

    protected T GetNew()
    {
        T entity = CreateNew();
        ++CountAll;
        return entity;
    }

    public abstract T CreateNew();

    public void Release(T entity)
    {
        Released?.Invoke(entity);

        if (_list.Count < _maxSize)
            _list.Add(entity);
        else
            Destroyed?.Invoke(entity);

        entity = null;
    }
}