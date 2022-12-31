using UnityEngine;

[RequireComponent(typeof(NutMovement))]
public class Nut : Item
{
    private NutMovement _nutMovement;

    public NutMovement Movement => _nutMovement;

    private void Awake()
    {
        _nutMovement = GetComponent<NutMovement>();
    }

    private void OnDisable()
    {
        if (_nutMovement.enabled)
            _nutMovement.Disable();
    }
}
