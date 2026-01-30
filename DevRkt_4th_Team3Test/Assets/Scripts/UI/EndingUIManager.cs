using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TitleText;
    [SerializeField] private TextMeshProUGUI ResultText;
    public static bool IsClear = false;
    public static string EndTime = "00:00";
    
    private void Start()
    {
        TitleText.text = IsClear ? "Clear!" : "Fail";
        
        int _killCount = 10; //TODO: 실제 데이터 추가하기
        string killCountText = $"잡은 몬스터: {_killCount}마리";
        string endTimeText = $"종료 타임: {EndTime}";
        ResultText.text = IsClear ? killCountText : endTimeText;

    }
    
    /// <summary>
    /// 시작 화면으로 이동
    /// </summary>
    public void MoveStartScene()
    {
        IsClear = false;
        SceneManager.LoadScene(0);
    }
    
    /// <summary>
    /// 다시 시작 기능
    /// </summary>
    public void ReplayGame()
    {
        //게임 상태 리셋
        IsClear = false;
        EndTime = "00:00";
        //몬스터 리셋
        MonsterManager.ResetCount();
        MonsterState.OnMonsterDie = null;
        //무기 카드 리셋
        if (CardManager.CardInstance != null)
        {
            //TODO: 리셋하는 코드 넣어야함.
            //CardManager.CardInstance
        }
        //씬 초기화 다시 실행
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
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