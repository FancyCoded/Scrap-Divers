using UnityEngine;

public class PortalFinder : MonoBehaviour
{
    [SerializeField] private float _rayCastDistance;

    private RaycastHit _hit;

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.forward, out _hit, _rayCastDistance))
        {
            if (_hit.collider.TryGetComponent(out Portal portal))
            {
                portal.Enter();
                enabled = false;
            }
        }
    }
}