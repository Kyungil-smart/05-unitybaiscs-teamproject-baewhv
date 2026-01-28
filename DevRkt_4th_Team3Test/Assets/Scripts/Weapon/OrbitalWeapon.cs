using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Game;
public class OrbitalWeapon : WeaponBase, IWeapon
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
        if (!collider.CompareTag("Enemy"))
        {
            return;
        } 
            
        
        // Monster monster = collider.GetComponent<Monster>();
        // // 컴포넌트가 없으면 리턴
        // if (monster == null) return;
        //
        // // 공격 실행
        // WeaponAttack(monster);
    }
    
    
    
    /// <summary>
    /// 무기로 몬스터 공격.
    /// </summary>
    /// <param name="monster"></param>
    public void WeaponAttack(WeaponMonster monster)
    {
        // monster선언되면 이부분 변경. HP대신 HP에 대응되는 변수로
        // monster.HP -= weaponDamage;
        // Debug.Log($"HP : {monster.HP}");
    }
    
}
