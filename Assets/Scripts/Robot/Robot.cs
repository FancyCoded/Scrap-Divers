using UnityEngine;
using UnityEngine.Events;

public class Robot : MonoBehaviour
{
    [SerializeField] private Body _body;

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
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + positionOffsetZ);
        gameObject.SetActive(true);
        Revived?.Invoke();
    }
}