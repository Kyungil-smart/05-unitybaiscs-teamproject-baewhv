using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPopupUI : MonoBehaviour , IPopup
{
    public static ExitPopupUI Instance;
    void Awake() => Instance = this;
    
    public void Open()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    
    public void Close()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
       
    public void MoveStartScene()
    {
        Reset();
        Close();
        GameManager.Instance.SetGameOver(0);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Reset()
    {
        //몬스터 리셋
        MonsterManager.ResetCount();
        MonsterState.OnMonsterDie = null;
        StageUI.KillCount = 0;
    }
 
}