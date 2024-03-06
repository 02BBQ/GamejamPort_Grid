using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSetting : MonoSingleton<SoundSetting>
{
    public AudioMixer audioMixer;
    [SerializeField]
    Slider _masterVolumeSlider;
    [SerializeField]
    Slider _bgmVolumeSlider;
    [SerializeField]
    Slider _sfxVolumeSlider;

    [SerializeField]
    GameObject _panel;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Open()
    {
        _panel.SetActive(true);
    }

    public void Close()
    {
        _panel.SetActive(false);
    }

    public void MasterControl()
    {
        float sound = _masterVolumeSlider.value;
        if (sound <= -50) sound = -80;
        audioMixer.SetFloat("Master", sound);
    }

    public void BGMControl()
    {
        float sound = _bgmVolumeSlider.value;
        if (sound <= -50) sound = -80;
        audioMixer.SetFloat("BGM", sound);
    }

    public void SFXControl()
    {
        float sound = _sfxVolumeSlider.value;
        if (sound <= -50) sound = -80;
        audioMixer.SetFloat("SFX", sound);
    }
}
