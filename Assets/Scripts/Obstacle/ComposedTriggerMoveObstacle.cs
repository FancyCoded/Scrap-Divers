using System.Collections;
using UnityEngine;

public class ComposedTriggerMoveObstacle : MonoBehaviour, IMoveable, IResetable
{
    [SerializeField] private TriggerMoveObstacle[] _triggerMoveObstacles;
    [SerializeField] private float _cooldownPerObstacle;

    private IEnumerator _move;

    public void Move()
    {
        _move = MoveObstacles();
        StartCoroutine(_move);
    }

    public void ResetState()
    {
        for (int i = 0; i < _triggerMoveObstacles.Length; i++)
            _triggerMoveObstacles[i].ResetState();
    }

    private IEnumerator MoveObstacles()
    {
        WaitForSeconds cooldown = new WaitForSeconds(_cooldownPerObstacle);

        for (int i = 0; i < _triggerMoveObstacles.Length; i++)
        {
            _triggerMoveObstacles[i].Move();

            if (i % 2 != 0)
                yield return cooldown;
        }
    }
}