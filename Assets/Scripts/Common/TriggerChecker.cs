using UnityEngine;

public class TriggerChecker : MonoBehaviour
{
    [SerializeField] private Level _level;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out FrameSpawnTrigger frameSpawnTrigger))
            _level.FrameSpawnTriggerReached();

        if (other.TryGetComponent(out MoveableObstacleTrigger moveableObstacleTrigger))
            moveableObstacleTrigger.TargetEnter();
    }
}
