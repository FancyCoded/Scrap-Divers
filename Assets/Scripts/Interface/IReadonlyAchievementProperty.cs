public interface IReadonlyAchievementProperty
{
    AchievementType Type { get; }
    string Description { get; }
    uint Reward { get; }
    bool IsCollected { get; }
    bool IsCompleted { get; }
}