using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider), typeof(AudioSource))]
public class Collector : MonoBehaviour
{
    [SerializeField] private uint _nutCount = 0;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private BuffApplier _buffApplyer;
    [SerializeField] private float _boxCastDistance;
    [SerializeField] private AudioClip _coinSound;
    [SerializeField] private AudioClip _buffSound;
    [SerializeField] private ItemSpawner _itemSpawner;

    private BoxCollider _boxCollider;
    private AudioSource _audioSource;
    private RaycastHit _hit;

    public uint NutCount => _nutCount;

    public event UnityAction<uint> NutCountChanged;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        transform.position = _rigidbody.transform.position;
        Vector3 boxHalfSize = _boxCollider.size / 2;

        if (Physics.BoxCast(_boxCollider.bounds.center, boxHalfSize, Vector3.forward, out _hit, Quaternion.identity, _boxCastDistance))
            if (_hit.collider.TryGetComponent(out Item item))
                Collect(item);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Item item))
            Collect(item);
    }

    private void Collect(Item item)
    {
        if (item is Nut)
        {
            _audioSource.PlayOneShot(_coinSound);
            _nutCount += 1;
            NutCountChanged?.Invoke(_nutCount);
        }
        else
        {
            _audioSource.PlayOneShot(_buffSound);
            _buffApplyer.Apply(item);
        }

        _itemSpawner.Release(item);
    }
}