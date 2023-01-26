using UnityEngine;
using UnityEngine.UI;

public class BoughtCheckPoint : CheckPoint
{
    [SerializeField] private Image _checkImage;
    [SerializeField] private Button _checkButton;
    [SerializeField] private Sprite _checkedSprite;
    [SerializeField] private Sprite _uncheckedSprite;

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
        _checkButton.image.sprite = _checkedSprite;
        _checkImage.gameObject.SetActive(true);
    }

    public void UnSetChecked()
    {
        _checkButton.image.sprite = _uncheckedSprite;
        _checkImage.gameObject.SetActive(false);
    }

    private void OnCheckButtonClicked()
    {
        CheckPointMapView.ClickPointCheckButton(Properties);
    }
}
