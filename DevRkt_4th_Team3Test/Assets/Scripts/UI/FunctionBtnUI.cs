using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FunctionBtnManager : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] private GameObject _infoPopup;
    private GameObject _currentInfoPopup;
    private InfoPopupUI  _infoPopupUI;
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
        if (_currentInfoPopup == null)
        {
            // Instantiate: 게임 오브젝트를 동적으로 생성
            _currentInfoPopup = Instantiate(_infoPopup, GameObject.Find("Canvas").transform);
            // 생성 직후 화면에서 숨김
            _currentInfoPopup.SetActive(false);
        }
        
        _infoPopupUI = _currentInfoPopup.GetComponent<InfoPopupUI>();
        
        //버튼 이미지 컴포넌트
        _btnImage = GetComponent<Image>();
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
    /// 설정 팝업 열기
    /// </summary>
    public void OnClickSetting()
    {
        _infoPopupUI.Open();
    }
    
    /// <summary>
    /// 게임 종료 기능
    /// </summary>
    public void OnClickExit()
    {
        SceneManager.LoadScene(2);
    }
}
