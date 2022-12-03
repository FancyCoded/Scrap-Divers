using UnityEngine;

public class Feather : Item
{
    [SerializeField] private float _targetSpeed;
    [SerializeField] private float _duration;

    public float Duration => _duration;
    public float TargetSpeed => _targetSpeed;
}