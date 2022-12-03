using UnityEngine;

public class ParentBody : Part
{
    [SerializeField] private Part _childPart;
    [SerializeField] private ParticleSystem _electricEffect;

    private void OnEnable()
    {
        _childPart.Destructed += OnChildDestructed;
    }

    private void OnDisable()
    {
        _childPart.Destructed -= OnChildDestructed;
    }

    private void OnChildDestructed(Part part)
    {
        _electricEffect.Play();
    }

    public override void Destruct()
    {
        base.Destruct();
        _childPart.Destruct();
        _electricEffect.Stop();
    }
}
