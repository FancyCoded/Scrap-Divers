using UnityEngine;

public class BuffApplier : MonoBehaviour
{
    [SerializeField] private Body _body;
    [SerializeField] private MagnetField _magnetField;
    [SerializeField] private RagdollScaler _ragdollScaler;
    [SerializeField] private RobotMovement _robotMovement;

    public void Apply(Item item)
    {
        if (item is Magnet magnet)
            _magnetField.ActivateFor(magnet.Duration);
        if (item is Star star)
            _ragdollScaler.ChangeScaleFor(star.Duration, star.TargetScale);
        if (item is Wrench wrench)
            _body.Repair();
        if (item is Feather feather)
            _robotMovement.ReduceSpeedFor(feather.Duration, feather.SpeedDecrement);
    }

    public void ResetState()
    {
        _body.Repair();
        _ragdollScaler.ResetState();
        _magnetField.ResetState();
    }
}
