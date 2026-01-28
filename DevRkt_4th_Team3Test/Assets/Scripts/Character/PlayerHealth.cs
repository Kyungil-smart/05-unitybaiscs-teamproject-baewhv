using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 캐릭터 hp게이지 슬라이더로 구성, hp 양에따라 색상 다르게
/// 키 누르면 작동확인 할 수 있게
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    private int _currentHP;
    
    // 플레이어 스탯 참조
    private PlayerStats _playerStats;

    // HP 전체 게이지
    [SerializeField] private Slider _hpSlider; 
    // 게이지 내부 현재 HP 게이지
    [SerializeField] private Image _fillImage;      

    public void Start()
    {
        _playerStats = GetComponent<PlayerStats>();
        _playerStats.OnHPChanged += UpdateHPGuage;
        
        // Slider 초기화
        _hpSlider.maxValue = _playerStats.MaxHP;
        _hpSlider.value = _currentHP;

        UpdateHPGuage();
    }
    
    // TakeDamage, Heal 테스트용
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(100); 
        }
    }
    
    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        _currentHP = Mathf.Clamp(_currentHP, 0, _playerStats.MaxHP);
        UpdateHPGuage();
    }

    public void Heal(int amount)
    {
        _currentHP += amount;
        _currentHP = Mathf.Clamp(_currentHP, 0, _playerStats.MaxHP);
        UpdateHPGuage();
    }

    private void UpdateHPGuage()
    {
        // 슬라이더 값을 현재 hp로 설정
        _hpSlider.value = _playerStats.CurrentHP;

        float ratio = (float)_playerStats.CurrentHP / _playerStats.MaxHP;

        // 게이지에 따른 색상 변경
        if (ratio > 0.7f)
            _fillImage.color = Color.green;
        else if (ratio > 0.5f)
            _fillImage.color = Color.yellow;
        else
            _fillImage.color = Color.red;
    }

}

