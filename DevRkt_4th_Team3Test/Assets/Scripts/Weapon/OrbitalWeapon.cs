using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrbitalWeapon : WeaponBase, IWeapon
{
    [SerializeField] private GameObject _player;
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
        if (!collider.CompareTag("WeaponEnemy"))
        {
            return;
        } 
            
        
        WeaponMonster monster = collider.GetComponent<WeaponMonster>();
        // 컴포넌트가 없으면 리턴
        if (monster == null) return;
        
        // 공격 실행
        WeaponAttack(monster);
    }
    
    
    
    /// <summary>
    /// 무기로 몬스터 공격.
    /// </summary>
    /// <param name="monster"></param>
    public void WeaponAttack(WeaponMonster monster)
    {
        monster.HP -= weaponDamage;
        Debug.Log($"HP : {monster.HP}");
    }
    
}
