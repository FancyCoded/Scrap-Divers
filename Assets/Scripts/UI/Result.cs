using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    private const string MenuScene = "Menu";

    [SerializeField] private Button _restart;
    [SerializeField] private Button _menu;
    [SerializeField] private TMP_Text _distanceText;
    [SerializeField] private TMP_Text _bestDistanceText;
    [SerializeField] private TMP_Text _collectedNutsText;
    [SerializeField] private TMP_Text _bestCollectedNutsText;
    [SerializeField] private TMP_Text _fallingTime;
    [SerializeField] private TMP_Text _bestFallingTime;
    [SerializeField] private StorageComposition _storageComposition;

    private Storage _storage => _storageComposition.Storage;

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

    public void Display()
    {
        gameObject.SetActive(true);

        _collectedNutsText.text = $"{_storage.Score.NutCount}";
        _bestCollectedNutsText.text = $"{_storage.BestCollectedNuts}";
        _distanceText.text = $"{_storage.Score.Distance}m";
        _bestDistanceText.text = $"{_storage.BestDistance}m";
        _fallingTime.text = $"{_storage.Score.FallingTimer.Time}s";
        _bestFallingTime.text = $"{_storage.BestFallingTime}s";
    }

    private void OnMenuButtonClicked()
    {
        SceneManager.LoadScene(MenuScene);
    }

    private void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
