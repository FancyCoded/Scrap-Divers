using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : ObstaclePool, IResetable
{
    [SerializeField] private ItemSpawner _itemSpawner;

    private List<Obstacle> _spawnedObstacles = new List<Obstacle>();

    private uint _maxNutInRow = 4;
    private uint _minNutInRow = 1;
    private int _nutSpace = 2;

    private uint _magnetPerSpawnNumber = 4;
    private uint _wrenchPerSpawnNumber = 3;
    private uint _featherPerSpawnNumber = 3;
    private uint _starPerSpawnNumber = 3;
    private uint _itemSpawnedCounter = 0;

    public override void Init(LevelProperites levelProperites)
    {
        ResetState();
        base.Init(levelProperites);
    }

    public new void ResetState()
    {
        for (int i = 0; i < _spawnedObstacles.Count; i++)
        {
            DeleteChilds(_spawnedObstacles[i]);
            _spawnedObstacles[i].gameObject.SetActive(false);
        }

        _spawnedObstacles.Clear();
        base.ResetState();
    }

    public void Spawn(float zPosition)
    {
        Obstacle obstacle = GetObstacle();

        obstacle.transform.SetPositionAndRotation(new Vector3(0, 0, zPosition), Quaternion.identity);
        obstacle.gameObject.SetActive(true);
        _spawnedObstacles.Add(obstacle);

        uint minItemSpawnChance = 0;
        uint maxItemSpawnChance = 100;

        if (obstacle.ItemSpawnPoints.Count > 0 && obstacle.ItemSpawnChance >= Random.Range(minItemSpawnChance, maxItemSpawnChance))
        {
            _itemSpawnedCounter++;
            SpawnItemFor(obstacle);
        }
    }

    public void TryDeleteFirst(uint initialFrameCount, uint obstacleCountPerFrame)
    {
        if (_spawnedObstacles.Count <= initialFrameCount * obstacleCountPerFrame)
            return;

        for(int i = 0; i < obstacleCountPerFrame; i++)
        {
            DeleteChilds(_spawnedObstacles[0]);
            _spawnedObstacles[0].gameObject.SetActive(false);
            _spawnedObstacles.RemoveAt(0);
        }
    }

    private void SpawnItemFor(Obstacle obstacle)
    {
        int radnomIndex = Random.Range(0, obstacle.ItemSpawnPoints.Count);
        Transform point = obstacle.ItemSpawnPoints[radnomIndex].transform;

        if (_itemSpawnedCounter % _magnetPerSpawnNumber == 0)
            SpawnItem(ItemType.Magnet, point);
        else if (_itemSpawnedCounter % _wrenchPerSpawnNumber == 0)
            SpawnItem(ItemType.Wrench, point);
        else
            SpawnNuts(point);
    }

    private void SpawnItem(ItemType itemType, Transform point)
    {
        _itemSpawner.Spawn(itemType, point, point.position);
    }

    private void SpawnNuts(Transform point)
    {
        uint nutInRow = (uint)Random.Range(_minNutInRow, _maxNutInRow);
        Vector3 lastNutPosition = Vector3.zero;

        for (int i = 0; i < nutInRow; i++)
        {
            Vector3 nutPosition;

            if (i == 0)
            {
                nutPosition = point.position;
                lastNutPosition = nutPosition;

                _itemSpawner.Spawn(ItemType.Nut, point, nutPosition);
            }
            else
            {
                nutPosition = new Vector3(lastNutPosition.x, lastNutPosition.y, lastNutPosition.z + _nutSpace);
                lastNutPosition = nutPosition;

                _itemSpawner.Spawn(ItemType.Nut, point, nutPosition);
            }
        }
    }

    private void DeleteChilds(Obstacle obstacle)
    {
        Item[] items;
     
        for (int i = 0; i < obstacle.ItemSpawnPoints.Count; i++)
        {
            items = obstacle.ItemSpawnPoints[i].GetComponentsInChildren<Item>();

            for(int x = 0; x < items.Length; x++)
            {
                items[x].gameObject.SetActive(false);
                items[x].transform.parent = _itemSpawner.transform;
                items[x].transform.position = Vector3.zero;
            }
        }
    }
}