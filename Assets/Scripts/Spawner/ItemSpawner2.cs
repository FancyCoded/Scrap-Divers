using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner2 : MonoBehaviour, IDisposable
{
    [SerializeField] private ItemPool2 _itemPool;

    private readonly List<Item> _spawnedItems = new List<Item>();
    private LevelProperties _levelProperties;

    private ObjectSpawner<Magnet> _magnetSpawner;
    private ObjectSpawner<Nut> _nutSpawner;
    private ObjectSpawner<Wrench> _wrenchSpawner;
    private ObjectSpawner<Star> _starSpawner;
    private ObjectSpawner<Feather> _featherSpawner;

    public void Init()
    {
        Dispose();

        _magnetSpawner = new ObjectSpawner<Magnet>(GetMagnet, OnSpawned);
        _featherSpawner = new ObjectSpawner<Feather>(GetFeather, OnSpawned);
        _nutSpawner = new ObjectSpawner<Nut>(GetNut, OnSpawned);
        _starSpawner = new ObjectSpawner<Star>(GetStar, OnSpawned);
        _wrenchSpawner = new ObjectSpawner<Wrench>(GetWrench, OnSpawned);
    }

    public void SpawnBy(ItemType type, Vector3 position, Transform parent = null)
    {
        if (type == ItemType.Magnet)
            _magnetSpawner.Spawn(position, parent);
        if (type == ItemType.Nut)
            _nutSpawner.Spawn(position, parent);
        if (type == ItemType.Wrench)
            _wrenchSpawner.Spawn(position, parent);
        if (type == ItemType.Star)
            _starSpawner.Spawn(position, parent);
        if (type == ItemType.Feather)
            _featherSpawner.Spawn(position, parent);
    }

    public void Dispose()
    {
        _spawnedItems.Clear();
        _itemPool.Dispose();
    }

    public void Release(ref Item item)
    {
        _spawnedItems.Remove(item);
        _itemPool.Release(ref item);
    }

    private void OnSpawned(Item item)
    {
        _spawnedItems.Add(item);
    }

    private Magnet GetMagnet() => _itemPool.MagnetPool.Get();
    private Nut GetNut() => _itemPool.NutPool.Get();
    private Star GetStar() => _itemPool.StarPool.Get();
    private Wrench GetWrench() => _itemPool.WrenchPool.Get();
    private Feather GetFeather() => _itemPool.FeatherPool.Get();
}