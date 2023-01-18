using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider), typeof(AudioSource))]
public class Collector : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private BuffApplier _buffApplyer;
    [SerializeField] private float _boxCastDistance;
    [SerializeField] private AudioClip _coinSound;
    [SerializeField] private AudioClip _buffSound;
    [SerializeField] private ItemSpawner _itemSpawner;

    private BoxCollider _boxCollider;
    private AudioSource _audioSource;
    private RaycastHit _hit;

    public uint NutCount { get; private set; } = 0;
    public uint StarCount { get; private set; } = 0;
    public uint FeatherCount { get; private set; } = 0;
    public uint WrenchCount { get; private set; } = 0;
    public uint MagnetCount { get; private set; } = 0;


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
            NutCount++;
            NutCountChanged?.Invoke(NutCount);
        }
        else
        {
            if (item is Feather) FeatherCount++;
            if (item is Wrench) WrenchCount++;
            if (item is Star) StarCount++;
            if (item is Magnet) MagnetCount++;

            _audioSource.PlayOneShot(_buffSound);
            _buffApplyer.Apply(item);
        }

        _itemSpawner.Release(item);
    }
}