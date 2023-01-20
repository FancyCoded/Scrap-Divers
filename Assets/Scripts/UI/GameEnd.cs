using UnityEngine;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private Revival _revival;
    [SerializeField] private Result _result;
    [SerializeField] private Robot _robot;
    [SerializeField] private GameStorageComposition _storageComposition;
    [SerializeField] private Ad _ad;

    private uint _adViewedCount => _storageComposition.Storage.InterstitialAdViewedCount;

    private void Awake()
    {
        Hide();
    }

    private void OnEnable()
    {
        _robot.Body.Died += OnGameEnd;
    }

    private void OnDisable()
    {
        _robot.Body.Died -= OnGameEnd;
    }

    public void Hide()
    {
        _revival.gameObject.SetActive(false);
        _result.gameObject.SetActive(false);
    }

    private void OnGameEnd()
    {
        if(_adViewedCount % 2 == 0)
        {
            _storageComposition.Storage.SetAdViewedCount(_adViewedCount + 1);
            _storageComposition.Save();
            _ad.ShowInterstitialAd(OnGameEnd);
        }
        
        if (_revival.ReviveNumber < _revival.ReviveMaxCount)
            _revival.Display();
        else
            _result.Display();
    }
}
