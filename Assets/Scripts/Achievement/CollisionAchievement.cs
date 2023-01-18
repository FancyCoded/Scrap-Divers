public class CollisionAchievement : IAchievement
{
    private readonly AchievementProperties _properties;
    private readonly Body _body;
    private readonly MathCompareType _compareType;
    private readonly CollisionType _collisionType;
    private readonly uint _targetCount;

    public CollisionAchievement(AchievementProperties properties, Body body, uint targetCount,
        MathCompareType compareType, CollisionType collisionType)
    {
        _properties = properties;
        _body = body;
        _targetCount = targetCount;
        _compareType = compareType;
        _collisionType = collisionType;
    }

    public IReadonlyAchievementProperty Properties => _properties;
    public bool IsCompleted => _properties.IsCompleted;

    public bool CheckComplete()
    {
        if (IsCompleted)
            return IsCompleted;

        if (_body == null)
            return false;

        if (_collisionType == CollisionType.Colliding)
            TryComplete(_body.CollidingsCount);
        if (_collisionType == CollisionType.Destruction)
            TryComplete(_body.DestructedPartCount);

        return IsCompleted;
    }

    private void TryComplete(uint collisionCount)
    {
        if (MathComparer.Compare(collisionCount, _targetCount, _compareType))
            _properties.SetCompleted();
    }
}