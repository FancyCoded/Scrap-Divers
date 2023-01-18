using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UncollectedRewardAchievementView : AchievementView
{
    [SerializeField] private TMP_Text _reward;
    [SerializeField] private Button _collect;
    [SerializeField] private Color _completedButtonColor;
    [SerializeField] private Color _uncompletedButtonColor;

    private AchievementWindow _achievementWindow;

    private void OnEnable()
    {
        _collect.onClick.AddListener(OnCollectButtonClicked);
    }

    private void OnDisable()
    {
        _collect.onClick.RemoveListener(OnCollectButtonClicked);
    }

    public void Init(IReadonlyAchievementProperty achievementProperty, AchievementWindow achievementWindow)
    {
        _achievementWindow = achievementWindow;
        _reward.text = achievementProperty.Reward.ToString();

        base.Init(achievementProperty);

        if (achievementProperty.IsCompleted)
            SetCompleted();
        else
            SetUncompleted();
    }

    public void SetCompleted()
    {
        _collect.interactable = true;
        _collect.image.color = _completedButtonColor; 
    }

    public void SetUncompleted()
    {
        _collect.interactable = false;
        _collect.image.color = _uncompletedButtonColor;
    }

    private void OnCollectButtonClicked() => _achievementWindow.ClickCollectButton(AchievementProperty);
}
