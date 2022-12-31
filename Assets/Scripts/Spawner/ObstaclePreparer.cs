using UnityEngine;
using Random = UnityEngine.Random;

public class ObstaclePreparer
{
    private ItemSpawner _itemSpawner;
    private uint _minItemSpawnChance = 0;
    private uint _maxItemSpawnChance = 100;

    private LevelProperties _levelProperties;
    private float _buffSpawnChance;

    public ObstaclePreparer(ItemSpawner itemSpawner,
        LevelProperties levelProperties,
        uint minItemSpawnChance = 0,
        uint maxItemSpawnChance = 100)
    {
        _itemSpawner = itemSpawner;
        _levelProperties = levelProperties;
        _minItemSpawnChance = minItemSpawnChance;
        _maxItemSpawnChance = maxItemSpawnChance;
    }

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
        Item buff = _itemSpawner.Buffs[Random.Range(0, _itemSpawner.Buffs.Count)];

        _itemSpawner.Spawn(buff, point.position, point);
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

            _itemSpawner.Spawn(_itemSpawner.Nut, nutPosition, point);
        }
    }

    public void ReleaseChilds(Obstacle obstacle)
    {
        Item[] items;

        for (int i = 0; i < obstacle.ItemSpawnPoints.Count; i++)
        {
            items = obstacle.ItemSpawnPoints[i].GetComponentsInChildren<Item>();

            for (int x = 0; x < items.Length; x++)
                _itemSpawner.Release(items[x]);
        }
    }
}