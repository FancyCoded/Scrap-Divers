using UnityEngine;

public class BuffApplier : MonoBehaviour
{
    [SerializeField] private Body _body;
    [SerializeField] private MagnetField _magnetField;
    [SerializeField] private RagdollScaler _ragdollScaler;
    [SerializeField] private RobotMovement _robotMovement;

    private ItemVisitor _itemVisitor;

    private void Awake()
    {
        _itemVisitor = new ItemVisitor(_body, _magnetField, _ragdollScaler, _robotMovement);
    }

    public void Apply(Item item) => _itemVisitor.Visit(item);

    public void ResetState()
    {
        _body.Repair();
        _ragdollScaler.ResetState();
        _magnetField.ResetState();
    }

    private class ItemVisitor : IItemVisitor
    {
        private readonly Body _body;
        private readonly MagnetField _magnetField;
        private readonly RagdollScaler _ragdollScaler;
        private readonly RobotMovement _robotMovement;

        public ItemVisitor(Body body, 
            MagnetField magnetField,
            RagdollScaler ragdollScaler,
            RobotMovement robotMovement)
        {
            _body = body; 
            _magnetField = magnetField;
            _ragdollScaler = ragdollScaler;
            _robotMovement = robotMovement;
        }

        public void Visit(Item item, ItemVisitParams visitParams = null) => Visit((dynamic)item, visitParams);

        public void Visit(Wrench wrench, ItemVisitParams visitParams) => _body.Repair();

        public void Visit(Magnet magnet, ItemVisitParams visitParams) =>
             _magnetField.ActivateFor(magnet.Duration);

        public void Visit(Star star, ItemVisitParams visitParams) =>
            _ragdollScaler.ChangeScaleFor(star.Duration, star.TargetScale);

        public void Visit(Feather feather, ItemVisitParams visitParams) =>
            _robotMovement.ReduceSpeedFor(feather.Duration, feather.SpeedDecrement);

        public void Visit(Nut nut, ItemVisitParams visitParams) { }
    }
}
