using UnityEngine;

[CreateAssetMenu(menuName ="Level props", order = 51)]
public class LevelProperites : ScriptableObject
{
    [SerializeField] private uint _level;
    [SerializeField] private uint _levelStartPositionZ;
    [SerializeField] private uint _levelLenght;
    [SerializeField] private float _minObstacleSpace = 15;
    [SerializeField] private float _maxObstacleSpace = 25;
    [SerializeField] private uint _obstacleCountPerFrame = 4;

    [SerializeField] private Frame _frame;
    [SerializeField] private Obstacle[] _obstacles;

    LevelConfig _levelConfig = new LevelConfig();

    public uint Level => _level;
    public uint LevelStartPositionZ => _levelStartPositionZ * _levelConfig.LevelMeterFactor;
    public uint LevelLength => LevelStartPositionZ + _levelLenght * _levelConfig.LevelMeterFactor;
    public float MinObstacleSpace => _minObstacleSpace;
    public float MaxObstacleSpace => _maxObstacleSpace;
    public uint ObstacleCountPerFrame => _obstacleCountPerFrame;
    public Obstacle[] Obstacles => _obstacles;
    public Frame Frame => _frame;
}
