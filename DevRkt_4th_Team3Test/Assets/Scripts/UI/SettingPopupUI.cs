using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingPopupUI : MonoBehaviour, IPopup
{
    public static SettingPopupUI Instance;
    
    [Header("Volume Sliders")]
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;
    
    [Header("Volume Texts")]
    [SerializeField] private TextMeshProUGUI _bgmText;
    [SerializeField] private TextMeshProUGUI _sfxText;
    
    // PlayerPrefs에 저장될 키 이름
    private const string BGM_SAVE_KEY = "BGMVolume";
    private const string SFX_SAVE_KEY = "SFXVolume";

    void Start()
    {
        // 저장된 설정을 불러오기
        float currentBGM = PlayerPrefs.GetFloat("BGMVolume", 0.75f);
        float currentSFX = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        _bgmSlider.value = currentBGM;
        _sfxSlider.value = currentSFX;
        
        //텍스트 정수로만 표시
        if (_bgmText != null) _bgmText.text = Mathf.RoundToInt(currentBGM * 100).ToString() + "%";
        if (_sfxText != null) _sfxText.text = Mathf.RoundToInt(currentSFX * 100).ToString() + "%";
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMVolume(currentBGM);
            AudioManager.Instance.SetSFXVolume(currentSFX);
        }
        
        // 슬라이더 UI에서 미리듣기
        _bgmSlider.onValueChanged.AddListener(val => {
            if (AudioManager.Instance != null) 
                AudioManager.Instance.SetBGMVolume(val);
            if (_bgmText != null) _bgmText.text = Mathf.RoundToInt(val * 100).ToString() + "%";
        });
        
        _sfxSlider.onValueChanged.AddListener(val => {
            if (AudioManager.Instance != null) 
                AudioManager.Instance.SetSFXVolume(val);
            if (_sfxText != null) _sfxText.text = Mathf.RoundToInt(val * 100).ToString() + "%";
        });
    }

    /// <summary>
    /// 설정 팝업 열기
    /// </summary>
    public void Open()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// 설정 팝업 닫기
    /// </summary>
    public void Close()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}