using UnityEngine;

[CreateAssetMenu(menuName ="Level props", order = 51)]
public class LevelProperties : ScriptableObject
{
    [SerializeField] private uint _level;
    [SerializeField] private uint _levelStartPositionZ;
    [SerializeField] private uint _levelLenght;
    [SerializeField] private float _minObstacleSpace = 15;
    [SerializeField] private float _maxObstacleSpace = 25;
    [SerializeField] private uint _obstacleCountPerFrame = 4;
    [SerializeField] private uint _maxNutInRow = 4;
    [SerializeField] private uint _minNutInRow = 1;
    [SerializeField] private int _nutSpace = 2;
    [SerializeField] private float _buffSpawnChanceMax;

    [SerializeField] private Frame _frame;
    [SerializeField] private Obstacle[] _obstacles;

    LevelConfig _levelConfig = new LevelConfig();

    public uint Level => _level;
    public uint LevelStartPositionZ => _levelStartPositionZ * _levelConfig.LevelMeterFactor;
    public uint LevelLength => LevelStartPositionZ + _levelLenght * _levelConfig.LevelMeterFactor;
    public float MinObstacleSpace => _minObstacleSpace;
    public float MaxObstacleSpace => _maxObstacleSpace;
    public uint ObstacleCountPerFrame => _obstacleCountPerFrame;
    public uint MaxNutInRow => _maxNutInRow;
    public uint MinNutInRow => _minNutInRow;
    public int NutSpace => _nutSpace;
    public float BuffSpawnChanceMax => _buffSpawnChanceMax;
    public Obstacle[] Obstacles => _obstacles;
    public Frame Frame => _frame;
}
