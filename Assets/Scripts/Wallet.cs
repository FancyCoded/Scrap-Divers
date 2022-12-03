using System;

public class Wallet
{
    private uint _nutCount = 0;

    public uint NutCount => _nutCount;

    public event Action<uint> NutCountChanged;

    public Wallet(uint nutCount = 0)
    {
        Init(nutCount);
    }

    public void Init(uint nutCount)
    {
        _nutCount = nutCount;
        NutCountChanged?.Invoke(_nutCount);
    }

    public void Reduce(uint nutCount)
    {
        _nutCount -= nutCount;
        NutCountChanged?.Invoke(_nutCount);
    }

    public void Increase(uint nutCount)
    {
        _nutCount += nutCount;
        NutCountChanged?.Invoke(_nutCount);
    }
}
