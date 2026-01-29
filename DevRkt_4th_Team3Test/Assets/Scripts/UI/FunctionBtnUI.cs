using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FunctionBtnManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingPopup;
    private GameObject _currentSettingPopup;
    [SerializeField] private GameObject _pauseAlert;
    private GameObject _currentAlertInstance;
    
    private bool _isPaused = false;
    private DataController _dataController;
    
    /// <summary>
    /// 게임 정지 기능
    /// </summary>
    public void OnClickPause()
    {
        _isPaused = !_isPaused;

        if (_isPaused)
        {
            Time.timeScale = 0f;
            //얼럿 프리팹 생성해서 할당
            if (_currentAlertInstance == null)
            {
                _currentAlertInstance = Instantiate(_pauseAlert, GameObject.Find("Canvas").transform);
            }
            else _currentAlertInstance.SetActive(true);
            //팝업 프리팹 생성해서 할당
            if (_currentSettingPopup == null)
            {
                _currentSettingPopup = Instantiate(_settingPopup, GameObject.Find("Canvas").transform);
            }
            else _currentSettingPopup.SetActive(true);

        }
        else
        {
            ResumeGame();
        }
    }
    
    public void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        if (_pauseAlert != null) _currentAlertInstance.SetActive(false);
    }
    
    /// <summary>
    /// 설정 팝업 열기
    /// </summary>
    public void OnClickSetting()
    {
        if (_settingPopup != null)
        {
            _settingPopup.SetActive(!_settingPopup.activeSelf);
        }
    }
    
    /// <summary>
    /// 게임 종료 기능
    /// </summary>
    public void OnClickExit()
    {
        //_dataController.Save();
        SceneManager.LoadScene(2);
    }
}
