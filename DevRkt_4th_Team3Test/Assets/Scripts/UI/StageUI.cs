using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageUI : MonoBehaviour
{
    [Header("Timer UI")]
    [SerializeField]private TextMeshProUGUI _timerText;
    private float _elapsedTime = 0f;
    private const float GAME_LIMIT_TIME = 300f;
    private bool _isGameOver = false;
    
    [Header("Kill UI")]
    [SerializeField]private TextMeshProUGUI _killCountText;
    private int _killCount = 0;
    public static int KillCount;
    
    
    void Update()
    {
        if (_isGameOver) return;

        //타이머 계산
        _elapsedTime += Time.deltaTime;
        UpdateTimerDisplay();

        //5분 체크
        if (_elapsedTime >= GAME_LIMIT_TIME)
        {
            _isGameOver = true;
            EndingUIManager.IsClear = true;
            GameManager.Instance.SetGameOver();
        }
    }
    
    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(_elapsedTime / 60);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60);
        string timerText = string.Format("{0:00}:{1:00}", minutes, seconds);
        _timerText.text = timerText;
        EndingUIManager.EndTime = timerText;
    }
    
    void OnEnable()
    {
        // 몬스터 죽음 이벤트에 내 함수를 등록
        MonsterState.OnMonsterDie += OnCountMonsterKilled;
    }
    public void OnCountMonsterKilled()
    {
        _killCount++;
        _killCountText.text = $"{_killCount}";
        KillCount = _killCount;
    }
    
    void OnDisable()
    {
        // 씬이 바뀔 때 등록 해제
        MonsterState.OnMonsterDie -= OnCountMonsterKilled;
    }
}
