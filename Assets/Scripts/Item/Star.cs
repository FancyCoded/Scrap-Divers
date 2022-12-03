using UnityEngine;

public class Star : Item
{
    [SerializeField] private float _targetScale;
    [SerializeField] private float _duration;

    public float TargetScale => _targetScale;
    public float Duration => _duration;
}
