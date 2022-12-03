using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WarpEffect : MonoBehaviour
{
    [SerializeField] private float _duration;

    public event UnityAction Disabled;
    public event UnityAction Entered;

    public void Enable()
    {
        gameObject.SetActive(true);
        StartCoroutine(ActivatеFor(_duration));
    }

    private IEnumerator ActivatеFor(float duration)
    {
        WaitForSeconds seconds = new WaitForSeconds(duration);

        Entered?.Invoke();

        yield return seconds;

        Disabled?.Invoke();
        gameObject.SetActive(false);
    }
}