using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(menuName = "CheckPointProperty", order = 51)]
public class CheckPointProperty : ScriptableObject, IReadonlyCheckPointProperty, IResetable
{
    [JsonProperty][SerializeField] private uint _level;
    [JsonProperty][SerializeField] private uint _distance;
    [JsonProperty][SerializeField] private uint _price;
    [JsonProperty][SerializeField] private bool _isBought;
    [JsonProperty][SerializeField] private bool _isChecked;

    public uint Level => _level;
    public uint Distance => _distance;
    public uint Price => _price;
    public bool IsBought => _isBought;
    public bool IsChecked => _isChecked;

    private void Awake()
    {
        if (_distance == 0)
            _isBought = true;
    }

    public void SetBought() => _isBought = true;

    public void SetChecked() => _isChecked = true;

    public void UnSetChecked() => _isChecked = false;

    public void ResetState()
    {
        if (_distance == 0)
        {
            _isBought = true;
            _isChecked = true;
            return;
        }

        _isBought = false;
        _isChecked = false;
    }
}