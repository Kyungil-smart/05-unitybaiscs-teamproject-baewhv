using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponBase : MonoBehaviour
{
    [Header("기본 정보")]
    [SerializeField] private string _weaponName; // 무기 이름
    
    [Header("전투 스탯")]
    public float weaponDamage; // 기본 공격력
    public float weaponAttackSpeed;  // 공격속도 (궤도무기의 경우 공전속도)
    public int projectileCount; // 투사체 개수
    public float weaponRange; // 공격범위
    [Header("프리팹")]
    public GameObject objectPrefab;

    
    /// <summary>
    /// 카드에서 상승시킬 능력치 이름, 상승치를 넘겨주면 무기 강화시켜줌
    /// </summary>
    /// <param name="variableName"></param>
    /// <param name="value"></param>
    public void UpgradeWeapon(string variableName,float value)
    {
        switch (variableName)
        {
            case "weaponDamage" :
                weaponDamage *= value;
                break;
            case "weaponAttackSpeed" :
                weaponAttackSpeed *= value;
                break;
            case "projectileCount" :
                projectileCount += (int)value;
                break;
            case "weaponRange" :
                weaponRange *= value;
                break;
        }
    }
    public void ChangeWeaponDamage(float damage)
    {
        weaponDamage = damage;
    }
    
    public void ChangeAttackSpeed(float attackSpeed)
    {
        weaponAttackSpeed = attackSpeed;
    }
    
    public void ChangeProjectileCount(int count)
    {
        projectileCount = count;
    }

    public void ChangeAttackRange(float range)
    {
        weaponRange = range;
    }
}