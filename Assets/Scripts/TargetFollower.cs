using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TargetFollower : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private Vector3 _positionOffset;
    private Vector3 _desiredPosition;

    private void Start() =>
        _positionOffset = transform.position - _target.transform.position;

    private void LateUpdate()
    {
        if (_target == null)
            return;

        _desiredPosition = _target.transform.position + _positionOffset;
        transform.position = _desiredPosition;
    }
}
