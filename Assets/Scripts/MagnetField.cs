using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MagnetField : MonoBehaviour
{
    [SerializeField] private Transform _robot;

    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private IEnumerator _magnetActivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Nut nut))
            nut.Movement.Init(_robot);
    }
     
    public void ActivateFor(float duration)
    {
        if (_magnetActivate != null)
            StopCoroutine(_magnetActivate);

        _magnetActivate = MagnetActivate(duration);
        StartCoroutine(_magnetActivate);
    }

    private IEnumerator MagnetActivate(float duration)
    {
        WaitForSeconds seconds = new WaitForSeconds(duration);

        _collider.enabled = true;
        yield return seconds;
        _collider.enabled = false;
    }
}
