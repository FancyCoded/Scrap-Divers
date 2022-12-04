using System.Collections.Generic;
using UnityEngine;

public class GameStorageComposition : MonoBehaviour
{
    [SerializeField] private List<CheckPointProperty> _checkPointPropertiesDefault;
    [SerializeField] private Level _level;
    [SerializeField] private Result _result;
    [SerializeField] private Portal _portal;
    [SerializeField] private Robot _robot;
    [SerializeField] private RobotMovement _robotMovement;
    [SerializeField] private Collector _collector;
    [SerializeField] private ScoreView _scoreView;

    private readonly Storage _storage = new Storage();
    private readonly Wallet _wallet = new Wallet(0);
    private readonly CheckPointMap _checkPointMap = new CheckPointMap();
    private readonly Score _score = new Score();
    LevelConfig _levelConfig = new LevelConfig();
    PlayerInputRouter _input;

    public Storage Storage => _storage;

    private void Awake()
    {
        _input = new PlayerInputRouter();
    }

    private void OnEnable()
    {
        _level.LevelChanged += OnLevelChanged;

        _portal.WarpEffect.Disabled += _level.TurnNextLevel;
        _portal.WarpEffect.Disabled += _score.Record;
        _portal.PortalReached += _score.OnLevelFinished;
        _portal.PortalReached += _level.OnLevelFinished;
        _robot.Body.Died += _score.StopRecord;
        _robot.Revived += _score.Record;
        _robotMovement.SpeedChanged += _score.OnSpeedChanged;
        _collector.NutCountChanged += _score.OnNutCountChanged;

        _input.Enable();
    }

    private void OnDisable()
    {
        _level.LevelChanged -= OnLevelChanged;

        _portal.WarpEffect.Disabled -= _level.TurnNextLevel;
        _portal.WarpEffect.Disabled -= _score.Record;
        _portal.PortalReached -= _score.OnLevelFinished;
        _portal.PortalReached -= _level.OnLevelFinished;
        _robot.Body.Died -= _score.StopRecord;
        _robot.Revived -= _score.Record;
        _robotMovement.SpeedChanged -= _score.OnSpeedChanged;
        _collector.NutCountChanged -= _score.OnNutCountChanged;

        _input.Disable();
    }

    private void Start()
    {
        _storage.Init(_wallet, _checkPointMap, _score, _checkPointPropertiesDefault);
        _storage.Load();
        Save();

        uint startPosition = Storage.CheckPointMap.CurrentCheckPointProperty.Distance * _levelConfig.LevelMeterFactor;
        uint level = Storage.CheckPointMap.CurrentCheckPointProperty.Level;

        InitLevel(level, startPosition, _input);
    }

    private void Update()
    {
        _input.Update();        
        _score.Update(Time.deltaTime);
    }

    private void OnLevelChanged(LevelProperites levelProperties)
    {
        InitLevel(levelProperties.Level, levelProperties.LevelStartPositionZ, _input);
    }

    private void InitLevel(uint level, uint startPosition, PlayerInputRouter input)
    {
        _input = input;
        _level.Init(level, startPosition);
        _robotMovement.Init(_level.CurrentLevelProperties, startPosition, _input);
        _score.Init(_level.CurrentLevelProperties.LevelLength, startPosition, _level.IsLastLevel);
        _scoreView.Init(_score);
        _result.Init(_score, _wallet);
    }

    [ContextMenu("Save")]
    public void Save()
    {
        _storage.Save();
    }

    [ContextMenu("Reset")]
    public void ResetState()
    {
        _storage.ResetState();
    }
}