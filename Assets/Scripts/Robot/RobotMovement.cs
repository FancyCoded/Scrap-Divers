using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RobotMovement : MonoBehaviour, IResetable
{
    private const float DefaultSpeed = 30;
    private const float DefaultVelocitySpeed = 5;
    private const int MaxVelocityMagnitude = 10;

    [SerializeField] private float _defaultSpeed = DefaultSpeed;
    [SerializeField] private float _defaultVelocitySpeed = DefaultVelocitySpeed;
    [SerializeField] private float _speed;
    [SerializeField] private float _velocitySpeed;
    [SerializeField] private float _lerpMaxDelta;
    [SerializeField] private float _rayCastDistance;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Portal _portal;
    [SerializeField] private Level _level;
    [SerializeField] private ParticleSystem _featherEffect;
    [SerializeField] private int _spawnOffsetZ = 10;

    private Vector3 _targetPosition;
    private Vector2 _horizontalPositionRange = new Vector2(-3.3f, -0.7f);
    private Vector2 _verticalPositionRange = new Vector2(-1.5f, 1.4f);
    private Vector2 _startPosition;
    private IEnumerator _reduceSpeed;

    private bool _canMove = true;
    private bool _recordVelocity = true;

    private LevelProperties _levelProperites;
    private PlayerInputRouter _input;

    public float Speed => _speed;

    public event UnityAction Stopped;
    public event UnityAction<float> SpeedChanged;

    private void Awake()
    {
        _input = new PlayerInputRouter();
    }

    private void OnEnable()
    {
        _portal.PortalReached += OnPortalReached;
        _input.Enable();
    }

    private void Start()
    {
        StartCoroutine(SetDefaultSpeedSmooth());
    }

    private void FixedUpdate()
    {
        if (_canMove == false)
            return;

        _input.Update();

        Move();

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _rayCastDistance))
        {
            if (hit.collider.TryGetComponent(out Portal portal))
                _recordVelocity = false;
        }
    }

    public void ReduceSpeedFor(float duration, float speedDecrement)
    {
        if (_reduceSpeed != null)
            StopCoroutine(_reduceSpeed);

        _reduceSpeed = ReduceSpeed(duration, speedDecrement);
        StartCoroutine(_reduceSpeed);
    }

    public void Init(LevelProperties levelProperites, uint startPositionZ, PlayerInputRouter input)
    {
        ResetState();

        _input = input;
        _levelProperites = levelProperites;
        transform.position = new Vector3(transform.position.x, transform.position.y, startPositionZ + _spawnOffsetZ);
        _startPosition = _rigidbody.transform.position;
    }

    public void ResetState()
    {
        _canMove = true;
        _recordVelocity = true;

        _levelProperites = null;
        _input = null;
    }

    public void ChangeSpeed()
    {
        float speedIncrement = 0.5f;
        _speed += speedIncrement;

        SpeedChanged?.Invoke(_speed);
    }

    public IEnumerator SetDefaultSpeedSmooth()
    {
        _velocitySpeed = _defaultVelocitySpeed;

        if (_reduceSpeed != null)
            yield break;

        _speed = 0;

        while (_speed != _defaultSpeed)
        {
            _speed = Mathf.MoveTowards(_speed, _defaultSpeed, _lerpMaxDelta);
            SpeedChanged?.Invoke(_speed);
            yield return null;
        }
    }

    private void OnPortalReached()
    {
        _canMove = false;
        Stopped?.Invoke();

        _rigidbody.transform.position = new Vector3(_startPosition.x, _startPosition.y, _levelProperites.CoveredLevelsLength);
    }

    private void Move()
    {
        _targetPosition = transform.position + Vector3.forward;
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

        if (_recordVelocity == false)
            return;

        _rigidbody.velocity = new Vector3(
        Mathf.Clamp(_input.Movement.x * _velocitySpeed, -MaxVelocityMagnitude, MaxVelocityMagnitude),
        Mathf.Clamp(_input.Movement.y * _velocitySpeed, -MaxVelocityMagnitude, MaxVelocityMagnitude),
        _rigidbody.velocity.z);

        _rigidbody.transform.position = new Vector3(
            Mathf.Clamp(_rigidbody.transform.position.x, _horizontalPositionRange.x, _horizontalPositionRange.y),
            Mathf.Clamp(_rigidbody.transform.position.y, _verticalPositionRange.x, _verticalPositionRange.y),
            transform.position.z);
    }

    private IEnumerator ReduceSpeed(float duration, float speedDecrement)
    {
        WaitForSeconds seconds = new WaitForSeconds(duration);
        float targetSpeed = _speed - speedDecrement;

        _featherEffect.Play();

        while (_speed != targetSpeed)
        {
            _speed = Mathf.MoveTowards(_speed, targetSpeed, _lerpMaxDelta);
            yield return new WaitForEndOfFrame();
        }

        SpeedChanged?.Invoke(_speed);

        yield return seconds;

        _featherEffect.Stop();

        targetSpeed = _speed + speedDecrement;

        while (_speed != targetSpeed)
        {
            _speed = Mathf.MoveTowards(_speed, targetSpeed, _lerpMaxDelta);
            yield return null;
        }

        SpeedChanged?.Invoke(_speed);
        _reduceSpeed = null;
    }
}