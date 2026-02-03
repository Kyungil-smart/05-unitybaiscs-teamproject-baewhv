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
    [SerializeField] public int Level = 1;              
    public int CurrentExp = 0;         
    [SerializeField]public int ExpToNextLevel = 100;  
    [SerializeField] private TextMeshProUGUI _levelUpText;
    
    private PlayerStats _playerStats;
    //레벨업 이벤트를 선언
    public event Action OnLevelUp;

    public void Start()
    {
        _playerStats = GetComponent<PlayerStats>();
    }

    // 경험치 획득 _ 몬스터가 가진 경험치를 받아와야함
    public void GainExp(int exp)
    {
        CurrentExp += exp;
        Debug.Log($"GainExp 호출: {exp}, 현재 경험치: {CurrentExp}, 필요 경험치: {ExpToNextLevel}");

        if (CurrentExp >= ExpToNextLevel)
        {
            LevelUp();
        }
    }

    // 레벨업
    public void LevelUp()
    {
        Level++;
        CurrentExp -= ExpToNextLevel;
        ExpToNextLevel = Mathf.RoundToInt(ExpToNextLevel * 1.4f);
        _playerStats.IncreaseStats();
        _playerStats.StartLevelUpEffect();
        LevelUpText();
        // 테스트 용 콘솔 출력
        Debug.Log($"Level {Level} Next EXP: {ExpToNextLevel}");

        // 레벨업 이벤트 발생 알림
        // expSystem.OnLevelUp += *****; 으로 이벤트 구독하기
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
