using System.Collections;
using UnityEngine;

public class TriggerMoveObstacle : MonoBehaviour, IResetable, IMoveable
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _targetPosition;

    private Vector3 _startPosition;
    private IEnumerator _move;

    private void Awake()
    {
        _startPosition = transform.localPosition;
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
        if (_move != null)
            StopCoroutine(_move);

        transform.localPosition = new Vector3(_startPosition.x, _startPosition.y, transform.localPosition.z);
    }

    private IEnumerator MoveTo(Vector3 targetPosition)
    {
        float maxDelta = _speed * Time.deltaTime;

        while (transform.localPosition != targetPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, maxDelta);
            yield return null;
        }
    }
}
