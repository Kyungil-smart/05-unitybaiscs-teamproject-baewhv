using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponBase : MonoBehaviour
{
    [Header("기본 정보")]
    public string _weaponName; // 무기 이름
    public bool isActive = false; // 액티브상태인지 아닌지. 무기가 처음 뽑힐때 isActive = true;
    public int level = 0; //무기 카드가 선택된 수 = 무기 레벨
    public bool isPromotionWeapon = false; // 승급무기인지 체크하는 변수
    public bool isUpgradeFinished = false; // 무기가 업그레이드가 되었는지 체크하는 변수. CardManager의 PickWeaponType에서 사용.
    [Header("전투 스탯")]
    public float weaponDamage; // 기본 공격력
    public float weaponAttackSpeed;  // 공격속도 (궤도무기의 경우 공전속도)
    public int projectileCount; // 투사체 개수 (궤도무기만 사용)
    public float weaponRange; // 공격범위 (변경x 무기마다 고정값)
    [Header("프리팹")]
    public GameObject objectPrefab;
    [Header("승진 무기")]
    public GameObject promotionPrefab;
    [Header("스프라이트")]
    public Sprite weaponSprite;

    
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