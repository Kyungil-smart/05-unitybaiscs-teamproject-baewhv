using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FunctionBtnManager : MonoBehaviour
{
    [SerializeField] private GameObject _infoPopup;
    private GameObject _currentInfoPopup;
    [SerializeField] private GameObject _pauseAlert;
    private GameObject _currentAlertInstance;
    
    private bool _isPaused = false;
    private DataController _dataController;

    public void Awake()
    {
        //팝업 프리팹 생성해서 할당
        if (_currentInfoPopup == null)
        {
            // Instantiate: 게임 오브젝트를 동적으로 생성
            _currentInfoPopup = Instantiate(_infoPopup, GameObject.Find("Canvas").transform);
        }
        else _currentInfoPopup.SetActive(true);
    }

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
        if (_infoPopup != null)
        {
            _infoPopup.SetActive(!_infoPopup.activeSelf);
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
