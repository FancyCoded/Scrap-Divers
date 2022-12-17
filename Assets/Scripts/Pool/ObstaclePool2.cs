using System;
using UnityEngine;

public class ObstaclePool2 : MonoBehaviour, IDisposable
{
    [SerializeField] private uint _initialCount;
    [SerializeField] private Obstacle[] _templates;
    [SerializeField] private Transform _container;

    private ObjectPool<Obstacle> _pool;

    public void Init()
    {
        _pool = new ObjectPool<Obstacle>(CreateNew, OnGot, OnReleased, OnDestroyed);

        for (int i = 0; i < _initialCount; i++)
        {
            Obstacle obstacle = Create(_templates[0]);
            _pool.Release(ref obstacle);
        }
    }

    public void Dispose()
    {
        _pool.Dispose();
    }

    private Obstacle CreateNew()
    {
        return Instantiate(_templates[1], _container);
    }

    private Obstacle Create(Obstacle template)
    {
        return Instantiate(template, _container);
    }

    private void OnGot(Obstacle entity)
    {
        entity.gameObject.SetActive(true);
    }

    private void OnReleased(Obstacle entity)
    {
        entity.gameObject.SetActive(false);
    }

    private void OnDestroyed(Obstacle obstacle)
    {
        Destroy(obstacle.gameObject);
    }
}
