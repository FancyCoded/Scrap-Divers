public class FinishLevelAchievement : IAchievement
{
    private readonly AchievementProperties _properties;
    private readonly Storage _storage;
    private readonly uint _targetLevel;
    private readonly Level _level;

    public FinishLevelAchievement(AchievementProperties properties, Storage storage, Level level, uint targetLevel)
    {
        _properties = properties;
        _storage = storage;
        _level= level;
        _targetLevel = targetLevel;
    }

    public IReadonlyAchievementProperty Properties => _properties;

    public bool IsCompleted => _properties.IsCompleted;

    public bool CheckComplete()
    {
        if (IsCompleted)
            return IsCompleted;

        if (_level == null || _storage == null)
            return false;

        if (_storage.BestDistance >= _level.LevelPropertiesPair[_targetLevel].CoveredLevelsLengthPure)
            _properties.SetCompleted();

        return IsCompleted;
    }
}