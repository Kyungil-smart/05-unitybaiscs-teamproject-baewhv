using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponBase : MonoBehaviour
{
    [Header("기본 정보")]
    public string _weaponName; // 무기 이름
    public bool isActive = false; // 액티브상태인지 아닌지. 무기가 처음 뽑힐때 isActive = true;
    [Header("전투 스탯")]
    public float weaponDamage; // 기본 공격력
    public float weaponAttackSpeed;  // 공격속도 (궤도무기의 경우 공전속도)
    public int projectileCount; // 투사체 개수
    public float weaponRange; // 공격범위 (근접은 변경x)
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
                Debug.Log($"{_weaponName} 데미지 : {weaponDamage}");
                break;
            case "weaponAttackSpeed" :
                weaponAttackSpeed *= value;
                Debug.Log($"{_weaponName} 공속 : {weaponAttackSpeed}");
                break;
            case "projectileCount" :
                projectileCount += (int)value;
                Debug.Log($"{_weaponName} 투사체 개수 : {projectileCount}");
                break;
            case "weaponRange" :
                weaponRange *= value;
                Debug.Log($"{_weaponName} 공격 범위 : {weaponRange}");
                break;
        }
    }
    
}