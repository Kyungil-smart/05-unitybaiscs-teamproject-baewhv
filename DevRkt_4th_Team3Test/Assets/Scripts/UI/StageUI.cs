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
            SceneManager.LoadScene(2);
        }
    }
    
    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(_elapsedTime / 60);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60);
        
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
    /// <summary>
    /// 몬스터 죽으면 해당 함수 호출
    /// </summary>
    public void OnMonsterKilled(int expAmount)
    {
        _killCount++;
        _killCountText.text = $"{_killCount}";
    }
}
