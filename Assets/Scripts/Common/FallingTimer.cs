using System;

public class FallingTimer
{
    private bool _shouldRecord = true;
    private float _elapsedTime = 0;
    private uint _second = 1;

    public uint Time { get; private set; } = 0;

    public event Action TimeChanged;

    public void Update(float deltaTime)
    {
        if (_shouldRecord == false)
            return;

        _elapsedTime += deltaTime;

        if (_elapsedTime < _second)
            return;

        _elapsedTime = 0;
        Time += _second;
        TimeChanged?.Invoke();
    }

    public void StopRecord()
    {
        _shouldRecord = false;
    }

    public void Record()
    {
        _shouldRecord = true;
    }
}