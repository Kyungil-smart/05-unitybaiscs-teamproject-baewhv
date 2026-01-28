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
    
    // 작동 확인용 (추후 삭제)
    public void Update()
    {
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


}
