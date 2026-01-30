using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    //MeleeWeapon은 WeaponBase 전투스탯의 weaponDamage, weaponAttackSpeed만 씁니다
    //근접무기. isActive가 true가 되면 N/공격속도 초마다 N/(2*공격속도)만큼 범위공격(프리펩)을 활성화하고, 프리펩 애니메이션 재생
    //프리펩은 닿으면 적에게 데미지. (생각보다 간단하다?? 애니메이션만 좀 어떻게 하면..)
    private void OnTriggerEnter(Collider collider)
    {
        //충돌한 물체가 enemy이면 공격
        if (!collider.CompareTag("Enemy"))
        {
            return;
        } 
            
        
        MonsterState monster = collider.GetComponent<MonsterState>();
        // 컴포넌트가 없으면 리턴
        if (monster == null) return;
        monster.TakeDamage(weaponDamage);

    }
    
    
    
    
}
