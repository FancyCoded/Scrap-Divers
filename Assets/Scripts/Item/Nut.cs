using UnityEngine;

[RequireComponent(typeof(NutMovement))]
public class Nut : Item
{
    private NutMovement _nutMovement;

    public NutMovement Movement => _nutMovement;

    public override void Init(ItemSpawner itemSpawner)
    {
        base.Init(itemSpawner);
        _nutMovement = GetComponent<NutMovement>();
    }

    public override void Pick()
    {
        base.Pick();
        _nutMovement.Disable();
    }
}
