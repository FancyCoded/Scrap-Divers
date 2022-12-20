using UnityEngine;

public class ItemVisitParams
{
    public readonly Transform Parent;
    public readonly Vector3 Position;

    public ItemVisitParams(Transform parent, Vector3 position)
    {
        Parent = parent;
        Position = position;
    }
}