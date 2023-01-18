using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointMap : IResetable
{
    private Wallet _wallet;
    private Storage _storage;

    private List<CheckPointProperty> _checkPointProperties = new List<CheckPointProperty>();
    private List<CheckPointProperty> _availableCheckPointProperties = new List<CheckPointProperty>();
    private Dictionary<uint, CheckPointProperty> _checkPointPair = new Dictionary<uint, CheckPointProperty>();
    private CheckPointProperty _currentCheckPointProperty = null;

    public IReadOnlyList<IReadonlyCheckPointProperty> CheckPointProperties => _checkPointProperties;
    public IReadOnlyList<IReadonlyCheckPointProperty> AvaiableCheckPointProperties => _availableCheckPointProperties;
    public IReadOnlyDictionary<uint, IReadonlyCheckPointProperty> CheckPointPair => (IReadOnlyDictionary<uint, IReadonlyCheckPointProperty>)_checkPointPair;
    public CheckPointProperty CurrentCheckPointProperty => _currentCheckPointProperty;

    public event Action<IReadOnlyList<IReadonlyCheckPointProperty>> Inited;
    public event Action<IReadOnlyList<IReadonlyCheckPointProperty>> PointSold;
    public event Action<IReadonlyCheckPointProperty> PointCheckPropertyChanged;

    public void Init(List<CheckPointProperty> checkPointProperties, Wallet wallet, Storage storage)
    {
        _wallet = wallet;
        _storage = storage;
        _checkPointProperties = checkPointProperties;

        for (int i = 0; i < checkPointProperties.Count; i++)
        {
            if (checkPointProperties[i].Distance > _storage.BestDistance)
                continue;

            _availableCheckPointProperties.Add(checkPointProperties[i]);
            _checkPointPair.Add(checkPointProperties[i].Distance, checkPointProperties[i]);
        }

        InsertionSort();
        SetCurrentCheckPoint();

        Inited?.Invoke(AvaiableCheckPointProperties);
    }

    public void OnPointChecked(IReadonlyCheckPointProperty checkPointProperty)
    {
        if (checkPointProperty.IsChecked)
            return;

        if (_currentCheckPointProperty == null)
            SetCurrentCheckPoint();

        _currentCheckPointProperty.UnSetChecked();

        PointCheckPropertyChanged?.Invoke(_currentCheckPointProperty);

        _currentCheckPointProperty = _checkPointPair[checkPointProperty.Distance];
        _currentCheckPointProperty.SetChecked();

        PointCheckPropertyChanged?.Invoke(_currentCheckPointProperty);
    }

    public void OnPointSold(IReadonlyCheckPointProperty checkPointProperty)
    {
        if (_wallet.NutCount < checkPointProperty.Price)
            return;

        _wallet.Reduce(checkPointProperty.Price);

        CheckPointProperty targetCheckPoint = _checkPointPair[checkPointProperty.Distance];
        targetCheckPoint.SetBought();

        PointSold?.Invoke(AvaiableCheckPointProperties);
    }

    public void ResetState()
    {
        _currentCheckPointProperty = null;
        _availableCheckPointProperties.Clear();
        _checkPointPair.Clear();
        _wallet = null;
        _storage = null;
    }

    private void SetCurrentCheckPoint()
    {
        foreach (CheckPointProperty property in _checkPointPair.Values)
        {
            if (property.IsChecked)
            {
                _currentCheckPointProperty = property;
                break;
            }
        }
    }

    private void InsertionSort()
    {
        int length = _availableCheckPointProperties.Count;

        for (int i = 0; i < length; i++)
        {
            CheckPointProperty key = _availableCheckPointProperties[i];
            int j = i - 1;

            while (j >= 0 && _availableCheckPointProperties[j].Distance > key.Distance)
            {
                _availableCheckPointProperties[j + 1] = _availableCheckPointProperties[j];
                j--;
            }

            _availableCheckPointProperties[j + 1] = key;
        }
    }
}