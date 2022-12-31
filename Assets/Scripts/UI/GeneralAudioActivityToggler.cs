using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GeneralAudioActivityToggler : MonoBehaviour
{
    [SerializeField] private Sprite _mute;
    [SerializeField] private Sprite _volume;
    [SerializeField] private Button _toggle;
    [SerializeField] private StorageComposition _storageComposition;

    public bool IsMuted => AudioListener.volume == 0;

    public event UnityAction<bool> Toggled;

    private void OnEnable()
    {
        _toggle.onClick.AddListener(OnToggleButtonClicked);
    }

    private void OnDisable()
    {
        _toggle.onClick.RemoveListener(OnToggleButtonClicked);
    }

    public void Init(bool isMuted)
    {
        if (isMuted)
            Mute();
        else
            Play();
    }

    public void Play()
    {
        AudioListener.volume = 1;
        _toggle.image.sprite = _volume;
        Toggled?.Invoke(IsMuted);
    }

    public void Mute()
    {
        AudioListener.volume = 0;
        _toggle.image.sprite = _mute;
        Toggled?.Invoke(IsMuted);
    }

    private void OnToggleButtonClicked()
    {
        if (IsMuted)
            Play();
        else
            Mute();

        _storageComposition.Storage.Save();
    }
}
