using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 플레이어 이동 및 키입력, Rigidbody 적용
    /// 아이들, 달리기 애니메이션 구현
    /// </summary>
    private Vector3 _movement;
    private Rigidbody _rigidbody;
    private Animator _anim;
    private SpriteRenderer _sprite;

    // 플레이어 스탯 참조
    private PlayerStats _playerStats;
    // 플레이어 사망 시 이동 차단
    private bool _isDead = false;
    
    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerStats = GetComponent<PlayerStats>();
        // Animator가 자식요소에 있어서 Children으로 넣어줌
        _anim = GetComponentInChildren<Animator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        
        // 죽음 이벤트 구독
        _playerStats.OnPlayerDeath += HandleDeath;

    }
    private void HandleDeath()
    {
        _isDead = true;
        // 애니메이션 정지
        _anim.SetBool("isRunning", false); 
        _rigidbody.velocity = Vector3.zero;
    }

    public void Update()
    {
        if (_isDead) return; 

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        // 탑다운이기 때문에 y필요 없음, 노멀라이즈로 대각선
        _movement = new Vector3(h, 0, v).normalized;
        
        if (h != 0 || v != 0)
            _anim.SetBool("isRunning", true);  
        else
            _anim.SetBool("isRunning", false); 
        
        // 스프라이트 렌더러를 사용한 애니메이션 좌우 반전
        if (h < 0)
            _sprite.flipX = true;
        else if (h > 0)
            _sprite.flipX = false;
    }
    public void FixedUpdate()
    {
        if (_isDead) return;
        _rigidbody.velocity = _movement * _playerStats.MoveSpeed;
    }
}
