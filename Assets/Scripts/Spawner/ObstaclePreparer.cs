using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class ObstaclePreparer
{
    [SerializeField] private ItemSpawner2 _itemSpawner;
    [SerializeField] private uint _minItemSpawnChance = 0;
    [SerializeField] private uint _maxItemSpawnChance = 100;

    private LevelProperties _levelProperties;
    private float _buffSpawnChance;

    public void Prepare(Obstacle obstacle)
    {
        if (obstacle.ItemSpawnPoints.Count > 0 && obstacle.ItemSpawnChance >= Random.Range(_minItemSpawnChance, _maxItemSpawnChance))
            SpawnItemFor(obstacle);
    }

    private void SpawnItemFor(Obstacle obstacle)
    {
        int radnomIndex = Random.Range(0, obstacle.ItemSpawnPoints.Count);
        Transform point = obstacle.ItemSpawnPoints[radnomIndex].transform;
        _buffSpawnChance = Random.Range(0, _levelProperties.BuffSpawnChanceMax);

        if (_buffSpawnChance >= Random.Range(0, 100))
            SpawnRandomBuff(point);
        else
            SpawnNuts(point);
    }

    private void SpawnRandomBuff(Transform point)
    {
        int length = Enum.GetValues(typeof(ItemType)).Length;
        ItemType itemType = (ItemType)Random.Range(1, length);

        _itemSpawner.SpawnBy(itemType, point.position, point);
    }

    private void SpawnNuts(Transform point)
    {
        uint nutInRow = (uint)Random.Range(_levelProperties.MinNutInRow, _levelProperties.MaxNutInRow);
        Vector3 lastNutPosition = Vector3.zero;

        for (int i = 0; i < nutInRow; i++)
        {
            Vector3 nutPosition;

            if (i == 0)
                nutPosition = point.position;
            else
                nutPosition = new Vector3(lastNutPosition.x, lastNutPosition.y, lastNutPosition.z + _levelProperties.NutSpace);

            lastNutPosition = nutPosition;

            _itemSpawner.SpawnBy(ItemType.Nut, nutPosition, point);
        }
    }

    public void ReleaseChilds(Obstacle obstacle)
    {
        Item[] items;

        for (int i = 0; i < obstacle.ItemSpawnPoints.Count; i++)
        {
            items = obstacle.ItemSpawnPoints[i].GetComponentsInChildren<Item>();

            for (int x = 0; x < items.Length; x++)
                _itemSpawner.Release(ref items[x]);
        }
    }
}