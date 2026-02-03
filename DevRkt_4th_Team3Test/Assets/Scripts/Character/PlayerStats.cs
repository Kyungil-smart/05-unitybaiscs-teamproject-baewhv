using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private float _pickupRange = 0.7f;
    [SerializeField] private AudioClip _deathSound; 
    [SerializeField] private AudioClip[] _hitSounds;
    [SerializeField] private TextMeshProUGUI _healText;
    
    public float PickupRange => _pickupRange;
    private SphereCollider _pickupCollider;
    
    // 원래 컬러가 흰색이 아닐경우도 있어서 오리지널 컬러를 따로 지정해서 저장
    private Color _originalColor;
    private AudioSource _audioSource;
    private int _hitsoundIndex = 0;
    private bool _isInvincible = false;
    private bool _isDead = false;

    // 사망 시 캐릭터 색상 변경용
    private SpriteRenderer _renderer;

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
            _originalColor = _renderer.color;
        
        _pickupCollider = GetComponentInChildren<SphereCollider>();
        if (_pickupCollider != null)
        {
            _pickupCollider.isTrigger = true;
            _pickupCollider.radius = _pickupRange;
        }
    }

    // 레벨업에 따른 스탯 증가
    public void IncreaseStats()
    {
        _level++;
        _attackDamage += 1;
        _defense += 1;
        _moveSpeed += 0.3f;
        _maxHP += 120;
        _currentHP += 50;
        
        OnHPChanged?.Invoke();
    }
    
    // 캐릭터 데미지 받기
    public void TakeDamage(int damage)
    {
        if (_isDead) return;
        if (_isInvincible) return;
        
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
        // 캐릭터 피격 시 사망 사운드 출력 (2종 순환)
        if (_audioSource != null && _hitSounds.Length > 0)
        {
            _audioSource.PlayOneShot(_hitSounds[_hitsoundIndex]);
            // 여러번 피격시 사운드가 겹쳐 들려서 끊고 다음 사운드 재생
            _audioSource.Play();
            // _hitSounds.Length를 붙여줘서 다시 순환
            _hitsoundIndex = (_hitsoundIndex + 1) % _hitSounds.Length; 
        }

        _renderer.color = Color.red;
        yield return new WaitForSeconds(0.1f); 
        _renderer.color = _originalColor; 
    }

    // 캐릭터 힐 받기
    public void Heal(int amount)
    {
        if (_currentHP >= _maxHP) return;
        _currentHP += amount;
        // 범위제한
        _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);

        HealText(amount);
        
        OnHPChanged?.Invoke();
    }
    public void HealText(int amount)
    {
        if (_healText == null) return;
        _healText.gameObject.SetActive(true);
        _healText.text = $"+{amount} HP";   
        _healText.color = Color.green;      

        Invoke("HideHealText", 2f);
    }
    private void HideHealText()
    {
        if (_healText != null)
            _healText.gameObject.SetActive(false);
    }
    
    // 캐릭터 경험치 획득
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            (other.GetComponent<ItemObject>() as IInteractable)?.Interact(this);
        }
    }
    public void StartLevelUpEffect()
    {
        StartCoroutine(LevelUpRoutine());
    }

    private IEnumerator LevelUpRoutine()
    {
        _isInvincible = true;

        float duration = 2f;         
        float interval = 0.2f;        
        float timer = 0f;
        bool toggle = false;

        while (timer < duration)
        {
            if (_renderer != null)
            {
                _renderer.color = toggle ? Color.cyan : _originalColor;
                toggle = !toggle;
            }

            yield return new WaitForSecondsRealtime(interval);
            timer += interval;
        }
        
        if (_renderer != null)
            _renderer.color = _originalColor;

        _isInvincible = false;
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
            _renderer.color = Color.black;
        }

        // 죽음 이벤트 알림 -> 구독 필요
        OnPlayerDeath?.Invoke();
    }
}
