using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EmissionModule = UnityEngine.ParticleSystem.EmissionModule;

public class Body : MonoBehaviour, IRepairable
{
    [SerializeField] private List<Part> _parts;
    [SerializeField] private ParticleSystem _electricEffect;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private RobotMovement _robotMovement;
    [SerializeField] private ParticleSystem _blackFog;

    private EmissionModule _emission;
    private uint _destructedPartCount = 0;

    public event UnityAction Died;

    private void Awake()
    {
        _emission = _blackFog.emission;
        _emission.rateOverTime = 0;

        _robotMovement.Stopped += OnRobotStopped;

        for (int i = 0; i < _parts.Count; i++)
        {
            _parts[i].Damaged += OnPartDamaged;
            _parts[i].Destructed += OnPartDestructed;
        }        
    }
    
    public void Repair()
    {
        _emission.rateOverTime = 0;
        
        for (int i = 0; i < _parts.Count; i++)
        {
            _parts[i].Repair();
            _parts[i].Destructed += OnPartDestructed;
            _parts[i].Damaged += OnPartDamaged;
        }

        StartCoroutine(_robotMovement.SetDefaultSpeedSmooth());
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
        int emissionFactor = 10;

        if (part.PartType == PartType.Trunk)
            Die();

        _destructedPartCount++;
        _emission.rateOverTime = _destructedPartCount * emissionFactor;

        _robotMovement.IncreaseSpeedAndVelocity();
    }
}
