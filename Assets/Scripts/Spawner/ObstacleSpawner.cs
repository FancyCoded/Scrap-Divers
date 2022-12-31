using System;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour, IDisposable
{
    [SerializeField] private ObstaclePool _obstaclePool;
    [SerializeField] private ItemSpawner _itemSpawner;

    private readonly Queue<Obstacle> _spawnedObstacles = new Queue<Obstacle>();

    private ObstaclePreparer _obstaclePreparer;
    private LevelProperties _levelProperties;
    private ObjectSpawner<Obstacle> _spawner;

    public void Init(LevelProperties levelProperties)
    {
        _levelProperties = levelProperties;
        _spawner ??= new ObjectSpawner<Obstacle>(GetRandom, OnSpawned);
        _obstaclePreparer ??= new ObstaclePreparer(_itemSpawner, levelProperties);

        _obstaclePool.Init(levelProperties);
    }

    public void Spawn(Vector3 position) => _spawner.Spawn(position);

    public void ReleaseFirst(uint initialFrameCount)
    {
        if (_spawnedObstacles.Count <= initialFrameCount * _levelProperties.ObstacleCountPerFrame)
            return;

        for (int i = 0; i < _levelProperties.ObstacleCountPerFrame; i++)
        {
            Obstacle obstacle = _spawnedObstacles.Dequeue();

            _obstaclePreparer.ReleaseChilds(obstacle);
            _obstaclePool.Release(obstacle);
        }
    }

    public void Dispose()
    {
        while (_spawnedObstacles.Count > 0)
        {
            Obstacle target = _spawnedObstacles.Dequeue();

            _obstaclePreparer.ReleaseChilds(target);
            _obstaclePool.Release(target);
        }

        _spawnedObstacles.Clear();
        _obstaclePool.Dispose();
    }

    private void OnSpawned(Obstacle obstacle)
    {
        _spawnedObstacles.Enqueue(obstacle);
        _obstaclePreparer.Prepare(obstacle);
    }

    private Obstacle GetRandom() => _obstaclePool.GetRandom();
}