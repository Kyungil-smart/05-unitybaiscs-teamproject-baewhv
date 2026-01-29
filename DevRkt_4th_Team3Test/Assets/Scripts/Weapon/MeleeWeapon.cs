using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    //MeleeWeapon은 WeaponBase 전투스탯의 weaponDamage, weaponAttackSpeed만 씁니다
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
