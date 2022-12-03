using System.Collections.Generic;
using UnityEngine;

public class MenuStorageComposition : MonoBehaviour, IResetable
{
    [SerializeField] private List<CheckPointProperty> _checkPointPropertiesDefault;
    [SerializeField] private CheckPointMapView _checkPointMapView;
    [SerializeField] private WalletView _walletView;

    private readonly Storage _storage = new Storage();
    private readonly Wallet _wallet = new Wallet(0);
    private readonly CheckPointMap _checkPointMap = new CheckPointMap();
    private readonly Score _score = new Score();

    public Storage Storage => _storage;

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
        _storage.Init(_wallet, _checkPointMap, _score, _checkPointPropertiesDefault);
        _storage.Load();
        Save();
    }

    [ContextMenu("Reset")]
    public void ResetState()
    {
        _checkPointMapView.ResetState();
        _checkPointMap.ResetState();
        _storage.ResetState();
    }

    [ContextMenu("Reset Load")]
    public void ResetLoad()
    {
        _checkPointMapView.ResetState();
        _checkPointMap.ResetState();
        _storage.ResetState();
        _storage.Load();
    }

    [ContextMenu("Save")]
    public void Save()
    {
        _storage.Save();
    }

    [ContextMenu("TestMode")]
    public void TestMode()
    {
        _storage.TestMode();
    }
}