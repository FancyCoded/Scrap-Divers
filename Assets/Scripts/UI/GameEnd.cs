using UnityEngine;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private Revival _revival;
    [SerializeField] private Result _result;
    [SerializeField] private Robot _robot;

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
        if (_revival.ReviveNumber < _revival.ReviveMaxCount)
            _revival.Display();
        else
            _result.Display();
    }
}
