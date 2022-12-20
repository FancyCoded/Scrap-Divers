using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text _coveredDistance;
    [SerializeField] private Portal _portal;

    private Score _score;

    private void OnEnable()
    {
        _portal.PortalReached += OnLevelFinished;
    }

    private void OnDisable()
    {
        _portal.PortalReached -= OnLevelFinished;
    }

    private void FixedUpdate()
    {
        if (_score != null && _score.ShouldRecord)
            _coveredDistance.text = _score.Distance.ToString("F0") + 'm';
    }

    public void Init(Score score)
    {
        _score = score;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Display()
    {
        gameObject.SetActive(true);
    }

    private void OnLevelFinished()
    {
        _coveredDistance.text = _score.Distance.ToString("F0") + 'm';
    }
}
