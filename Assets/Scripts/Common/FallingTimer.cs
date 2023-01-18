using System;

public class FallingTimer
{
    private bool _shouldRecord = true;

    public float Time { get; private set; } = 0;

    public event Action TimeChanged;

    public void Update(float deltaTime)
    {
        if (_shouldRecord == false)
            return;

        Time += deltaTime;
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