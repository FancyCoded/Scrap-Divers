using Newtonsoft.Json;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "AchievementProperties", order = 51)]
public class AchievementProperties : ScriptableObject, IReadonlyAchievementProperty
{
    private const string Russian = "Russian";
    private const string English = "English";
    private const string Turkish = "Turkish";

    [JsonProperty][SerializeField] private string _descriptionEn;
    [JsonProperty][SerializeField] private string _descriptionRu;
    [JsonProperty][SerializeField] private string _descriptionTr;
    [JsonProperty][SerializeField] private uint _reward;
    [JsonProperty][SerializeField] private bool _isCompleted;
    [JsonProperty][SerializeField] private bool _isCollected;
    [JsonProperty][SerializeField] private AchievementType _achievementType;

    public event Action DescriptionChanged;

    public string DescriptionEn => _descriptionEn;
    public string DescriptionRu => _descriptionRu;
    public string DescriptionTr => _descriptionTr;
    public string CurrentDescription { get; private set;}
    public uint Reward => _reward;
    public bool IsCompleted => _isCompleted;
    public bool IsCollected => _isCollected;
    public AchievementType Type => _achievementType;

    public void SetCompleted() => _isCompleted = true;
    public void SetCollected() => _isCollected = true;

    public void OnLanguageChanged(string language)
    {
        if(language == Russian)
            CurrentDescription = _descriptionRu;
        if(language == English) 
            CurrentDescription = _descriptionEn;
        if(language == Turkish) 
            CurrentDescription = _descriptionTr;

        DescriptionChanged?.Invoke();
    }
}