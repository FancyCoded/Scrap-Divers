using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    [SerializeField] private int _targetFPS;

    private void Start()
    {
        Application.targetFrameRate= _targetFPS;
    }
}