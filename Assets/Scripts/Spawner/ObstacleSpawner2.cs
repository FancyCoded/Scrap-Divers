using System;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner2 : MonoBehaviour, IDisposable
{
    private readonly Queue<Obstacle> _spawnedObstacles = new Queue<Obstacle>();
    private ObstaclePreparer _obstaclePreparer = new ObstaclePreparer();
    private LevelProperties _levelProperties;
    private ObjectPool<Obstacle> _obstaclePool;

    public void Init()
    {
        Dispose();
        _obstaclePool = new ObjectPool<Obstacle>(Get ,OnSpawned);
    }

    public void ReleaseFirst(uint count)
    {
        if (_spawnedObstacles.Count <= count)
            return;

        Obstacle obstacle = _spawnedObstacles.Dequeue();
        _obstaclePool.Release(ref obstacle);
    }

    public void Dispose()
    {
        for (int i = 0; i < _spawnedObstacles.Count; i++)
        {
            Obstacle target = _spawnedObstacles.Dequeue();
            _obstaclePreparer.ReleaseChilds(target);
        }

        _obstaclePool.Dispose();
    }

    public void Release(Obstacle obstacle)
    {
        _obstaclePreparer.ReleaseChilds(obstacle);
    }

    private void OnSpawned(Obstacle obstacle)
    {
        _spawnedObstacles.Enqueue(obstacle);
        _obstaclePreparer.Prepare(obstacle);
    }

    private Obstacle Get() => _obstaclePool.Get();
}