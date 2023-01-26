using System.Collections.Generic;
using UnityEngine;

public class GameStorageComposition : StorageComposition, IResetable
{
    [SerializeField] private List<CheckPointProperty> _checkPointPropertiesDefault;
    [SerializeField] private List<AchievementProperties> _achievementPropertiesDefault;
    [SerializeField] private Level _level;
    [SerializeField] private Result _result;
    [SerializeField] private Portal _portal;
    [SerializeField] private Robot _robot;
    [SerializeField] private RobotMovement _robotMovement;
    [SerializeField] private Collector _collector;
    [SerializeField] private ScoreView _scoreView;
    [SerializeField] private GeneralAudioActivityToggler _generalAudioActivityToggler;
    [SerializeField] private Language _language;

    private readonly Wallet _wallet = new Wallet(0);
    private readonly CheckPointMap _checkPointMap = new CheckPointMap();
    private readonly AchievementMap _achievementMap = new AchievementMap();
    private readonly Score _score = new Score();
    private readonly LevelConfig _levelConfig = new LevelConfig();
    private readonly FallingTimer _fallingTimer = new FallingTimer();
    private AchievementFactory _achievementFactory;
    private PlayerInputRouter _input;

    private void Awake()
    {
        _input = new PlayerInputRouter();
        _achievementFactory = new AchievementFactory(_collector, _fallingTimer, _robot.Body, _level, Storage);

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
        _robot.Body.Died += _fallingTimer.StopRecord;
        _robot.Revived += _fallingTimer.Record;
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
        _robot.Body.Died -= _fallingTimer.StopRecord;
        _robot.Revived -= _fallingTimer.Record;
        _robotMovement.SpeedChanged -= _score.OnSpeedChanged;
        _collector.NutCountChanged -= _score.OnNutCountChanged;

        _input.Disable();
    }

    private void Start()
    {
        Compose();
    }

    private void FixedUpdate()
    {
        _input.Update();
        _score.Update(Time.deltaTime);
        _fallingTimer.Update(Time.deltaTime);
    }

    public override void Compose()
    {
        Storage.Init(_wallet, _checkPointMap, _score, _achievementMap, _checkPointPropertiesDefault, 
            _achievementPropertiesDefault, _generalAudioActivityToggler, _achievementFactory, _language);
        Storage.Load();

        uint startPosition = Storage.CheckPointMap.CurrentCheckPointProperty.Distance * _levelConfig.LevelMeterFactor;
        uint level = Storage.CheckPointMap.CurrentCheckPointProperty.Level;

        InitLevel(level, startPosition, _input);
    }

    private void OnLevelChanged(LevelProperties levelProperties)
    {
        InitLevel(levelProperties.Level, levelProperties.LevelStartPositionZ, _input);
    }

    private void InitLevel(uint level, uint startPosition, PlayerInputRouter input)
    {
        _input = input;
        _level.Init(level, startPosition);
        _robotMovement.Init(_level.CurrentLevelProperties, startPosition, _input);
        _score.Init(_fallingTimer, _level.CurrentLevelProperties.CoveredLevelsLength, startPosition, _level.IsLastLevel);
        _scoreView.Init(_score);
    }

    [ContextMenu("Save")]
    public void Save()
    {
        Storage.Save();
    }

    [ContextMenu("Reset")]
    public void ResetState()
    {
        Storage.ResetState();
    }
}