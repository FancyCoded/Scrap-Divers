using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    private const string MenuScene = "Menu";

    [SerializeField] private Button _pause;
    [SerializeField] private Button _resume;
    [SerializeField] private Button _menu;
    [SerializeField] private Transform _pauseWindow;

    public event UnityAction Paused;
    public event UnityAction Resumed;

    private void Awake()
    {
        _pauseWindow.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _pause.onClick.AddListener(OnPauseButtonClicked);
        _resume.onClick.AddListener(OnResumeButtonClicked);
        _menu.onClick.AddListener(OnMenuButtonClicked);
    }

    private void OnDisable()
    {
        _pause.onClick.RemoveListener(OnPauseButtonClicked);
        _resume.onClick.RemoveListener(OnResumeButtonClicked);
        _menu.onClick.RemoveListener(OnMenuButtonClicked);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    private void OnPauseButtonClicked()
    {
        _pauseWindow.gameObject.SetActive(true);
        PauseGame();
        Paused?.Invoke();
    }

    private void OnResumeButtonClicked()
    {
        _pauseWindow.gameObject.SetActive(false);
        ResumeGame();
        Resumed?.Invoke();
    }

    private void OnMenuButtonClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(MenuScene);
    }
}
