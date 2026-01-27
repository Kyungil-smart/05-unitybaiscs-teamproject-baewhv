using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 플레이어 이동 및 키입력, Rigidbody 적용
    /// </summary>
    private Vector3 _movement;
    private Rigidbody _rigidbody;

    // 플레이어 스탯 참조
    private PlayerStats _playerStats;
    
    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerStats = GetComponent<PlayerStats>();
    }
    public void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        // 탑다운이기 때문에 y필요 없음, 노멀라이즈로 대각선
        _movement = new Vector3(h, 0, v).normalized;
    }
    public void FixedUpdate()
    {
        _rigidbody.MovePosition(transform.position + _movement * _playerStats.MoveSpeed * Time.fixedDeltaTime);
    }

}
