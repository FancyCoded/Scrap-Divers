using System;

public interface IReadonlyAchievementProperty
{
    event Action DescriptionChanged;

    AchievementType Type { get; }
    string DescriptionEn { get; }
    string DescriptionTr { get; }
    string DescriptionRu { get; }
    string CurrentDescription { get; }
    uint Reward { get; }
    bool IsCollected { get; }
    bool IsCompleted { get; }
}