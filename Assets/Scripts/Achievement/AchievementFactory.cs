public class AchievementFactory
{
    private readonly Collector _collector;
    private readonly FallingTimer _fallingTimer;
    private readonly Body _body;
    private readonly Storage _storage;
    private readonly Level _level;

    public AchievementFactory(Collector collector = null, FallingTimer fallingTimer = null, Body body = null,
        Level level = null, Storage storage = null)
    {
        _collector = collector;
        _fallingTimer = fallingTimer;
        _body = body;
        _storage = storage;
        _level = level;
    }

    public IAchievement CreateAchievement(AchievementProperties achievementProperties)
    {
        if (achievementProperties.Type == AchievementType.Collect20Gold)
            return new CollectItemAchievement(ItemType.Nut, _collector, achievementProperties, 20, MathCompareType.EqualMore);

        if (achievementProperties.Type == AchievementType.Collect60Gold)
            return new CollectItemAchievement(ItemType.Nut, _collector, achievementProperties, 60, MathCompareType.EqualMore);

        if (achievementProperties.Type == AchievementType.Collect100Gold)
            return new CollectItemAchievement(ItemType.Nut, _collector, achievementProperties, 100, MathCompareType.EqualMore);

        if (achievementProperties.Type == AchievementType.Collect3Fether)
            return new CollectItemAchievement(ItemType.Feather, _collector, achievementProperties, 3, MathCompareType.EqualMore);

        if (achievementProperties.Type == AchievementType.Collect3Magnet)
            return new CollectItemAchievement(ItemType.Magnet, _collector, achievementProperties, 3, MathCompareType.EqualMore);

        if (achievementProperties.Type == AchievementType.Collect3Star)
            return new CollectItemAchievement(ItemType.Star, _collector, achievementProperties, 3, MathCompareType.EqualMore);

        if (achievementProperties.Type == AchievementType.Collect3Wrench)
            return new CollectItemAchievement(ItemType.Wrench, _collector, achievementProperties, 3, MathCompareType.EqualMore);

        if (achievementProperties.Type == AchievementType.Lose2BodyPartFirst20Seconds)
        {
            CollisionAchievement achievement = new CollisionAchievement(achievementProperties, _body, 2, MathCompareType.EqualMore, CollisionType.Destruction);
            return new FallingTimeAchievement(achievementProperties, _fallingTimer, 20, MathCompareType.EqualLess, achievement);
        }

        if (achievementProperties.Type == AchievementType.Lose4BodyPartFirst50Seconds)
        {
            CollisionAchievement achievement = new CollisionAchievement(achievementProperties, _body, 4, MathCompareType.EqualMore, CollisionType.Destruction);
            return new FallingTimeAchievement(achievementProperties, _fallingTimer, 50, MathCompareType.EqualLess, achievement);
        }

        if (achievementProperties.Type == AchievementType.LoseMaxBodyParts)
            return new CollisionAchievement(achievementProperties, _body, 8, MathCompareType.Equal, CollisionType.Destruction);

        if (achievementProperties.Type == AchievementType.DontLoseBodyPartFirst25Seconds)
        {
            CollisionAchievement achievement = new CollisionAchievement(achievementProperties, _body, 0, MathCompareType.Equal, CollisionType.Destruction);
            return new FallingTimeAchievement(achievementProperties, _fallingTimer, 25, MathCompareType.EqualMore, achievement);
        }

        if (achievementProperties.Type == AchievementType.DontLoseBodyPartFirst50Seconds)
        {
            CollisionAchievement achievement = new CollisionAchievement(achievementProperties, _body, 0, MathCompareType.Equal, CollisionType.Destruction);
            return new FallingTimeAchievement(achievementProperties, _fallingTimer, 50, MathCompareType.EqualMore, achievement);
        }

        if (achievementProperties.Type == AchievementType.MaxCollide1First20Seconds)
        {
            CollisionAchievement achievement = new CollisionAchievement(achievementProperties, _body, 1, MathCompareType.EqualLess, CollisionType.Colliding);
            return new FallingTimeAchievement(achievementProperties, _fallingTimer, 20, MathCompareType.EqualMore, achievement);
        }

        if (achievementProperties.Type == AchievementType.MaxCollide2First60Seconds)
        {
            CollisionAchievement achievement = new CollisionAchievement(achievementProperties, _body, 2, MathCompareType.EqualLess, CollisionType.Colliding);
            return new FallingTimeAchievement(achievementProperties, _fallingTimer, 60, MathCompareType.EqualMore, achievement);
        }

        if (achievementProperties.Type == AchievementType.MaxCollide0First40Seconds)
        {
            CollisionAchievement achievement = new CollisionAchievement(achievementProperties, _body, 0, MathCompareType.Equal, CollisionType.Colliding);
            return new FallingTimeAchievement(achievementProperties, _fallingTimer, 40, MathCompareType.EqualMore, achievement);
        }

        if (achievementProperties.Type == AchievementType.CollideParts10TimesFirst30Seconds)
        {
            CollisionAchievement achievement = new CollisionAchievement(achievementProperties, _body, 10, MathCompareType.EqualMore, CollisionType.Colliding);
            return new FallingTimeAchievement(achievementProperties, _fallingTimer, 30, MathCompareType.EqualLess, achievement);
        }

        if (achievementProperties.Type == AchievementType.CollideParts15TimesFirst50Seconds)
        {
            CollisionAchievement achievement = new CollisionAchievement(achievementProperties, _body, 15, MathCompareType.EqualMore, CollisionType.Colliding);
            return new FallingTimeAchievement(achievementProperties, _fallingTimer, 50, MathCompareType.EqualLess, achievement);
        }

        if (achievementProperties.Type == AchievementType.Falling30Seconds)
            return new FallingTimeAchievement(achievementProperties, _fallingTimer, 30, MathCompareType.EqualMore);

        if(achievementProperties.Type == AchievementType.Falling60Seconds)
            return new FallingTimeAchievement(achievementProperties, _fallingTimer, 60, MathCompareType.EqualMore);

        if(achievementProperties.Type == AchievementType.Falling100Seconds)
            return new FallingTimeAchievement(achievementProperties, _fallingTimer, 100, MathCompareType.EqualMore);

        if (achievementProperties.Type == AchievementType.FinishLevel1)
            return new FinishLevelAchievement(achievementProperties, _storage, _level, 1);

        return null;
    }
}
