using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class CheckPointMapView : MonoBehaviour, IResetable
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private TradedCheckPoint _tradedCheckPointTemplate;
    [SerializeField] private BoughtCheckPoint _boughtCheckPointTemplate;
    [SerializeField] private Button _back;

    private readonly Dictionary<uint, BoughtCheckPoint> _boughtCheckPoints = new Dictionary<uint, BoughtCheckPoint>();

    private CanvasGroup _canvasGroup;

    public event UnityAction<IReadonlyCheckPointProperty> PointCheckButtonClicked;
    public event UnityAction<IReadonlyCheckPointProperty> PointSellButtonClicked;

    private void OnEnable()
    {
        _back.onClick.AddListener(Hide);
    }

    private void OnDisable()
    {
        _back.onClick.RemoveListener(Hide);
    }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        Hide();
    }

    public void Display()
    {
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1;
    }

    public void Hide()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0;
    }

    public void ClickPointCheckButton(IReadonlyCheckPointProperty checkPointProperty)
    {
        PointCheckButtonClicked?.Invoke(checkPointProperty);
    }

    public void ClickPointSellButton(IReadonlyCheckPointProperty checkPointProperty)
    {
        PointSellButtonClicked?.Invoke(checkPointProperty);
    }

    public void OnPointCheckPropertyChanged(IReadonlyCheckPointProperty checkPointProperty)
    {
        if (_boughtCheckPoints[checkPointProperty.Distance].Properties.IsChecked)
            _boughtCheckPoints[checkPointProperty.Distance].SetChecked();
        else
            _boughtCheckPoints[checkPointProperty.Distance].UnSetChecked();
    }

    public void OnUpdated(IReadOnlyList<IReadonlyCheckPointProperty> checkPointProperties)
    {
        ResetState();

        for (int i = 0; i < checkPointProperties.Count; i++)
            CreatePoint(checkPointProperties[i]);
    }

    public void ResetState()
    {
        for (int i = 0; i < _content.childCount; i++)
            Destroy(_content.GetChild(i).gameObject);

        _boughtCheckPoints.Clear();
    }

    private void CreatePoint(IReadonlyCheckPointProperty checkPointProperty)
    {
        CheckPoint checkPoint;

        if (checkPointProperty.IsBought)
        {
            checkPoint = Instantiate(_boughtCheckPointTemplate, _content);
            _boughtCheckPoints.Add(checkPointProperty.Distance, (BoughtCheckPoint)checkPoint);
        }
        else
            checkPoint = Instantiate(_tradedCheckPointTemplate, _content);

        checkPoint.Init(this, checkPointProperty);
    }
}