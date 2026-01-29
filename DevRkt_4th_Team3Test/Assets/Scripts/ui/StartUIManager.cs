using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _infoPopup;
    private DataController _dataController;
    private GameObject _currentInfoPopup;
    private InfoPopupUI  _infoPopupUI;
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
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    
    public void OpenOptions()
    {
        _infoPopupUI.Open();
    }
    
    /// <summary>
    /// 게임 종료 기능
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
