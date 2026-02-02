using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FunctionBtnManager : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] private GameObject _infoPopup;
    private GameObject _currentInfoPopup;
    private InfoPopupUI  _infoPopupUI;
    [SerializeField] private GameObject _settingPopup;
    private GameObject _currentSettingPopup;
    private SettingPopupUI  _settingPopupUI;
    [SerializeField] private GameObject _exitPopup;
    private GameObject _currentExitPopup;
    private ExitPopupUI  _exitPopupUI;
    
    [Header("Alert")]
    [SerializeField] private GameObject _pauseAlert;
    private GameObject _currentAlertInstance;
    
    [Header("Button")]
    public Sprite pauseSprite;
    public Sprite startSprite;
    private Image _btnImage;
    private bool _isPaused = false;

    public void Awake()
    {
        //팝업 프리팹 생성해서 할당
        _currentInfoPopup = CreatePopup(_infoPopup);
        if (_currentInfoPopup != null)
            _infoPopupUI = _currentInfoPopup.GetComponent<InfoPopupUI>();
    
        _currentSettingPopup = CreatePopup(_settingPopup);
        if (_currentSettingPopup != null)
            _settingPopupUI = _currentSettingPopup.GetComponent<SettingPopupUI>();
        
        _currentExitPopup = CreatePopup(_exitPopup);
        if (_currentExitPopup != null)
            _exitPopupUI = _currentExitPopup.GetComponent<ExitPopupUI>();
        
        //버튼 이미지 컴포넌트
        _btnImage = GetComponent<Image>();
    }

    private GameObject CreatePopup(GameObject basePopup)
    {
        if (basePopup == null) return null;

        // Canvas 찾기 및 생성
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.LogError("씬에 Canvas 오브젝트가 없습니다!");
            return null;
        }

        GameObject clone = Instantiate(basePopup, canvas.transform);
        clone.SetActive(false);
        return clone;
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
            
            //버튼 이미지 변경
            _btnImage.sprite = startSprite;
        }
        else
        {
            ResumeGame();
            _btnImage.sprite = pauseSprite;
        }
    }
    
    public void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        if (_pauseAlert != null) _currentAlertInstance.SetActive(false);
    }
    
    /// <summary>
    /// 소개 팝업 열기
    /// </summary>
    public void OnClickInfo()
    {
        _infoPopupUI.Open();
    }
    /// <summary>
    /// 설정 팝업 열기
    /// </summary>
    public void OnClickSetting()
    {
        _settingPopupUI.Open();
    }
    
    /// <summary>
    /// 게임 종료 팝업 열기
    /// </summary>
    public void OnClickExit()
    {
        _exitPopupUI.Open();
    }
}
