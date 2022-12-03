using UnityEngine;
using UnityEngine.Events;

public class Portal : MonoBehaviour
{
    [SerializeField] private WarpEffect _warpEffect;
    [SerializeField] private Robot _robot;
    [SerializeField] private FrameSpawner _frameSpawner;
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private ParticleSystem _portalEffect;

    private Vector2 _defaultPosition = new Vector2(-2, 0);
    private Vector3 _targetPosition;

    public WarpEffect WarpEffect => _warpEffect;

    public event UnityAction PortalReached;

    public void Enter()
    {
        PortalReached?.Invoke();
        _warpEffect.Enable();
        _boxCollider.enabled = false;
        _portalEffect.gameObject.SetActive(false);
    }

    public void Init()
    {
        _targetPosition = new Vector3(_defaultPosition.x, _defaultPosition.y, _frameSpawner.LastFrameEndZ);
        transform.position = _targetPosition;

        _portalEffect.gameObject.SetActive(true);
        _boxCollider.enabled = true;
    }
}
