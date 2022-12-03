using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WhiteDisplay : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Portal _portal;

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
        StartCoroutine(EnableFor(1));
    }

    private IEnumerator EnableFor(float duration)
    {
        WaitForSeconds seconds = new WaitForSeconds(duration);

        _image.gameObject.SetActive(true);

        yield return seconds;

        _image.gameObject.SetActive(false);
    }
}
