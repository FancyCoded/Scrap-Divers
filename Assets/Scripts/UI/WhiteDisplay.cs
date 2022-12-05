using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WhiteDisplay : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Portal _portal;
    [SerializeField] private float _activeDuration = 0.5f;

    private void OnEnable()
    {
        _portal.WarpEffect.Entered += SetActive;
        _portal.WarpEffect.Disabled += SetActive;
    }

    private void OnDisable()
    {
        _portal.WarpEffect.Entered -= SetActive;
        _portal.WarpEffect.Disabled -= SetActive;

    }

    private void SetActive()
    {
        StartCoroutine(EnableFor(_activeDuration));
    }

    private IEnumerator EnableFor(float duration)
    {
        WaitForSeconds seconds = new WaitForSeconds(duration);

        _image.gameObject.SetActive(true);

        yield return seconds;

        _image.gameObject.SetActive(false);
    }
}
