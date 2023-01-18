using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _fpsText;
    [SerializeField] private float _delay;

    private float _elapsedTime = 0;
    private uint _frameCount = 0;
    private int _frameRate = 0;

    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        _frameCount++;

        if (_elapsedTime < _delay)
            return;

        _frameRate = Mathf.RoundToInt(_frameCount / _elapsedTime);
        _fpsText.text = $"{_frameRate} - fps";
        _frameCount = 0;
        _elapsedTime -= _delay;
    }
}