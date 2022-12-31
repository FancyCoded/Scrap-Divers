using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradedCheckPoint : CheckPoint
{
    [SerializeField] private TMP_Text _price;
    [SerializeField] private Image _nut;
    [SerializeField] private Button _sellButton;

    private void OnEnable()
    {
        _sellButton.onClick.AddListener(OnSellButtonClicked);
    }

    private void OnDisable()
    {
        _sellButton.onClick.RemoveListener(OnSellButtonClicked);
    }

    public override void Init(CheckPointMapView checkPointsMapView, IReadonlyCheckPointProperty checkPointProperty)
    {
        base.Init(checkPointsMapView, checkPointProperty);
        _price.text = Properties.Price.ToString();
    }

    private void OnSellButtonClicked()
    {
        CheckPointMapView.ClickPointSellButton(Properties);
    }
}
