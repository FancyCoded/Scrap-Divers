using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private const string GameScene = "Game";

    [SerializeField] private Button _play;
    [SerializeField] private Button _checkPointMap;
    [SerializeField] private CheckPointMapView _checkPointMapView;
    [SerializeField] private MenuStorageComposition _storageComposition;
    [SerializeField] private AudioSource _audio;
    [SerializeField] private GeneralAudioActivityToggler _generalAudio;

    private void OnEnable()
    {
        _play.onClick.AddListener(OnPlayButtonClicked);
        _checkPointMap.onClick.AddListener(OnCheckPointMapButtonClicked);
        _generalAudio.Toggled += OnAudioAcitivityToggled;
    }

    private void OnDisable()
    {
        _play.onClick.RemoveListener(OnPlayButtonClicked);
        _checkPointMap.onClick.RemoveListener(OnCheckPointMapButtonClicked);
        _generalAudio.Toggled -= OnAudioAcitivityToggled;
    }

    private void OnPlayButtonClicked()
    {
        _storageComposition.Save();
        SceneManager.LoadScene(GameScene);
    }

    private void OnCheckPointMapButtonClicked()
    {
        _checkPointMapView.Display();
    }

    private void OnAudioAcitivityToggled(bool isMuted)
    {
        if (isMuted)
            _audio.Pause();
        else
            _audio.Play();
    }
}
