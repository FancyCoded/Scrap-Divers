using System;
using UnityEngine;

[Serializable]
public class CollectItemAchievement : IAchievement
{
    private readonly AchievementProperties _properties;
    private readonly Collector _collector;
    private readonly MathCompareType _compareType;
    private readonly uint _targetCount;
    private readonly ItemType _itemType;

    public CollectItemAchievement(ItemType itemType, Collector collector, AchievementProperties properties, uint targetCount,
        MathCompareType mathCompareType)
    {
        _itemType = itemType;
        _collector = collector;
        _properties = properties;
        _targetCount = targetCount;
        _compareType = mathCompareType;
    }

    public bool IsCompleted => _properties.IsCompleted;
    public IReadonlyAchievementProperty Properties => _properties;

    public bool CheckComplete()
    {
        if (IsCompleted)
            return IsCompleted;

        if(_collector == null)
            return false;

        if (_itemType == ItemType.Nut)
            TryComplete(_collector.NutCount);
        if (_itemType == ItemType.Magnet)
            TryComplete(_collector.MagnetCount);
        if(_itemType == ItemType.Wrench) 
            TryComplete(_collector.WrenchCount);
        if(_itemType == ItemType.Star)
            TryComplete(_collector.StarCount);
        if(_itemType == ItemType.Feather)
            TryComplete(_collector.FeatherCount);
        
        return IsCompleted;
    }

    private void TryComplete(uint itemCount)
    {
        if (MathComparer.Compare(itemCount, _targetCount, _compareType))
            _properties.SetCompleted();
    }
}