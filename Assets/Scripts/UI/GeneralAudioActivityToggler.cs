using UnityEngine;
using UnityEngine.UI;

public class GeneralAudioActivityToggler : MonoBehaviour
{
    [SerializeField] private Sprite _mute;
    [SerializeField] private Sprite _volume;
    [SerializeField] private Button _toggle;
    [SerializeField] private StorageComposition _storageComposition;

    public bool IsMuted => AudioListener.pause;

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
        AudioListener.pause = false;
        _toggle.image.sprite = _volume;
    }

    public void Mute()
    {
        AudioListener.pause = true;
        _toggle.image.sprite = _mute;
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
