using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstaclePool : MonoBehaviour, IDisposable
{
    [SerializeField] private Transform _container;

    private IObjectPool<Obstacle> _pool;
    private LevelProperties _levelProperties;

    private Obstacle[] _templates => _levelProperties.Obstacles;

    public void Init(LevelProperties levelProperties)
    {
        _pool ??= new ObjectPool<Obstacle>(CreateNew, OnDestroyed, OnGot, OnReleased);
        _levelProperties = levelProperties;

        for (int i = 0; i < _templates.Length; i++)
        {
            Obstacle obstacle = Create(_templates[i]);
            Release(obstacle);
        }
    }

    public Obstacle Get() => _pool.Get();

    public Obstacle GetRandom() => _pool.GetRandom();

    public void Release(Obstacle obstacle) => _pool?.Release(obstacle);

    public void Dispose()
    {
        if (_pool is IDisposable disposable)
            disposable?.Dispose();
    }

    private Obstacle CreateNew()
    {
        int randomIndex = Random.Range(0, _templates.Length);
        return Instantiate(_templates[randomIndex], _container);
    }

    private Obstacle Create(Obstacle template) => Instantiate(template, _container);

    private void OnGot(Obstacle obstacle) => obstacle.gameObject.SetActive(true);

    private void OnReleased(Obstacle obstacle) => obstacle.gameObject.SetActive(false);

    private void OnDestroyed(Obstacle obstacle) => Destroy(obstacle.gameObject);
}
