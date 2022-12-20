using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    [SerializeField] private uint _initialCount;
    [SerializeField] private Transform _container;
    [SerializeField] private Item[] _templates;

    private readonly Dictionary<ItemType, Item> _items = new Dictionary<ItemType, Item>();
    private readonly List<Item> _buffs = new List<Item>();

    private ObjectPool<Magnet> _magnetPool;
    private ObjectPool<Nut> _nutPool; 
    private ObjectPool<Feather> _featherPool;
    private ObjectPool<Star> _starPool;
    private ObjectPool<Wrench> _wrenchPool;

    private ItemPoolFunctions _itemPoolFunctions;

    public Nut Nut => (Nut)_items[ItemType.Nut];
    public List<Item> Buffs => _buffs;

    public void Init()
    {
        _magnetPool = new ObjectPool<Magnet>(CreateMagnet, OnGot, OnReleased, OnDestoyed);
        _wrenchPool = new ObjectPool<Wrench>(CreateWrench, OnGot, OnReleased, OnDestoyed);
        _starPool = new ObjectPool<Star>(CreateStar, OnGot, OnReleased, OnDestoyed);
        _nutPool = new ObjectPool<Nut>(CreateNut, OnGot, OnReleased, OnDestoyed);
        _featherPool = new ObjectPool<Feather>(CreateFeather, OnGot, OnReleased, OnDestoyed);
        _itemPoolFunctions = new ItemPoolFunctions(_featherPool, _magnetPool, _wrenchPool, _starPool, _nutPool);

        for (int i = 0; i < _templates.Length; i++)
        {
            _items.Add(_templates[i].Type, _templates[i]);

            for(int j = 0; j < _initialCount; j++)
                Release(Create(_templates[i]));

            if (_templates[i] is Nut)
                continue;
            else
                _buffs.Add(_templates[i]);
        }
    }

    public void Release(Item item) => _itemPoolFunctions.Visit(item);

    public Nut GetNut() => _nutPool.Get();
    public Wrench GetWrench() => _wrenchPool.Get();
    public Star GetStar() => _starPool.Get();
    public Magnet GetMagnet() => _magnetPool.Get();
    public Feather GetFeather() => _featherPool.Get();

    private Nut CreateNut() => (Nut)Create(_items[ItemType.Nut]);
    private Feather CreateFeather() => (Feather)Create(_items[ItemType.Feather]);
    private Star CreateStar() => (Star)Create(_items[ItemType.Star]);
    private Magnet CreateMagnet() => (Magnet)Create(_items[ItemType.Magnet]);
    private Wrench CreateWrench() => (Wrench)Create(_items[ItemType.Wrench]);
    private Item Create(Item template) => Instantiate(template, _container);

    private void OnReleased(Item item)
    {
        item.gameObject.SetActive(false);
        item.transform.parent = _container;
        item.transform.position = Vector3.zero;
    }

    private void OnGot(Item item) => item.gameObject.SetActive(true);
    
    private void OnDestoyed(Item item) => Destroy(item.gameObject);

    private class ItemPoolFunctions : IItemVisitor
    {
        private readonly ObjectPool<Feather> _featherPool;
        private readonly ObjectPool<Magnet> _magnetPool;
        private readonly ObjectPool<Wrench> _wrenchPool;
        private readonly ObjectPool<Star> _starPool;
        private readonly ObjectPool<Nut> _nutPool;

        public ItemPoolFunctions(ObjectPool<Feather> featherPool, 
            ObjectPool<Magnet> magnetPool, 
            ObjectPool<Wrench> wrenchPool, 
            ObjectPool<Star> starPool, 
            ObjectPool<Nut> nutPool)
        {
            _featherPool = featherPool;
            _magnetPool = magnetPool;
            _wrenchPool = wrenchPool;
            _starPool = starPool;
            _nutPool = nutPool;
        }

        public void Visit(Item item, ItemVisitParams visitParams = null) => Visit((dynamic)item, visitParams);
        public void Visit(Magnet magnet, ItemVisitParams visitParams) => _magnetPool.Release(magnet);
        public void Visit(Star star, ItemVisitParams visitParams) => _starPool.Release(star);
        public void Visit(Feather feather, ItemVisitParams visitParams) => _featherPool.Release(feather);
        public void Visit(Wrench wrench, ItemVisitParams visitParams) => _wrenchPool.Release(wrench);
        public void Visit(Nut nut, ItemVisitParams visitParams) => _nutPool.Release(nut);
    }
}