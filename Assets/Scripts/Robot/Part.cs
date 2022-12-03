using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Part : MonoBehaviour, IDamageable, IRepairable
{
    [SerializeField] private float _health;
    [SerializeField] private Transform _mesh;
    [SerializeField] private PartType _partType;
    [SerializeField] private ParticleSystem _brokenEffect;
    [SerializeField] private ParticleSystem _dustEffect;
    [SerializeField] private float _colliderCastDistance = 0.1f;
    [SerializeField] private AudioClip _hitSound;

    private Collider _collider;
    private Rigidbody _rigidbody;
    private Rigidbody _rigidbodyStartState;
    private RaycastHit _hit;
    private float _maxHealth;
    private Obstacle _lastCollidedObstacle;

    public float Health => _health;
    public float MaxHealth => _maxHealth;
    public PartType PartType => _partType;
    public AudioClip HitSound => _hitSound;
    public Rigidbody Rigidbody => _rigidbody;

    public event UnityAction<Part> Destructed;
    public event UnityAction<Part> Damaged;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbodyStartState = _rigidbody;
        _maxHealth = _health;
    }

    private void FixedUpdate()
    {
        CheckObstacle();

        if (_hit.collider)
            TryGetDamage(_hit);
    }

    public void ApplyDamage(float damage)
    {
        if (damage <= 0)
            throw new ArgumentOutOfRangeException(); 

        _health -= damage;
        Damaged?.Invoke(this);

        _dustEffect.Play();

        float halfHealth = _maxHealth / 2;

        if (_health <= halfHealth)
            _brokenEffect.Play();

        if (_health <= 0)
            Destruct();
    }

    public void Repair()
    {
        _health = _maxHealth;
        _mesh.gameObject.SetActive(true);
        enabled = true;

        if (_brokenEffect)
            _brokenEffect.Stop();

        _rigidbody = _rigidbodyStartState;
    }

    public virtual void Destruct()
    {
        Destructed?.Invoke(this);
        _mesh.gameObject.SetActive(false);
        enabled = false;

        if (_brokenEffect)
            _brokenEffect.Stop();
    }

    private void CheckObstacle()
    {
        if (_collider is CapsuleCollider capsule)
        {
            Vector3 point1 = transform.position + transform.up * capsule.radius;
            Vector3 point2 = transform.position - transform.up * capsule.radius + (transform.up * capsule.height);

            Physics.CapsuleCast(point1, point2, capsule.radius, Vector3.forward, out _hit, _colliderCastDistance);
        }

        if (_collider is BoxCollider box)
        {
            Vector3 boxHalfSize = box.bounds.size / 2;

            Physics.BoxCast(transform.position, boxHalfSize, Vector3.forward, out _hit, Quaternion.identity, _colliderCastDistance);
        }
    }

    private void TryGetDamage(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent(out Obstacle obstacle))
        {
            if (_lastCollidedObstacle == obstacle)
                return;

            ApplyDamage(obstacle.Damage);
            _lastCollidedObstacle = obstacle;
        }
    }
}