using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class RobotMovement : MonoBehaviour, IResetable
{
    private const string Vertical = "Vertical";
    private const string Horizontal = "Horizontal";

    [SerializeField] private float _maxSpeed = 35;
    [SerializeField] private float _velocitySpeed;
    [SerializeField] private float _speed;
    [SerializeField] private float _lerpMaxDelta;
    [SerializeField] private float _rayCastDistance;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Portal _portal;
    [SerializeField] private Level _level;

    private Vector3 _targetPosition;
    private Vector2 _horizontalPositionRange = new Vector2(-3.5f, -0.5f);
    private Vector2 _verticalPositionRange = new Vector2(-1.5f, 1.5f);
    private Vector2 _startPosition;
    private IEnumerator _changeSpeed;

    private bool _canMove = true;
    private bool _recordVelocity = true;

    private float _vertical;
    private float _horizontal;
    private LevelProperites _levelProperites;

    public float Speed => _speed;

    public event UnityAction Stopped;
    public event UnityAction<float> SpeedChanged;

    private void OnEnable()
    {
        _portal.PortalReached += OnPortalReached;
    }

    private void Start()
    {
        StartCoroutine(SetSpeedSmooth());
    }

    private void FixedUpdate()
    {
        if (_canMove == false)
            return;

        _rigidbody.velocity = Vector3.zero;

        Move();

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _rayCastDistance))
        {
            if (hit.collider.TryGetComponent(out Portal portal))
                _recordVelocity = false;
        }
    }

    public void ChangeSpeedFor(float duration, float speed)
    {
        if (_changeSpeed != null)
            StopCoroutine(_changeSpeed);

        _changeSpeed = SetSpeedFor(duration, speed);
        StartCoroutine(_changeSpeed);
    }

    public void Init(LevelProperites levelProperites, uint startPositionZ)
    {
        ResetState();

        _levelProperites = levelProperites;
        transform.position = new Vector3(transform.position.x, transform.position.y, startPositionZ);
        _startPosition = _rigidbody.transform.position;
    }

    public void ResetState()
    {
        _canMove = true;
        _recordVelocity = true;

        _levelProperites = null;
    }

    private void OnPortalReached()
    {
        _canMove = false;
        Stopped?.Invoke();

        _rigidbody.transform.position = new Vector3(_startPosition.x, _startPosition.y, _levelProperites.LevelLength);
    }

    private void Move()
    {
        _vertical = Input.GetAxis(Vertical);
        _horizontal = Input.GetAxis(Horizontal);

        _targetPosition = transform.position + Vector3.forward;
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

        if (_recordVelocity == false)
            return;

        _rigidbody.velocity = new Vector3(_horizontal * _velocitySpeed, _vertical * _velocitySpeed);
        _rigidbody.transform.position = new Vector3(
            Mathf.Clamp(_rigidbody.transform.position.x, _horizontalPositionRange.x, _horizontalPositionRange.y),
            Mathf.Clamp(_rigidbody.transform.position.y, _verticalPositionRange.x, _verticalPositionRange.y), 
            transform.position.z);
    }

    private IEnumerator SetSpeedSmooth()
    {
        _speed = 0;

        while (_speed != _maxSpeed)
        {
            _speed = Mathf.MoveTowards(_speed, _maxSpeed, _lerpMaxDelta);
            SpeedChanged?.Invoke(_speed);
            yield return null;
        }
    }

    private IEnumerator SetSpeedFor(float duration, float targetSpeed)
    {
        float lastSpeed = _speed;
        WaitForSeconds seconds = new WaitForSeconds(duration);

        while(_speed != targetSpeed)
        {
            _speed = Mathf.MoveTowards(_speed, targetSpeed, _lerpMaxDelta);
            yield return null;
        }

        SpeedChanged?.Invoke(_speed);

        yield return seconds;

        while (_speed != lastSpeed)
        {
            _speed = Mathf.MoveTowards(_speed, lastSpeed, _lerpMaxDelta);
            yield return null;
        }

        SpeedChanged?.Invoke(_speed);
    }
}