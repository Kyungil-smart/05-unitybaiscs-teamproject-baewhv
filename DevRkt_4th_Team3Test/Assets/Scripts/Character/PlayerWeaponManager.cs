using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 캐릭터가 들고있는 무기 관리 
/// </summary>
public class PlayerWeaponManager : MonoBehaviour
{
    public List<WeaponBase> weapons = new List<WeaponBase>();
    private Dictionary<WeaponBase, float> _nextAttackTimes = new Dictionary<WeaponBase, float>();

    private void Start()
    {
        foreach (var weapon in weapons)
        {
            // 무기를 활성화
            weapon.gameObject.SetActive(true);
            // 무기별 공격 타이머 초기화
            _nextAttackTimes[weapon] = 0f;     
            Debug.Log($"{weapon._weaponName} 장착됨! 데미지: {weapon.weaponDamage}, 공속: {weapon.weaponAttackSpeed}");
        }
    }

    private void Update()
    {
        foreach (var weapon in weapons)
        {
            if (Time.time >= _nextAttackTimes[weapon])
            {
                Attack(weapon);
                _nextAttackTimes[weapon] = Time.time + (1f / weapon.weaponAttackSpeed);
            }
        }
    }

    private void Attack(WeaponBase weapon)
    {
        Debug.Log($"{weapon._weaponName} 공격! 데미지: {weapon.weaponDamage}");
    }
}


