using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("BGM Clips")]
    [SerializeField] private AudioClip _bgmClip; // 🎵 BGM 클립 추가
    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _hitEnemy;
    

    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;

    void Awake()
    {
        SingletonInit();
    }

    void Start() {
        // 씬 시작 시 자동으로 BGM 재생
        if (_bgmClip != null) {
            PlayBGM(_bgmClip);
        }
    }

    public void PlayBGM(AudioClip clip) {
        bgmSource.clip = clip;
        bgmSource.loop = true; // 배경음악은 반복 재생
        bgmSource.Play();
    }

    public void PlayHitSFX() {
        sfxSource.PlayOneShot(_hitEnemy);
    }

    public void SetBGMVolume(float value) {
        mixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value) {
        mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }
}