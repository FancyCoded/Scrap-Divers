using UnityEngine;

public class FallingTimeAchievement : IAchievement
{
    private readonly IAchievement _achievement;
    private readonly AchievementProperties _properties;
    private readonly FallingTimer _fallingTimer;
    private readonly MathCompareType _compareType;
    private readonly uint _targetFallingTime;

    public FallingTimeAchievement(AchievementProperties properties, FallingTimer fallingTimer,
        uint targetFallingTime, MathCompareType compareType, IAchievement achievement = null)
    {
        _achievement = achievement;
        _properties = properties;
        _fallingTimer = fallingTimer;
        _targetFallingTime = targetFallingTime;
        _compareType = compareType;

        if (_fallingTimer != null)
            fallingTimer.TimeChanged += OnTimeChanged;
    }

    public bool IsCompleted { get; private set; } = false;
    public IReadonlyAchievementProperty Properties => _properties;

    public bool CheckComplete()
    {
        if (IsCompleted)
            return IsCompleted;

        if (_fallingTimer == null)
            return false;

        if(_achievement == null)
        {
            if (MathComparer.Compare(_fallingTimer.Time, _targetFallingTime, _compareType))
            {
                _properties.SetCompleted();
                IsCompleted = true;
            }
        }
        else if(MathComparer.Compare(_fallingTimer.Time, _targetFallingTime, _compareType))
            IsCompleted = _achievement.CheckComplete();

        return IsCompleted;
    }

    private void OnTimeChanged()
    {
        CheckComplete();
    }
}
