using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 데미지 주고 받는 인터페이스 & 체력회복
/// </summary>
public interface IDamagable
{
    void TakeDamage(int amount);
    void Heal(int amount);

}


