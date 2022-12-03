using UnityEngine;

public class Score
{
    private LevelConfig _config = new LevelConfig();
    private Vector3 _targetPosition;
    private Vector3 _position = Vector3.zero;
    private float _targetSpeed = 0;
    private uint _nutCount = 0;
    private uint _levelLength;
    private bool _shouldRecord = true;
    private bool _isLastLevel = false;

    public uint Distance => (uint)(_position.z * _config.ScoreMeterFactor);
    public bool ShouldRecord => _shouldRecord;
    public uint NutCount => _nutCount;

    public void Init(uint levelLength, float startPositionZ, bool isLastLevel)
    {
        _position.z = startPositionZ;
        _levelLength = levelLength;
        _isLastLevel = isLastLevel;
    }

    public void Update(float deltaTime)
    {
        if (_shouldRecord == false)
            return;

        if (_position.z >= _levelLength && _isLastLevel == false)
            OnLevelFinished();

        _targetPosition = _position + Vector3.forward;
        _position = Vector3.MoveTowards(_position, _targetPosition, _targetSpeed * deltaTime);
    }

    public void Record()
    {
        _shouldRecord = true;
    }

    public void StopRecord()
    {
        _shouldRecord = false;
    }

    public void OnSpeedChanged(float speed)
    {
        _targetSpeed = speed;
    }

    public void OnLevelFinished()
    {
        StopRecord();
        _position = new Vector3(_position.x, _position.y, _levelLength);
    }

    public void OnNutCountChanged(uint nutCount)
    {
        _nutCount = nutCount;
    }
}
