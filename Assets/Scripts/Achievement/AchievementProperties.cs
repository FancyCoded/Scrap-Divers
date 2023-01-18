using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(menuName = "AchievementProperties", order = 51)]
public class AchievementProperties : ScriptableObject, IReadonlyAchievementProperty
{
    [JsonProperty][SerializeField] private string _description;
    [JsonProperty][SerializeField] private uint _reward;
    [JsonProperty][SerializeField] private bool _isCompleted;
    [JsonProperty][SerializeField] private bool _isCollected;
    [JsonProperty][SerializeField] private AchievementType _achievementType;

    public string Description => _description;
    public uint Reward => _reward;
    public bool IsCompleted => _isCompleted;
    public bool IsCollected => _isCollected;
    public AchievementType Type => _achievementType;

    public void SetCompleted() => _isCompleted = true;
    public void SetCollected() => _isCollected = true;
}