using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemType _type;

    private ItemSpawner _itemSpawner;

    public ItemType Type => _type;

    public virtual void Init(ItemSpawner itemSpawner)
    {
        _itemSpawner = itemSpawner;
    }

    public virtual void Pick()
    {
        gameObject.SetActive(false);

        transform.parent = _itemSpawner.transform;
        transform.position = _itemSpawner.transform.position;
    }
}
