using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : MonoSingleton<SoundManager>
{
    private AudioSource _audioSource;

    
    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

    public GameObject soundObj;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 0 : Credit
    /// 1 : MainTheme
    /// </summary>
    public void BGMChange(int index, bool fade = false, float duration = 0)
    {
        if (fade)
        {
            _audioSource.DOFade(0, duration).OnComplete(() =>
            {
                _audioSource.clip = bgmClips[index];
                _audioSource.Play();
                _audioSource.DOFade(1, duration);
            });
            return;
        }
        _audioSource.clip = bgmClips[index];
        if (_audioSource.clip != null)
            _audioSource.Play();
    }

    public void BGMVolumeDown(float volume, bool fade = true, float duration = 0)
    {
        if(fade)
        {
            _audioSource.DOFade(volume, duration);
            return;
        }
        _audioSource.volume = volume;
    }

    /// <summary>
    /// 0 : click
    /// 1 : PowerUp
    /// 2 ~ 4 : Laser
    /// 5 : GeoDeath
    /// 6 : UnderDeath
    /// 7 ~ 11 : Glass  
    /// 12 : Slam
    /// 13 : Swipe
    /// 14 : Hit
    /// </summary>
    public void PlaySFX(int id, bool randomPitch = false)
    {
        AudioSource audio = PoolManager.Get(soundObj).GetComponent<AudioSource>();
        audio.clip = sfxClips[id];
        if (randomPitch)
            audio.pitch = Random.Range(0.7f, 1.4f);
        audio.Play();
    }

    public void PlaySFX(AudioClip clip, bool randomPitch = false, float volume = 0.778f)
    {
        AudioSource audio = PoolManager.Get(soundObj).GetComponent<AudioSource>();
        audio.clip = clip;
        audio.volume = volume;
        if (randomPitch)
            audio.pitch = Random.Range(0.7f, 1.4f);
        audio.Play();
    }

}
