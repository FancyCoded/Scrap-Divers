using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class AchievementWindow : MonoBehaviour
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private CollectedRewardAchievementView _collectedRewardAchievementView;
    [SerializeField] private UncollectedRewardAchievementView _uncollectedRewardAchievementView;
    [SerializeField] private Button _back; 

    private Dictionary<AchievementType, UncollectedRewardAchievementView> _achievementViewsPair = new Dictionary<AchievementType, UncollectedRewardAchievementView>();
    private CanvasGroup _canvasGroup;

    public event UnityAction<IReadonlyAchievementProperty> CollectButtonClicked;

    private void OnEnable()
    {
        _back.onClick.AddListener(Hide);
    }

    private void OnDisable()
    {
        _back.onClick.RemoveListener(Hide);
    }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        Hide();
    }

    public void Display()
    {
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1;
    }

    public void Hide()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0;
    }

    public void OnUpdated(IReadOnlyList<IReadonlyAchievementProperty> achievementProperties)
    {
        ResetState();

        for(int i = 0; i < achievementProperties.Count; i++)
            CreateAchievementView(achievementProperties[i]);
    }

    public void OnAchievementCompleted(IReadonlyAchievementProperty achievementProperty)
    {
        _achievementViewsPair[achievementProperty.Type].SetCompleted();
    }

    public void ClickCollectButton(IReadonlyAchievementProperty achievementProperty)
    {
        CollectButtonClicked?.Invoke(achievementProperty);
    }

    public void ResetState()
    {
        for (int i = 0; i < _content.childCount; i++)
            Destroy(_content.GetChild(i).gameObject);

        _achievementViewsPair.Clear();
    }

    private void CreateAchievementView(IReadonlyAchievementProperty achievementProperty)
    {
        if (achievementProperty.IsCollected)
        {
            CollectedRewardAchievementView achievementView = Instantiate(_collectedRewardAchievementView, _content);
            achievementView.Init(achievementProperty);
        }
        else
        {
            UncollectedRewardAchievementView achievementView = Instantiate(_uncollectedRewardAchievementView, _content);
            _achievementViewsPair.Add(achievementProperty.Type, achievementView);
            achievementView.Init(achievementProperty, this);
        }
    }
}