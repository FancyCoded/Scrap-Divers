using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Result : MonoBehaviour
{
    private const string MenuScene = "Menu";

    [SerializeField] private Button _restart;
    [SerializeField] private Button _menu;
    [SerializeField] private TMP_Text _distanceText;
    [SerializeField] private TMP_Text _bestDistanceText;
    [SerializeField] private TMP_Text _collectedNutsText;
    [SerializeField] private TMP_Text _bestCollectedNutsText;
    [SerializeField] private GameStorageComposition _storageCompostion;

    private Score _score;
    private Wallet _wallet;

    private void OnEnable()
    {
        _restart.onClick.AddListener(OnRestartButtonClicked);
        _menu.onClick.AddListener(OnMenuButtonClicked);
    }

    private void OnDisable()
    {
        _restart.onClick.RemoveListener(OnRestartButtonClicked);
        _menu.onClick.RemoveListener(OnMenuButtonClicked);
    }

    public void Init(Score score, Wallet wallet)
    {
        _score = score;
        _wallet = wallet;
    }

    public void Display()
    {
        gameObject.SetActive(true);
        _wallet.Increase(_score.NutCount);
        _storageCompostion.Storage.Save();

        if (_score.NutCount > _storageCompostion.Storage.BestCollectedNuts)
            _storageCompostion.Storage.SaveBestCollectedNuts();

        if (_score.Distance > _storageCompostion.Storage.BestDistance)
            _storageCompostion.Storage.SaveBestDistance();

        _collectedNutsText.text = $"{_score.NutCount}";
        _bestCollectedNutsText.text = $"best: {_storageCompostion.Storage.BestCollectedNuts}";
        _distanceText.text = $"{_score.Distance}m";
        _bestDistanceText.text = $"best: {_storageCompostion.Storage.BestDistance}m";
    }

    private void OnMenuButtonClicked()
    {
        _storageCompostion.Save();
        SceneManager.LoadScene(MenuScene);
    }

    private void OnRestartButtonClicked()
    {
        _storageCompostion.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
