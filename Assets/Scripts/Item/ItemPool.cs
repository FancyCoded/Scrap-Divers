using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    private const uint InitialCount = 20;

    [SerializeField] private Transform _container;
    [SerializeField] private List<Item> _templates;

    private List<Item> _pool = new List<Item>();

    private Dictionary<ItemType, Item> _templatePairs = new Dictionary<ItemType, Item>();

    public void Init()
    {
        if(_container.childCount > 0)
            return;

        for (int i = 0; i < _templates.Count; i++)
        {
            _templatePairs.Add(_templates[i].Type, _templates[i]);

            for(int x = 0; x < InitialCount; x++)
                Create(_templates[i]);
        }
    }

    public Item GetItem(ItemType type)
    {
        for(int i = 0; i < _pool.Count; i++)
        {
            if (_pool[i].Type == type && _pool[i].gameObject.activeSelf == false)
                return _pool[i];
        }

        return Create(_templatePairs[type]);
    }

    public Item Create(Item template)
    {
        Item instance = Instantiate(template, _container);

        instance.gameObject.SetActive(false);
        _pool.Add(instance);

        return instance;
    }
}
