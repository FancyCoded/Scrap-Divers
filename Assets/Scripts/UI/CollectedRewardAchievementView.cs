using TMPro;
using UnityEngine;

public class CollectedRewardAchievementView : AchievementView 
{
    [SerializeField] private TMP_Text _reward;

    public override void Init(IReadonlyAchievementProperty achievementProperty)
    {
        base.Init(achievementProperty);
        _reward.text = AchievementProperty.Reward.ToString();
    }
}