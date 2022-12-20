using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private ItemPool _itemPool;

    private readonly List<Item> _spawnedItems = new List<Item>();

    private bool _isInited = false;

    private ObjectSpawner<Magnet> _magnetSpawner;
    private ObjectSpawner<Nut> _nutSpawner;
    private ObjectSpawner<Wrench> _wrenchSpawner;
    private ObjectSpawner<Star> _starSpawner;
    private ObjectSpawner<Feather> _featherSpawner;

    private ItemVisitor _itemVisitor;

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
        _itemVisitor = new ItemVisitor(_wrenchSpawner, _nutSpawner, _starSpawner, _featherSpawner, _magnetSpawner);
        _isInited = true; 
    }

    public void Spawn(Item item, Vector3 position, Transform parent)
    {
        ItemVisitParams itemVisitParams = new ItemVisitParams(parent, position);
        _itemVisitor.Visit(item, itemVisitParams);
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

    private class ItemVisitor : IItemVisitor
    {
        private readonly ObjectSpawner<Wrench> _wrenchSpawner;
        private readonly ObjectSpawner<Magnet> _magnetSpawner;
        private readonly ObjectSpawner<Nut> _nutSpawner;
        private readonly ObjectSpawner<Star> _starSpawner;
        private readonly ObjectSpawner<Feather> _featherSpawner;
       
        public ItemVisitor(ObjectSpawner<Wrench> wrenchSpawner,
            ObjectSpawner<Nut> nutSpawner,
            ObjectSpawner<Star> starSpawner,
            ObjectSpawner<Feather> featherSpawner,
            ObjectSpawner<Magnet> magnetSpawner)
        {
            _wrenchSpawner = wrenchSpawner;
            _magnetSpawner = magnetSpawner;
            _starSpawner = starSpawner;
            _featherSpawner = featherSpawner;
            _nutSpawner = nutSpawner;
        }

        public void Visit(Item item, ItemVisitParams visitParams) => 
            Visit((dynamic)item, visitParams);

        public void Visit(Magnet magnet, ItemVisitParams visitParams) => 
            _magnetSpawner.Spawn(visitParams.Position, visitParams.Parent);

        public void Visit(Wrench wrench, ItemVisitParams visitParams) => 
            _wrenchSpawner.Spawn(visitParams.Position, visitParams.Parent);

        public void Visit(Star star, ItemVisitParams visitParams) => 
            _starSpawner.Spawn(visitParams.Position, visitParams.Parent);

        public void Visit(Nut nut, ItemVisitParams visitParams) => 
            _nutSpawner.Spawn(visitParams.Position, visitParams.Parent);

        public void Visit(Feather feather, ItemVisitParams visitParams) => 
            _featherSpawner.Spawn(visitParams.Position, visitParams.Parent);
    }
}