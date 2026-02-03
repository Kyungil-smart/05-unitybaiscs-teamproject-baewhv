using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrbitalWeapon : WeaponBase
{
    private void Start()
    {
        
    }
    
    private void Update()
    {
        //캐릭터의 위치를 기준으로 공전
    }
    private void OnTriggerEnter(Collider collider)
    {
        //충돌한 물체가 enemy이면 공격
        if (collider.CompareTag("Enemy"))
        {
            MonsterState monster = collider.GetComponent<MonsterState>();
            // 컴포넌트가 없으면 리턴
            if (monster == null) return;
            monster.TakeDamage(weaponDamage);
            
            return;
        }

        //Breakable물체이면 공격.
        if (collider.CompareTag("Breakable"))
        {
            BreakableObject breakable = collider.GetComponent<BreakableObject>();

            if (breakable == null) return;
            breakable.TakeDamage((int)weaponDamage);

            return;
        }
        

    }
    
    
    
    
    
}
