using System;
using System.Collections.Generic;
using UnityEngine;

public class FrameSpawner2 : MonoBehaviour, IDisposable
{
    private readonly Queue<Frame> _spawnedObstacles = new Queue<Frame>();
    private LevelProperties _levelProperties;
    private ObjectSpawner<Frame> _spawner;
    private ObjectPool<Frame> _framePool;

    public void Init(ObjectPool<Frame> framePool)
    {
        _spawner = new ObjectSpawner<Frame>(Get, OnSpawned);
        _framePool = framePool;
    }

    public void Spawn(Vector3 position)
    {
        _spawner.Spawn(position);
    }

    public void Release(uint count)
    {
        if (_spawnedObstacles.Count <= count)
            return;

        Frame frame = _spawnedObstacles.Dequeue();
        _framePool.Release(ref frame);
    }

    public void Dispose()
    {
        _spawnedObstacles.Clear();
        _framePool.Dispose();
    }

    private Frame Get() => _framePool.Get();

    private void OnSpawned(Frame frame)
    {
        _spawnedObstacles.Enqueue(frame);
    }
}