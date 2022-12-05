using UnityEngine;

public class NutMovement : MonoBehaviour
{
    [SerializeField] private float _extraSpeedFactor = 1.2f;

    private Transform _target;
    private float _speed;

    private void FixedUpdate()
    {
        if (_target == null)
            enabled = false;

        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
    }

    public void Init(Transform target, RobotMovement robotMovement)
    {
        _target = target;
        _speed = robotMovement.Speed * _extraSpeedFactor;
        enabled = true;
    }

    public void Disable()
    {
        enabled = false;
        _target = null;
    }
}