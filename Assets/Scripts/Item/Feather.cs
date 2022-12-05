using UnityEngine;

public class Feather : Item
{
    [SerializeField] private float _speedDecrement;
    [SerializeField] private float _duration;

    public float Duration => _duration;
    public float SpeedDecrement => _speedDecrement;
}