public interface IReadonlyCheckPointProperty
{
    uint Distance { get; }
    uint Price { get; }
    bool IsBought { get; }
    bool IsChecked { get; }
}