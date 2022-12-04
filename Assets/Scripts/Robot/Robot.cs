using UnityEngine;
using UnityEngine.Events;

public class Robot : MonoBehaviour
{
    [SerializeField] private Body _body;
    [SerializeField] private RagdollScaler _ragdollScaler;
    [SerializeField] private RobotMovement _robotMovement;

    public Body Body => _body;

    public event UnityAction Revived;

    private void OnEnable()
    {
        _body.Died += OnDied;
    }

    private void OnDisable()
    {
        _body.Died -= OnDied;
    }

    private void OnDied()
    {
        gameObject.SetActive(false);
    }

    public void Revive()
    {
        int positionOffsetZ = 2;

        _body.Repair();
        _ragdollScaler.CorrectCharacterJoints();
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + positionOffsetZ);
        gameObject.SetActive(true);
        StartCoroutine(_robotMovement.SetSpeedSmooth());
        Revived?.Invoke();
    }
}