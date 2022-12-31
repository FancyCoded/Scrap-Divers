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
    [SerializeField] private LevelProperties[] _levelProperites;

    private LevelProperties _currentLevelProperties;
    private uint _currentLevel = 1;
    private bool _isLevelAllFrameSpawned;

    public bool IsLastLevel => _currentLevel == LevelCount;
    public int LevelCount => _levelProperites.Length;
    public LevelProperties CurrentLevelProperties => _currentLevelProperties;

    public event UnityAction<LevelProperties> LevelChanged;

    public void Init(uint level, uint startPosition)
    {
        _currentLevel = level;

        for (int i = 0; i < _levelProperites.Length; i++)
        {
            if (_currentLevel != _levelProperites[i].Level)
                continue;

            _currentLevelProperties = _levelProperites[i];
            break;
        }

        _frameSpawner.Init(_currentLevelProperties, _initialFramesCount, startPosition);
        _itemSpawner.Init();
        _obstacleSpawner.Init(_currentLevelProperties);

        _frameEndHider.Enable();
        SpawnStartFrames();
    }

    public void FrameSpawnTriggerReached()
    {
        if (_frameSpawner.LastFramePosition.z >= _currentLevelProperties.LevelLength && _currentLevel < LevelCount)
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

        for (int i = 0; i < _levelProperites.Length; i++)
        {
            if (_levelProperites[i].Level == _currentLevel)
            {
                _currentLevelProperties = _levelProperites[i];
                break;
            }
        }

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