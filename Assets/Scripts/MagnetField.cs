using System.Collections;
using UnityEngine;

public class MagnetField : MonoBehaviour
{
    [SerializeField] private RobotMovement _robotMovement;
    [SerializeField] private Transform _target;
    [SerializeField] private float _boxCastDistance;
    [SerializeField] private ParticleSystem _magnetEffect;

    private Vector3 _boxHalfSize = new Vector3(4, 4, 1);
    private Vector2 _boxPositon = new Vector2(-2, 0);
    private Vector3 _boxCenter = Vector3.zero;
    private IEnumerator _magnetActivate;
    private bool _isActive = false;

    private void FixedUpdate()
    {
        if (_isActive == false)
            return;
        
        _boxCenter = new Vector3(_boxPositon.x, _boxPositon.y, transform.position.z);

        if (Physics.BoxCast(_boxCenter, _boxHalfSize, Vector3.forward, out RaycastHit _hit, Quaternion.identity, _boxCastDistance))
            if (_hit.collider.TryGetComponent(out Nut nut))
                nut.Movement.Init(_target, _robotMovement);
    }

    public void ActivateFor(float duration)
    {
        if (_magnetActivate != null)
            StopCoroutine(_magnetActivate);

        _magnetActivate = MagnetActivate(duration);
        StartCoroutine(_magnetActivate);
    }

    public void ResetState()
    {
        if (_magnetActivate != null)
            StopCoroutine(_magnetActivate);

        _isActive = false;
    }

    private IEnumerator MagnetActivate(float duration)
    {
        WaitForSeconds seconds = new WaitForSeconds(duration);

        _magnetEffect.Play();
        _isActive = true;

        yield return seconds;

        _isActive = false;
        _magnetEffect.Stop();
    }
}
