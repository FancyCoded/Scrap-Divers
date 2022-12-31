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

    public Nut Nut => (Nut)_items[ItemType.Nut];
    public List<Item> Buffs => _buffs;

    public void Init()
    {
        _magnetPool = new ObjectPool<Magnet>(CreateMagnet, OnDestoyed, OnGot, OnReleased);
        _wrenchPool = new ObjectPool<Wrench>(CreateWrench, OnDestoyed, OnGot, OnReleased);
        _starPool = new ObjectPool<Star>(CreateStar, OnDestoyed, OnGot, OnReleased);
        _nutPool = new ObjectPool<Nut>(CreateNut, OnDestoyed, OnGot, OnReleased);
        _featherPool = new ObjectPool<Feather>(CreateFeather, OnDestoyed, OnGot, OnReleased);

        for (int i = 0; i < _templates.Length; i++)
        {
            _items.Add(_templates[i].Type, _templates[i]);

            for (int j = 0; j < _initialCount; j++)
            {
                Item item = Create(_templates[i]);
                Release(item);
            }

            if (_templates[i] is Nut)
                continue;
            else
                _buffs.Add(_templates[i]);
        }
    }

    public void Release(Item item)
    {
        if (item is Feather feather)
            _featherPool.Release(feather);
        if (item is Star star)
            _starPool.Release(star);
        if (item is Wrench wrench)
            _wrenchPool.Release(wrench);
        if (item is Nut nut)
            _nutPool.Release(nut);
        if (item is Magnet magnet)
            _magnetPool.Release(magnet);
    }

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
}