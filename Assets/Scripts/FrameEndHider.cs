using UnityEngine;

public class FrameEndHider : MonoBehaviour
{
    public void Enable() => gameObject.SetActive(true);

    public void Disable() => gameObject.SetActive(false);
}
