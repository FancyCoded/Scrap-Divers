using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Revival : MonoBehaviour
{
    [SerializeField] private Button _forward;
    [SerializeField] private Button _revive;
    [SerializeField] private Result _result;
    [SerializeField] private Slider _countdownSlider;
    [SerializeField] private float _countdown;
    [SerializeField] private TMP_Text _revivePriceText;
    [SerializeField] private GameStorageComposition _storageComposition;
    [SerializeField] private ScoreView _scoreView;
    [SerializeField] private NutCountView _nutCountView;
    [SerializeField] private WalletView _walletView;

    private uint _reviveNumber = 0;
    private uint _reviveMaxCount = 3;
    private uint _revivePrice = 20;
    private uint _revivePriceIncrement = 20;
    private IEnumerator _startCountDown;
    private Storage _storage => _storageComposition.Storage;
    private Wallet _wallet => _storage.Wallet;
    private Score _score => _storage.Score;

    public uint ReviveNumber => _reviveNumber;
    public uint ReviveMaxCount => _reviveMaxCount;


    public event UnityAction ReviveButtonClicked;

    private void OnEnable()
    {
        _forward.onClick.AddListener(OnForwardButtonClicked);
        _revive.onClick.AddListener(OnReviveButtonClicked);
    }

    private void OnDisable()
    {
        _forward.onClick.RemoveListener(OnForwardButtonClicked);
        _revive.onClick.RemoveListener(OnReviveButtonClicked);
    }

    public void Display()
    {
        _scoreView.Hide();
        _nutCountView.Hide();
        gameObject.SetActive(true);

        if (_score.NutCount > _storage.BestCollectedNuts)
        _storage.SaveBestCollectedNuts();

        if (_score.Distance > _storage.BestDistance)
            _storage.SaveBestDistance();

        _wallet.Increase(_score.NutCount);
        _walletView.OnNutCountChanged(_wallet.NutCount);
        _storageComposition.Storage.Save();

        _revivePriceText.text = _revivePrice.ToString();
        gameObject.SetActive(true);

        if (_startCountDown != null)
            StopCoroutine(_startCountDown);

        _startCountDown = StartCountDown(_countdown);
        StartCoroutine(_startCountDown);
    }

    private void OnReviveButtonClicked()
    {
        if(_wallet.NutCount < _revivePrice)
            return;

        _scoreView.Display();
        _nutCountView.Display();

        _wallet.Reduce(_revivePrice);

        _reviveNumber++;
        _revivePrice += _revivePriceIncrement;

        ReviveButtonClicked?.Invoke();
    }

    private void OnForwardButtonClicked()
    {
        GoForward();
    }

    private IEnumerator StartCountDown(float duration)
    {
        _countdownSlider.value = duration;

        float step = 0.001f;
        
        while(_countdownSlider.value > 0)
        {
            _countdownSlider.value -= step;
            yield return null;
        }

        GoForward();
    }

    private void GoForward()
    {
        gameObject.SetActive(false);
        _result.Display();
    }
}
