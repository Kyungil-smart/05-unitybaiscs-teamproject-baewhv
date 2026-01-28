using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 스탯관련 클래스
/// </summary>
public class PlayerStats : MonoBehaviour
{
    [SerializeField][Range (1, 99)]private int _level = 1;
    [SerializeField] private int _maxHP = 1000;
    [SerializeField] private int _currentHP;
    [SerializeField] private float _attackDamage = 1;
    [SerializeField] private int _defense = 1;
    [SerializeField] private float _moveSpeed = 5f;
    
    // 죽음 이벤트를 선언
    public event Action OnPlayerDeath;

    // _moveSpeed를 프로퍼티로 외부에서 참조할 수 있게
    public float MoveSpeed
    {
        get
        {
            return _moveSpeed;
        }
    }
    // 최대hp 프로퍼티
    public int MaxHP
    {
        get
        {
            return _maxHP;
        }
    }
    public void Start()
    {
        _currentHP = _maxHP;
    }

    // 작동 확인용 (추후 삭제)
    public void Update()
    {
        // 테스트용: 데미지 받기
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(50);
            Debug.Log($"HP: {_currentHP}/{_maxHP}");
        }
        // 테스트용: 레벨업
        if (Input.GetKeyDown(KeyCode.L))
        {
            IncreaseStats();
            Debug.Log($"Level: {_level}, Attack: {_attackDamage}, Defense: {_defense}, MoveSpeed: {_moveSpeed}");
        }
    }

    // 레벨업에 따른 스탯 증가
    public void IncreaseStats()
    {
        _level++;
        _attackDamage += 1;
        _defense += 1;
        _moveSpeed += 0.3f;
        _maxHP += 100;
    }
    
    // 테스트용 데미지 받기
    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - _defense, 1); // 최소 1 데미지
        _currentHP -= finalDamage;

        if (_currentHP <= 0)
        {
            Death();
        }
    }

    // 캐릭터 죽음 처리
    private void Death()
    {
        _currentHP = 0;
        // 테스트용 테스트 후 삭제
        Debug.Log("플레이어 사망");
        // 죽음 이벤트 알림 -> 구독 필요
        OnPlayerDeath?.Invoke();
    }
}
