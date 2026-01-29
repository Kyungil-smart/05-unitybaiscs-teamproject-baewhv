using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 스탯관련 클래스
/// </summary>
public class PlayerStats : MonoBehaviour, IDamagable
{
    [SerializeField][Range (1, 99)]private int _level = 1;
    [SerializeField] private int _maxHP = 300;
    [SerializeField] private int _currentHP;
    [SerializeField] private float _attackDamage = 1;
    [SerializeField] private int _defense = 1;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private AudioClip _deathSound; 
    [SerializeField] private AudioClip _hitSound;
    
    // 원래 컬러가 흰색이 아닐경우도 있어서 오리지널 컬러를 따로 지정해서 저장
    private Color _originalColor;
    private AudioSource _audioSource;
    private bool _isDead = false;

    // 사망 시 캐릭터 색상 변경용
    private Renderer _renderer;   // 캐릭터 색상 변경용

    // 죽음 이벤트를 선언
    public event Action OnPlayerDeath;
    // HP 변경 이벤트
    public event Action OnHPChanged;

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
    // PlayerStats.cs
    public int CurrentHP
    {
        get
        {
            return _currentHP;
        }
    }
    public void Start()
    {
        _currentHP = _maxHP;
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        
        if (_renderer != null)
            _originalColor = _renderer.material.color;
    }

    // 작동 확인용 (추후 삭제)
    public void Update()
    {
        // 테스트용: 데미지 받기
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(50);
        }
        // 테스트용: 힐
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(50);
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
    
    // 캐릭터 데미지 받기
    public void TakeDamage(int damage)
    {
        if (_isDead) return;
        
        int lastDamage = Mathf.Max(damage - _defense, 1);
        _currentHP -= lastDamage;
        
        // 범위 제한
        _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);

        // 캐릭터 피격 이펙트 (hp가 0이면 작동하지 않게)
        if (_currentHP > 0 && _renderer != null) 
        {
            StartCoroutine(HitReaction());
        }
        
        OnHPChanged?.Invoke();

        if (_currentHP <= 0)
        {
            Death();
        }
    }
    // 캐릭터 피격 이펙트
    private IEnumerator HitReaction()
    {
        // 캐릭터 피격 시 사망 사운드 출력
        if (_audioSource != null && _hitSound != null)
        {
            _audioSource.PlayOneShot(_hitSound);
        }
        _renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f); 
        _renderer.material.color = _originalColor; 
    }


    // 캐릭터 힐 받기
    public void Heal(int amount)
    {
        _currentHP += amount;
        // 범위제한
        _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);

        OnHPChanged?.Invoke();
    }


    // 캐릭터 죽음 처리
    private void Death()
    {
        if (_isDead) return;
        _isDead = true;
        _currentHP = 0;

        // 캐릭터 색상 회색으로 변경
        if (_renderer != null)
        {
            _renderer.material.color = Color.black;
        }
        // 캐릭터 사망 시 사망 사운드 출력
        if (_audioSource != null && _deathSound != null)
        {
            _audioSource.PlayOneShot(_deathSound);
        }

        // 죽음 이벤트 알림 -> 구독 필요
        OnPlayerDeath?.Invoke();
    }
}
