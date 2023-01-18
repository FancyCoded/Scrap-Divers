public interface IAchievement
{
    IReadonlyAchievementProperty Properties { get; }
    bool IsCompleted { get; }
    bool CheckComplete();
}