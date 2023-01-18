using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    [SerializeField] private ItemSpawner _itemSpawner;
    [SerializeField] private FrameSpawner _frameSpawner;
    [SerializeField] private ObstacleSpawner _obstacleSpawner;
    [SerializeField] private uint _initialFramesCount;
    [SerializeField] private Portal _portal;
    [SerializeField] private RobotMovement _robotMovement;
    [SerializeField] private FrameEndHider _frameEndHider;
    [SerializeField] private LevelProperties[] _levelProperties;

    private LevelProperties _currentLevelProperties;
    private uint _currentLevel = 1;
    private bool _isLevelAllFrameSpawned;
    private Dictionary<uint, LevelProperties> _levelPropertiesPair = new Dictionary<uint, LevelProperties>();

    public bool IsLastLevel => _currentLevel == LevelCount;
    public int LevelCount => _levelProperties.Length;
    public LevelProperties CurrentLevelProperties => _currentLevelProperties;
    public IReadOnlyDictionary<uint, LevelProperties> LevelPropertiesPair => _levelPropertiesPair;

    public event UnityAction<LevelProperties> LevelChanged;

    public void Init(uint level, uint startPosition)
    {
        _levelPropertiesPair.Clear();
        _currentLevel = level;

        for (int i = 0; i < _levelProperties.Length; i++)
            _levelPropertiesPair.Add(_levelProperties[i].Level, _levelProperties[i]);

        _currentLevelProperties = _levelPropertiesPair[_currentLevel];

        _frameSpawner.Init(_currentLevelProperties, _initialFramesCount, startPosition);
        _itemSpawner.Init();
        _obstacleSpawner.Init(_currentLevelProperties);

        _frameEndHider.Enable();
        SpawnStartFrames();
    }

    public void FrameSpawnTriggerReached()
    {
        if (_frameSpawner.LastFramePosition.z >= _currentLevelProperties.CoveredLevelsLength && _currentLevel < LevelCount)
        {
            _isLevelAllFrameSpawned = true;
            _portal.Init();
            _frameEndHider.Disable();
        }

        if (_isLevelAllFrameSpawned)
            return;

        _frameSpawner.ReleaseFirst(_initialFramesCount);
        _obstacleSpawner.ReleaseFirst(_initialFramesCount);
        _frameSpawner.Spawn();
        SpawnObstacle();
    }

    public void OnLevelFinished()
    {
        _frameSpawner.Dispose();
        _obstacleSpawner.Dispose();
    }

    public void TurnNextLevel()
    {
        _currentLevel++;

        _currentLevelProperties = _levelPropertiesPair[_currentLevel];
        _isLevelAllFrameSpawned = false;
        _currentLevel = _currentLevelProperties.Level;

        LevelChanged?.Invoke(_currentLevelProperties);
    }

    private void SpawnStartFrames()
    {
        for (int i = 0; i < _initialFramesCount; i++)
        {
            _frameSpawner.Spawn();

            if (_frameSpawner.SpawnedFrames.Count > 1)
                SpawnObstacle();
        }
    }

    private void SpawnObstacle()
    {
        float obstacleSpace = Random.Range(_currentLevelProperties.MinObstacleSpace, _currentLevelProperties.MaxObstacleSpace);
        float obstaclePositionZ = _frameSpawner.LastFrameOriginZ;
        Vector3 obstaclePosition;

        for (int i = 0; i < _currentLevelProperties.ObstacleCountPerFrame; i++)
        {
            if (i > 0)
                obstaclePositionZ += obstacleSpace;

            obstaclePosition = new Vector3(0, 0, obstaclePositionZ);
            _obstacleSpawner.Spawn(obstaclePosition);
        }
    }
}