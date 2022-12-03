using UnityEngine;

public class MoveableObstacleTrigger : MonoBehaviour
{
    [SerializeField] private Obstacle[] _obstacles;

    public void TargetEnter()
    {
        for (int i = 0; i < _obstacles.Length; i++)
            if (_obstacles[i].TryGetComponent(out IMoveable moveable))
                moveable.Move();
    }
}