using System.Collections.Generic;
using UnityEngine;

public class MenuStorageComposition : StorageComposition, IResetable
{
    [SerializeField] private List<CheckPointProperty> _checkPointPropertiesDefault;
    [SerializeField] private CheckPointMapView _checkPointMapView;
    [SerializeField] private WalletView _walletView;
    [SerializeField] private GeneralAudioActivityToggler _generalAudioActivityToggler;

    private readonly Wallet _wallet = new Wallet(0);
    private readonly CheckPointMap _checkPointMap = new CheckPointMap();
    private readonly Score _score = new Score();

    private void OnEnable()
    {
        _checkPointMap.Inited += _checkPointMapView.OnUpdated;
        _checkPointMap.PointCheckPropertyChanged += _checkPointMapView.OnPointCheckPropertyChanged;
        _checkPointMap.PointSold += _checkPointMapView.OnUpdated;

        _checkPointMapView.PointCheckButtonClicked += _checkPointMap.OnPointChecked;
        _checkPointMapView.PointSellButtonClicked += _checkPointMap.OnPointSold;

        _wallet.NutCountChanged += _walletView.OnNutCountChanged;
    }

    private void OnDisable()
    {
        _checkPointMap.Inited -= _checkPointMapView.OnUpdated;
        _checkPointMap.PointCheckPropertyChanged -= _checkPointMapView.OnPointCheckPropertyChanged;
        _checkPointMap.PointSold -= _checkPointMapView.OnUpdated;

        _checkPointMapView.PointCheckButtonClicked -= _checkPointMap.OnPointChecked;
        _checkPointMapView.PointSellButtonClicked -= _checkPointMap.OnPointSold;

        _wallet.NutCountChanged -= _walletView.OnNutCountChanged;
    }

    private void Start()
    {
        Compose();
    }

    public override void Compose()
    {
        Storage.Init(_wallet, _checkPointMap, _score, _checkPointPropertiesDefault, _generalAudioActivityToggler);
        Storage.Load();
    }

    [ContextMenu("Reset")]
    public void ResetState()
    {
        _checkPointMapView.ResetState();
        _checkPointMap.ResetState();
        Storage.ResetState();
    }

    [ContextMenu("Reset Load")]
    public void ResetLoad()
    {
        ResetState();
        Storage.Load();
    }

    [ContextMenu("Save")]
    public void Save()
    {
        Storage.Save();
    }

    [ContextMenu("TestMode")]
    public void TestMode()
    {
        ResetState();
        Storage.TestMode();
        Storage.Load();
    }
}