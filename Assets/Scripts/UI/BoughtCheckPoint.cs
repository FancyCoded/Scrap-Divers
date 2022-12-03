using UnityEngine;
using UnityEngine.UI;

public class BoughtCheckPoint : CheckPoint
{
    [SerializeField] private Image _checkImage;
    [SerializeField] private Button _checkButton;
    [SerializeField] private Color _checkedColor;
    [SerializeField] private Color _uncheckedColor;

    private void OnEnable()
    {
        _checkButton.onClick.AddListener(OnCheckButtonClicked);
    }

    private void OnDisable()
    {
        _checkButton.onClick.RemoveListener(OnCheckButtonClicked);
    }

    public override void Init(CheckPointMapView checkPointMapView, IReadonlyCheckPointProperty checkPointProperty)
    {
        base.Init(checkPointMapView, checkPointProperty);

        if (Properties.IsChecked)
            SetChecked();
        else
            UnSetChecked();
    }

    public void SetChecked()
    {
        _checkButton.image.color = _checkedColor;
        _checkImage.gameObject.SetActive(true);
    }

    public void UnSetChecked()
    {
        _checkButton.image.color = _uncheckedColor;
        _checkImage.gameObject.SetActive(false);
    }

    private void OnCheckButtonClicked()
    {
        CheckPointMapView.ClickPointCheckButton(Properties);
    }
}
