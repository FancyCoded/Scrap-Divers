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
        _bestCollectedNutsText.text = $"best: {_storage.BestCollectedNuts}";
        _distanceText.text = $"{_storage.Score.Distance}m";
        _bestDistanceText.text = $"best: {_storage.BestDistance}m";
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
