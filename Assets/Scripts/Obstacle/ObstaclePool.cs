using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour, IResetable
{
    [SerializeField] private Transform _container;

    private List<Obstacle> _pool = new List<Obstacle>();
    private List<Obstacle> _disabledObstacles = new List<Obstacle>();

    private LevelProperties _currentLevelProperties;

    public LevelProperties LevelProperties => _currentLevelProperties;

    public virtual void Init(LevelProperties levelProperites)
    {
        _currentLevelProperties = levelProperites;

        for (int i = 0; i < levelProperites.Obstacles.Length; i++)
            Create(levelProperites.Obstacles[i]);
    }

    public Obstacle GetObstacle()
    {
        _disabledObstacles.Clear();

        for(int i = 0; i < _pool.Count; i++)
        {
            if (_pool[i].gameObject.activeSelf == false)
               _disabledObstacles.Add(_pool[i]);
        }

        int randomIndex;

        if (_disabledObstacles.Count > 0)
        {
            randomIndex = Random.Range(0, _disabledObstacles.Count);
            
            return _disabledObstacles[randomIndex];
        }

        randomIndex = Random.Range(0, _currentLevelProperties.Obstacles.Length);

        return Create(_currentLevelProperties.Obstacles[randomIndex]);
    }

    private Obstacle Create(Obstacle template)
    {
        Obstacle instance = Instantiate(template, _container);

        instance.gameObject.SetActive(false);
        _pool.Add(instance);

        return instance;
    }

    public void ResetState()
    {
        for (int i = 0; i < _container.childCount; i++)
            Destroy(_container.GetChild(i).gameObject);

        _disabledObstacles.Clear();
        _pool.Clear();
    }
}