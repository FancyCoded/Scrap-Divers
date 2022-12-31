using System;
using UnityEngine;

public class FramePool : MonoBehaviour, IDisposable
{
    [SerializeField] private Transform _container;

    private ObjectPool<Frame> _pool;
    private LevelProperties _levelProperties;

    private Frame _template => _levelProperties.Frame;

    public void Init(uint initialCount, LevelProperties levelProperties)
    {
        _levelProperties = levelProperties;

        _pool ??= new ObjectPool<Frame>(CreateNew, OnDestoyed, OnGot, OnReleased);

        for (int i = 0; i < initialCount; i++)
        {
            Frame frame = CreateNew();
            Release(frame);
        }
    }

    public Frame Get() => _pool.Get();

    public void Release(Frame frame) => _pool.Release(frame);

    private Frame CreateNew() => Instantiate(_template, _container);

    public void Dispose() => _pool?.Dispose();

    private void OnReleased(Frame frame) => frame.gameObject.SetActive(false);

    private void OnDestoyed(Frame frame) => Destroy(frame.gameObject);

    private void OnGot(Frame frame) => frame.gameObject.SetActive(true);
}