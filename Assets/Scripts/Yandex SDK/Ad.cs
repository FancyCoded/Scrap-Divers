using UnityEngine;
using Agava.YandexGames;
using System;

public class Ad : MonoBehaviour
{
    [SerializeField] private GeneralAudioActivityToggler _generalAudio;
    [SerializeField] private Pause _pause;

    private bool _isRewarded = false;

    private Action _onInterstitialAdEnded;
    private Action<bool> _onVideoAdEnded;

    public void ShowInterstitialAd(Action onInterstitialAdEndAction)
    {
        _onInterstitialAdEnded = onInterstitialAdEndAction;

#if !UNITY_WEBGL || UNITY_EDITOR
        _onInterstitialAdEnded?.Invoke();
        return;
#endif

        InterstitialAd.Show(OnInterstitialAdOpened, OnInterstitialAdClosed, OnInterstitialAdError, OnInterstitialAdOffline);
    }

    public void ShowVideoAd(Action<bool> onVideoAdEndAction)
    {
        _onVideoAdEnded = onVideoAdEndAction;

#if !UNITY_WEBGL || UNITY_EDITOR
        _onVideoAdEnded?.Invoke(_isRewarded);
        return;
#endif

        VideoAd.Show(OnVideoAdOpened, OnVideoAdRewarded, OnVideoAdClosed, VideoAdError);
    }

    private void OnInterstitialAdOpened()
    {
        _generalAudio.Mute();
        _pause.PauseGame();
    }

    private void OnInterstitialAdClosed(bool value)
    {
        _pause.ResumeGame();
        _onInterstitialAdEnded?.Invoke();
        _generalAudio.Play();
    }

    private void OnInterstitialAdError(string error)
    {
        _onInterstitialAdEnded?.Invoke();
    }

    private void OnInterstitialAdOffline()
    {
        _onInterstitialAdEnded?.Invoke();
    }

    private void OnVideoAdOpened()
    {
        _pause.PauseGame();
        _isRewarded = false;
        _generalAudio.Mute();
    }

    private void OnVideoAdClosed()
    {
        _pause.ResumeGame();
        _onVideoAdEnded?.Invoke(_isRewarded);
        _generalAudio.Play();
    }

    private void OnVideoAdRewarded()
    {
        _isRewarded = true;
    }

    private void VideoAdError(string error)
    {
        _onVideoAdEnded?.Invoke(_isRewarded);
    }
}
