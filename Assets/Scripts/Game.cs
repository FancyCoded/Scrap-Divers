using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    [SerializeField] private Robot _robot;
    [SerializeField] private Revival _revival;
    [SerializeField] private GameEnd _gameEnd;

    public event UnityAction GameEnded;

    private void OnEnable()
    {
        _revival.ReviveButtonClicked += ReviveGame;
        _robot.Body.Died += OnPlayerDied;
    }

    private void OnDisable()
    {
        _revival.ReviveButtonClicked -= ReviveGame;
        _robot.Body.Died -= OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        GameEnded?.Invoke();
    }

    private void ReviveGame()
    {
        _gameEnd.Hide();
        _robot.Revive();
    }
}
