using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private ItemPool _itemPool;

    private readonly List<Item> _spawnedItems = new List<Item>();

    private bool _isInited = false;

    private IObjectSpawner<Magnet> _magnetSpawner;
    private IObjectSpawner<Nut> _nutSpawner;
    private IObjectSpawner<Wrench> _wrenchSpawner;
    private IObjectSpawner<Star> _starSpawner;
    private IObjectSpawner<Feather> _featherSpawner;

    public Nut Nut => _itemPool.Nut;
    public List<Item> Buffs => _itemPool.Buffs;

    public void Init()
    {
        if (_isInited)
            return;

        _itemPool.Init();

        _magnetSpawner = new ObjectSpawner<Magnet>(GetMagnet, OnSpawned);
        _featherSpawner = new ObjectSpawner<Feather>(GetFeather, OnSpawned);
        _nutSpawner = new ObjectSpawner<Nut>(GetNut, OnSpawned);
        _starSpawner = new ObjectSpawner<Star>(GetStar, OnSpawned);
        _wrenchSpawner = new ObjectSpawner<Wrench>(GetWrench, OnSpawned);
        _isInited = true;
    }

    public void Spawn(Item item, Vector3 position, Transform parent)
    {
        if (item is Nut)
            _nutSpawner.Spawn(position, parent);
        if (item is Wrench)
            _wrenchSpawner.Spawn(position, parent);
        if (item is Star)
            _starSpawner.Spawn(position, parent);
        if (item is Magnet)
            _magnetSpawner.Spawn(position, parent);
        if (item is Feather)
            _featherSpawner.Spawn(position, parent);
    }

    public void Release(Item item)
    {
        _spawnedItems.Remove(item);
        _itemPool.Release(item);
    }

    private void OnSpawned(Item item) => _spawnedItems.Add(item);

    private Magnet GetMagnet() => _itemPool.GetMagnet();
    private Nut GetNut() => _itemPool.GetNut();
    private Star GetStar() => _itemPool.GetStar();
    private Wrench GetWrench() => _itemPool.GetWrench();
    private Feather GetFeather() => _itemPool.GetFeather();
}