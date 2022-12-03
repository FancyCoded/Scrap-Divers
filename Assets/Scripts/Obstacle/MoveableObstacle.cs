using UnityEngine;
using System.Collections;

public class MoveableObstacle : MonoBehaviour, IResetable, IMoveable
{
    [SerializeField] private float _cooldown;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _targetPosition;

    private Vector3 _startPosition;
    private IEnumerator _move;

    private void Awake()
    {
        _startPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        Move();
    }

    private void OnDisable()
    {
        ResetState();
    }

    public void Move()
    {
        _move = MoveTo(_targetPosition);
        StartCoroutine(_move);
    }
    
    public void ResetState()
    {
        if(_move != null)
            StopCoroutine(_move);

        transform.localPosition = _startPosition;
    }

    private IEnumerator MoveTo(Vector3 targetPosition)
    {
        WaitForSeconds cooldown = new WaitForSeconds(_cooldown);
        float maxDelta = _speed * Time.deltaTime;

        while (true)
        {
            while (transform.localPosition != targetPosition)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, maxDelta);
                yield return null;
            }

            yield return cooldown;

            while(transform.localPosition != _startPosition)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _startPosition, maxDelta);
                yield return null;
            }

            yield return cooldown;
        }
    }
}
