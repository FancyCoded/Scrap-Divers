using System;
using UnityEngine;

public class FramePool2 : MonoBehaviour, IDisposable
{
    [SerializeField] private uint _initialCount;
    [SerializeField] private Frame _template;
    [SerializeField] private Transform _container;

    private ObjectPool<Frame> _pool;

    public void Init()
    {
        _pool = new ObjectPool<Frame>(CreateNew, OnGot, OnReleased, OnDestoyed);

        for (int i = 0; i < _initialCount; i++)
        {
            Frame frame = CreateNew();
            _pool.Release(ref frame);
        }
    }

    public void Dispose()
    {
        _pool.Dispose();
    }

    private Frame CreateNew()
    {
        return Instantiate(_template, _container);
    }

    private void OnReleased(Frame frame)
    {
        frame.gameObject.SetActive(false);
    }

    private void OnDestoyed(Frame frame)
    {
        Destroy(frame.gameObject);
    }

    private void OnGot(Frame frame)
    {
        frame.gameObject.SetActive(true);
    }
}