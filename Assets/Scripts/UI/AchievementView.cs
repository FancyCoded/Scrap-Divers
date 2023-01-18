using TMPro;
using UnityEngine;

public abstract class AchievementView : MonoBehaviour
{
    [SerializeField] private TMP_Text _description;

    private IReadonlyAchievementProperty _achievementProperty;

    public IReadonlyAchievementProperty AchievementProperty => _achievementProperty;

    public virtual void Init(IReadonlyAchievementProperty achievementProperty)
    {
        _achievementProperty = achievementProperty;
        _description.text = _achievementProperty.Description.ToString();
    }
}