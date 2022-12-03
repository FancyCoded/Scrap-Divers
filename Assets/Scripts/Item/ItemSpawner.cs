using UnityEngine;

public class ItemSpawner : ItemPool
{
    public void Spawn(ItemType type, Transform parent, Vector3 position)
    {
        Item item = GetItem(type);

        item.Init(this);
        item.transform.SetParent(parent);
        item.transform.position = position;
        item.gameObject.SetActive(true);
    }
}