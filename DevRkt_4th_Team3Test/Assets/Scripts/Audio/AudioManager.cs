using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;

    [Header("BGM Clips")]
    [SerializeField] private List<AudioClip> _bgmClips;
    private int _currentIndex = 0;
    
    [Header("SFX Clips")]
    [SerializeField] private AudioClip _hitEnemy;

    [Header("UI Clips")]
    [SerializeField] private AudioClip _menuSelect;
    [SerializeField] private AudioClip _startSfx;
    [SerializeField] private AudioClip _clearStage;
    [SerializeField] private AudioClip _failStage;
    
    [Header("Mixer")]
    [SerializeField] private AudioMixer _mixer;

    void Awake()
    {
        SingletonInit();
    }

    void Start() {
        if (_bgmClips != null && _bgmClips.Count > 0 && _bgmSource != null)
        {
            PlayBGM(_currentIndex);
            StartCoroutine(BGMSequence());
        }
        if (_bgmClips != null && _bgmClips.Count > 0 && _bgmSource != null)
        {
            PlayBGM(_currentIndex);
            StartCoroutine(BGMSequence());
            SetBGMVolume(PlayerPrefs.GetFloat(SettingPopupUI.BGM_SAVE_KEY, 0.75f));
            SetSFXVolume(PlayerPrefs.GetFloat(SettingPopupUI.SFX_SAVE_KEY, 0.75f));
        }
    }

    public void PlayBGM(int index) {
        if (index < 0 || index >= _bgmClips.Count) return;

        _currentIndex = index;
        _bgmSource.clip = _bgmClips[_currentIndex];
        _bgmSource.loop = false; 
        _bgmSource.Play();

    }
    private IEnumerator BGMSequence()
    {
        while (true)
        {
            // 현재 곡이 끝날 때까지 대기
            yield return new WaitForSeconds(_bgmSource.clip.length);

            // 다음 곡으로 이동
            _currentIndex = (_currentIndex + 1) % _bgmClips.Count;
            PlayBGM(_currentIndex);
        }
    }

    public void PlayHitSFX() {
        if (_hitEnemy != null)
            _sfxSource.PlayOneShot(_hitEnemy);
    }
    public void PlayMenuSelectSFX()
    {
        if (_menuSelect != null)
            _sfxSource.PlayOneShot(_menuSelect);
    }
    public void PlayStartSFX()
    {
        if (_startSfx != null)
            _sfxSource.PlayOneShot(_startSfx);
    }
    public void PlayClearSFX()
    {
        if (_clearStage !=null)
            _sfxSource.PlayOneShot(_clearStage);
    }
    public void PlayFailSFX()
    {
        if (_failStage !=null)
            _sfxSource.PlayOneShot(_failStage);
    }

    public void SetBGMVolume(float value) {
        if (value <= 0f)
        {
            _mixer.SetFloat("BGMVolume", -80f); 
        }
        else
        {
            _mixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20);
        }
    }

    public void SetSFXVolume(float value) {
        if (value <= 0f)
        {
            _mixer.SetFloat("SFXVolume", -80f); 
        }
        else
        {
            _mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        }
    }
}