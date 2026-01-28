using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 플레이어 레벨업 시스템
/// 레벨이 오를 수록 필요한 경험치 증가
/// </summary>
public class ExpSystem : MonoBehaviour
{
    [SerializeField] int _level = 1;              
    private int _currentExp = 0;         
    [SerializeField]private int _expToNextLevel = 100;  
    [SerializeField] private TextMeshProUGUI _levelUpText;
    
    private PlayerStats _playerStats;
    //레벨업 이벤트를 선언
    public event Action OnLevelUp;

    void Start()
    {
        _playerStats = GetComponent<PlayerStats>();
    }

    // 경험치 들어오는지 확인(테스트용 테스트 후 삭제)
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GainExp(200);
        }
    }

    
    // 경험치 획득 _ 몬스터가 가진 경험치를 받아와야함
    public void GainExp(int exp)
    {
        _currentExp += exp;

        if (_currentExp >= _expToNextLevel)
        {
            LevelUp();
        }
    }

    // 레벨업
    public void LevelUp()
    {
        _level++;
        _currentExp -= _expToNextLevel;
        _expToNextLevel = Mathf.RoundToInt(_expToNextLevel * 1.2f);
        _playerStats.IncreaseStats();
        LevelUpText();
        // 테스트 후 삭제
        Debug.Log($"Level {_level} Next EXP: {_expToNextLevel}");
        // 레벨업 이벤트 발생 알림
        OnLevelUp?.Invoke();
    }

    public void LevelUpText()
    {
        _levelUpText.text = "Level Up!";
        _levelUpText.gameObject.SetActive(true);
        // 2초 후에 레벨업 메시지 사라짐
        Invoke("HideLevelUpText", 2f);
    }
    private void HideLevelUpText()
    {
        _levelUpText.gameObject.SetActive(false);
    }
}
