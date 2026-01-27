using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 레벨업 시스템
/// 레벨이 오를 수록 필요한 경험치 증가
/// </summary>
public class ExpSystem : MonoBehaviour
{
    [SerializeField] int _level = 1;              
    private int _currentExp = 0;         
    [SerializeField]private int _expToNextLevel = 100;  
    
    private PlayerStats _playerStats;

    void Start()
    {
        _playerStats = GetComponent<PlayerStats>();
    }

    // 경험치 획득 
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
        // 경험치 int로 관리하기 위해 RoundToInt 사용, 1.2씩 필요 경험치 증가
        _expToNextLevel = Mathf.RoundToInt(_expToNextLevel * 1.2f); 
        _playerStats.IncreaseStats();
    }

}
