using System;
using System.Collections.Generic;
using UnityEngine;

public class FrameSpawner : MonoBehaviour, IDisposable
{
    private const uint FrameScaleZ = 100;

    private readonly Queue<Frame> _spawnedFrames = new Queue<Frame>();

    [SerializeField] private FramePool _framePool;

    private ObjectSpawner<Frame> _spawner;
    private Vector3 _frameSpawnStartPosition;

    public IReadOnlyCollection<Frame> SpawnedFrames => _spawnedFrames;
    public Vector3 LastFramePosition => _spawnedFrames.ToArray()[_spawnedFrames.Count - 1].transform.position;
    public float FrameCenter => FrameScaleZ / 2;
    public float LastFrameOriginZ => LastFramePosition.z - FrameCenter;
    public float LastFrameEndZ => LastFramePosition.z + FrameCenter;

    public void Init(LevelProperties levelProperties, uint initialFramesCount, uint startPositionZ)
    {
        _framePool.Init(initialFramesCount, levelProperties);

        _frameSpawnStartPosition = new Vector3(0, 0, startPositionZ + FrameCenter);
        _spawner ??= new ObjectSpawner<Frame>(Get, OnSpawned);
    }

    public void Spawn()
    {
        Vector3 position;

        if (_spawnedFrames.Count == 0)
            position = _frameSpawnStartPosition;
        else
            position = LastFramePosition + Vector3.forward * FrameScaleZ;

        _spawner.Spawn(position);
    }

    public void ReleaseFirst(uint initialCount)
    {
        if (_spawnedFrames.Count <= initialCount)
            return;

        Frame frame = _spawnedFrames.Dequeue();
        _framePool.Release(frame);
    }

    public void Dispose()
    {
        while(_spawnedFrames.Count > 0)
        {
            Frame frame = _spawnedFrames.Dequeue();
            _framePool.Release(frame);
        }

        _spawnedFrames.Clear();
        _framePool.Dispose();
    }

    private Frame Get() => _framePool.Get();

    private void OnSpawned(Frame frame) => _spawnedFrames.Enqueue(frame);
}