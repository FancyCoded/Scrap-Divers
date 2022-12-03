using TMPro;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private TMP_Text _distance;

    private CheckPointMapView _checkPointMapView;
    private IReadonlyCheckPointProperty _properties;

    public IReadonlyCheckPointProperty Properties => _properties;
    public CheckPointMapView CheckPointMapView => _checkPointMapView;

    public virtual void Init(CheckPointMapView checkPointsMapView, IReadonlyCheckPointProperty checkPointProperty)
    {
        _checkPointMapView = checkPointsMapView;
        _properties = checkPointProperty;

        _distance.text = $"{ _properties.Distance}m";
    }
}