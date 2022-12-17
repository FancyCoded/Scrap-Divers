using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool2 : MonoBehaviour, IDisposable
{
    [SerializeField] private uint _initialCount;
    [SerializeField] private Transform _container;
    [SerializeField] private Item[] _templates;

    private readonly Dictionary<ItemType, Item> _items = new Dictionary<ItemType, Item>();
    
    private ObjectPool<Magnet> _magnetPool;
    private ObjectPool<Nut> _nutPool; 
    private ObjectPool<Feather> _featherPool;
    private ObjectPool<Star> _starPool;
    private ObjectPool<Wrench> _wrenchPool; 

    public ObjectPool<Magnet> MagnetPool => _magnetPool;
    public ObjectPool<Nut> NutPool => _nutPool;
    public ObjectPool<Star> StarPool => _starPool;
    public ObjectPool<Wrench> WrenchPool => _wrenchPool;
    public ObjectPool<Feather> FeatherPool => _featherPool;

    public void Init()
    {
        _magnetPool = new ObjectPool<Magnet>(CreateMagnet, OnGot, OnReleased, OnDestoyed);
        _wrenchPool = new ObjectPool<Wrench>(CreateWrench, OnGot, OnReleased, OnDestoyed);
        _starPool = new ObjectPool<Star>(CreateStar, OnGot, OnReleased, OnDestoyed);
        _nutPool = new ObjectPool<Nut>(CreateNut, OnGot, OnReleased, OnDestoyed);
        _featherPool = new ObjectPool<Feather>(CreateFeather, OnGot, OnReleased, OnDestoyed);

        for (int i = 0; i < _templates.Length; i++)
        {
            _items.Add(_templates[i].Type, _templates[i]);

            Item item = Create(_templates[i]);
            Release(ref item);
        }
    }

    public void Dispose()
    {
        _nutPool.Dispose();
        _magnetPool.Dispose();
        _featherPool.Dispose();
        _starPool.Dispose();
        _wrenchPool.Dispose();
    }

    private Nut CreateNut() => (Nut) Create(_items[ItemType.Nut]);
    private Feather CreateFeather() => (Feather) Create(_items[ItemType.Feather]);
    private Star CreateStar() => (Star) Create(_items[ItemType.Star]);
    private Magnet CreateMagnet() => (Magnet) Create(_items[ItemType.Magnet]);
    private Wrench CreateWrench() => (Wrench) Create(_items[ItemType.Wrench]);
    private Item Create(Item template) => Instantiate(template, _container);

    public void Release(ref Item item)
    {
        if (item is Magnet magnet)
            _magnetPool.Release(ref magnet);
        if (item is Wrench wrench)
            _wrenchPool.Release(ref wrench);
        if (item is Star star)
            _starPool.Release(ref star);
        if (item is Nut nut)
            _nutPool.Release(ref nut);
        if (item is Feather feather)
            _featherPool.Release(ref feather);
    }

    private void OnReleased(Item item)
    {
        item.gameObject.SetActive(false);
        item.transform.parent = _container;
        item.transform.position = Vector3.zero;
    }

    private void OnDestoyed(Item item)
    {
        Destroy(item.gameObject);
    }

    private void OnGot(Item item)
    {
        item.gameObject.SetActive(true);
    }
}