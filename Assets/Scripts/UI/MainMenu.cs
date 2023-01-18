using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private const string GameScene = "Game";

    [SerializeField] private Button _play;
    [SerializeField] private Button _checkPointMap;
    [SerializeField] private Button _achievementMap;
    [SerializeField] private CheckPointMapView _checkPointMapView;
    [SerializeField] private AchievementWindow _achievementWindow;
    [SerializeField] private MenuStorageComposition _storageComposition;
    [SerializeField] private AudioSource _audio;
    [SerializeField] private GeneralAudioActivityToggler _generalAudio;

    private void OnEnable()
    {
        _play.onClick.AddListener(OnPlayButtonClicked);
        _achievementMap.onClick.AddListener(OnAchevementMapButtonClicked);
        _checkPointMap.onClick.AddListener(OnCheckPointMapButtonClicked);
        _generalAudio.Toggled += OnAudioAcitivityToggled;
    }

    private void OnDisable()
    {
        _play.onClick.RemoveListener(OnPlayButtonClicked);
        _checkPointMap.onClick.RemoveListener(OnCheckPointMapButtonClicked);
        _achievementMap.onClick.RemoveListener(OnAchevementMapButtonClicked);
        _generalAudio.Toggled -= OnAudioAcitivityToggled;
    }

    private void OnAchevementMapButtonClicked()
    {
        _achievementWindow.Display();
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
