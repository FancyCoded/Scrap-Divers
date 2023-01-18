using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AchievementMap : IResetable
{
    private AchievementFactory _achievementFactory;
    private Wallet _wallet;

    private List<AchievementProperties> _achievementProperties = new List<AchievementProperties>();
    private Dictionary<AchievementType, AchievementProperties> _achievementPropertiesPair = new Dictionary<AchievementType, AchievementProperties>();
    private List<IAchievement> _achievements = new List<IAchievement>();

    public IReadOnlyList<IReadonlyAchievementProperty> AchievementProperties => _achievementProperties;
    public IReadOnlyDictionary<AchievementType, IReadonlyAchievementProperty> AchievementPropertiesPair => (IReadOnlyDictionary<AchievementType, IReadonlyAchievementProperty>)_achievementPropertiesPair;

    public event Action<IReadOnlyList<IReadonlyAchievementProperty>> Inited;
    public event Action<IReadOnlyList<IReadonlyAchievementProperty>> AchievementRewardCollected;
    public event Action<IReadonlyAchievementProperty> AchievementCompleted;

    public void Init(List<AchievementProperties> achievementProperties, Wallet wallet,
        AchievementFactory achievementFactory)
    {
        ResetState();

        _achievementProperties = achievementProperties;
        _wallet = wallet;
        _achievementFactory = achievementFactory;

        for (int i = 0; i < _achievementProperties.Count; i++)
            _achievementPropertiesPair.Add(_achievementProperties[i].Type, _achievementProperties[i]);

        CreateAchievements(_achievementProperties);

        Inited?.Invoke(AchievementProperties);
    }

    public void ResetState()
    {
        _achievementPropertiesPair.Clear();
        _achievements.Clear();
        _wallet = null;
    }

    public void CompleteAchievement(IReadonlyAchievementProperty achievementProperty)
    {
        AchievementProperties achievementProperties = _achievementPropertiesPair[achievementProperty.Type];
        AchievementCompleted?.Invoke(achievementProperty);
    }

    public void OnAchievementRewardCollected(IReadonlyAchievementProperty achievementProperty)
    {
        AchievementProperties achievementProperties = _achievementPropertiesPair[achievementProperty.Type];

        achievementProperties.SetCollected();
        _wallet.Increase(achievementProperty.Reward);
        AchievementRewardCollected?.Invoke(AchievementProperties);
    }

    public void CheckAchievementsComplete()
    {
        for(int i = 0; i < _achievements.Count; i++)
            if (_achievements[i].CheckComplete())
                CompleteAchievement(_achievements[i].Properties);
    }

    private void CreateAchievements(List<AchievementProperties> achievementProperties)
    {
        for (int i = 0; i < achievementProperties.Count; i++)
            if (_achievementProperties[i].IsCompleted == false)
                _achievements.Add(_achievementFactory.CreateAchievement(achievementProperties[i]));
    }
}
