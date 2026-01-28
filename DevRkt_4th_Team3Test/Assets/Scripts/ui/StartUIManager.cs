using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUIManager : MonoBehaviour
{
    private DataController _dataController;
    [SerializeField] private GameObject _settingPopup;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ReloadGame()
    {
        _dataController.Load();
    }

    
    public void OpenOptions()
    {
        
    }
    
    /// <summary>
    /// 게임 종료 기능
    /// </summary>
    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
