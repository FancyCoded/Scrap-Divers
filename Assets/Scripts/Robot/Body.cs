using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Body : MonoBehaviour, IRepairable
{
    [SerializeField] private List<Part> _parts;
    [SerializeField] private ParticleSystem _electricEffect;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private RobotMovement _robotMovement;

    public event UnityAction Died;

    private void Awake()
    {
        _robotMovement.Stopped += OnRobotStopped;

        for(int i = 0; i < _parts.Count; i++)
        {
            _parts[i].Damaged += OnPartDamaged;
            _parts[i].Destructed += OnPartDestructed;
        }
    }
    
    public void Repair()
    {
        for (int i = 0; i < _parts.Count; i++)
        {
            _parts[i].Repair();
            _parts[i].Destructed += OnPartDestructed;
            _parts[i].Damaged += OnPartDamaged;
        }
    }

    private void OnRobotStopped()
    {
        for (int i = 0; i < _parts.Count; i++)
        {
            _parts[i].Rigidbody.velocity = Vector3.zero;
            _parts[i].Rigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void Die()
    {
        for (int i = 0; i < _parts.Count; i++)
        {
            _parts[i].Destructed -= OnPartDestructed;
            _parts[i].Damaged -= OnPartDamaged;
        }

        Died?.Invoke();
    }

    private void OnPartDamaged(Part part)
    {
        _audioSource.PlayOneShot(part.HitSound);
    }

    private void OnPartDestructed(Part part)
    {
        if (part.PartType == PartType.Trunk)
            Die();
    }
}
