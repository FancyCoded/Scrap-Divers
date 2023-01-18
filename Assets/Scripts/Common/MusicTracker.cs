using System.Collections;
using UnityEngine;

public class MusicTracker : MonoBehaviour
{
    private const float _audioMaxVolume = 0.5f;
    private const float _audioMinVolume = 0.05f;

    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip[] _clips;
    [SerializeField] private Game _game;
    [SerializeField] private Robot _robot;
    [SerializeField] private Revival _revival;
    [SerializeField] private Pause _pause;

    private IEnumerator _playClips;
    private bool _isPlaying = true;

    private void Awake()
    {
        _audio.volume = _audioMaxVolume;
        Play();
    }

    private void OnEnable()
    {
        _game.GameEnded += TurnDownTheVolume;
        _revival.ReviveButtonClicked += TurnUpTheVolume;
        _pause.Paused += TurnDownTheVolume;
        _pause.Resumed += TurnUpTheVolume;
    }

    private void OnDisable()
    {
        _game.GameEnded -= TurnDownTheVolume;
        _revival.ReviveButtonClicked -= TurnUpTheVolume;
        _pause.Paused -= TurnDownTheVolume;
        _pause.Resumed -= TurnUpTheVolume;
    }

    private void Play()
    {
        if (_playClips != null)
            StopCoroutine(_playClips);

        _playClips = PlayClips(_clips);
        StartCoroutine(_playClips);
    }

    private void TurnUpTheVolume()
    {
        _audio.volume = _audioMaxVolume;
    }

    private void TurnDownTheVolume()
    {
        _audio.volume = _audioMinVolume;
    }

    private IEnumerator PlayClips(AudioClip[] clips)
    {
        while (_isPlaying)
        {
            WaitForSeconds duration;

            int randomIndex = Random.Range(1, clips.Length);
            _audio.clip = clips[randomIndex];
            duration = new WaitForSeconds(_audio.clip.length);

            _audio.Play();

            clips[randomIndex] = clips[0];
            clips[0] = _audio.clip;

            yield return duration;
        }
    }
}
