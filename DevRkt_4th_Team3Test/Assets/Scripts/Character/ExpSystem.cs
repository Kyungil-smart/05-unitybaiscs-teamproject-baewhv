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
        CurrentExp += exp;

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
        ExpToNextLevel = Mathf.RoundToInt(ExpToNextLevel * 1.2f);
        _playerStats.IncreaseStats();
        LevelUpText();
        Debug.Log($"Level {Level} reached! Next EXP: {ExpToNextLevel}");

    }

    public void LevelUpText()
    {
        _levelUpText.text = "Level Up!";
        _levelUpText.gameObject.SetActive(true);
        Invoke("HideLevelUpText", 2f);
    }
    private void HideLevelUpText()
    {
        _levelUpText.gameObject.SetActive(false);
    }



}
