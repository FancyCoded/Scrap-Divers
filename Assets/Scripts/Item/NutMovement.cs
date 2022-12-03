using UnityEngine;

public class NutMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Transform _target;

    private void FixedUpdate()
    {
        if (_target == null)
            enabled = false;

        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
    }

    public void Init(Transform target)
    {
        _target = target;
        enabled = true;
    }
}