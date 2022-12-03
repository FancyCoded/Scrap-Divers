using UnityEngine;

public class Magnet : Item
{
    [SerializeField] private float _duration;

    public float Duration => _duration;
}
