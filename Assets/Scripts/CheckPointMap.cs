using System;
using System.Collections.Generic;

public class CheckPointMap : IResetable
{
    private Wallet _wallet;
    private Storage _storage;

    private List<CheckPointProperty> _checkPointProperties = new List<CheckPointProperty>();
    private List<CheckPointProperty> _availableCheckPointProperties = new List<CheckPointProperty>();
    private Dictionary<uint, CheckPointProperty> _checkPointKeyValue = new Dictionary<uint, CheckPointProperty>();
    private CheckPointProperty _currentCheckPointProperty = null;

    public IReadOnlyList<IReadonlyCheckPointProperty> CheckPointProperties => _checkPointProperties;
    public IReadOnlyList<IReadonlyCheckPointProperty> AvaiableCheckPointProperties => _availableCheckPointProperties;
    public IReadOnlyDictionary<uint, IReadonlyCheckPointProperty> CheckPointKeyValue => (IReadOnlyDictionary<uint, IReadonlyCheckPointProperty>)_checkPointKeyValue;
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
            _checkPointKeyValue.Add(checkPointProperties[i].Distance, checkPointProperties[i]);
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

        _currentCheckPointProperty = _checkPointKeyValue[checkPointProperty.Distance];
        _currentCheckPointProperty.SetChecked();

        PointCheckPropertyChanged?.Invoke(_currentCheckPointProperty);
    }

    public void OnPointSold(IReadonlyCheckPointProperty checkPointProperty)
    {
        if (_wallet.NutCount < checkPointProperty.Price)
            return;

        _wallet.Reduce(checkPointProperty.Price);

        CheckPointProperty targetCheckPoint = _checkPointKeyValue[checkPointProperty.Distance];
        targetCheckPoint.SetBought();

        PointSold?.Invoke(AvaiableCheckPointProperties);
    }

    public void ResetState()
    {
        _currentCheckPointProperty = null;
        _checkPointProperties.Clear();
        _availableCheckPointProperties.Clear();
        _checkPointKeyValue.Clear();
        _wallet = null;
    }

    private void SetCurrentCheckPoint()
    {
        foreach (CheckPointProperty property in _checkPointKeyValue.Values)
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