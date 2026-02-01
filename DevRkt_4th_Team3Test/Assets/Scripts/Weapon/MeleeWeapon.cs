using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    //MeleeWeapon은 WeaponBase 전투스탯의 weaponDamage, weaponAttackSpeed만 씁니다
    //근접무기. isActive가 true가 되면, N/공격속도 초마다 N/(2*공격속도)만큼 범위공격(콜라이더)을 활성화하고, 이펙트 애니메이션을 재생함.
    //프리펩은 닿으면 적에게 데미지.

    private float cooltimeTimer = 0f;
    private float activationTimer = 0f;
    public GameObject _player;
    private float AttackspeedOffset = 20f;
    public float startTime = 0f;
    private Collider _meleeCollider;
    private Animator animator;
    private SpriteRenderer _spriteRenderer;
    private bool isAttacking = false;
    private void Awake()
    {
        
        _meleeCollider =  GetComponent<Collider>();
        //하위에 있는 스프라이트로부터 애니메이터,스프라이트렌더러 가져옴.
        animator = GetComponentInChildren<Animator>();
        _spriteRenderer =  GetComponentInChildren<SpriteRenderer>();
        if(_spriteRenderer !=null) _spriteRenderer.enabled = false;
    }
    private void Start()
    {
        _player = MeleeWeaponManager.MeleeInstance._player;
    }
    
    private void Update()
    {
        //중심 플레이어에 맞춤.
        transform.position = _player.transform.position;
        
        // 무기가 활성화 되어있을때만 공격
        if (!isActive) return;
        cooltimeTimer += Time.deltaTime;

        // 공속에 따른 쿨타임 체크
        if (cooltimeTimer >= AttackspeedOffset / weaponAttackSpeed)
        {
            activationTimer += Time.deltaTime;
            //애니메이션 을 쿨타임/2 만큼 재생.
            if (isAttacking == false)
            {
                isAttacking = true;
                
                //콜라이더 활성화
                if(_meleeCollider != null) _meleeCollider.enabled = true;
                if(_spriteRenderer != null) _spriteRenderer.enabled = true;
                
                if (animator != null)
                {
                    // 애니메이션 속도도 공속에 맞춰 빨라지게 설정
                    animator.speed = AttackspeedOffset / (2*weaponAttackSpeed); 
                    animator.SetTrigger("Attack");
                }
            }
            
            //애니메이션이 끝났을떄
            if (activationTimer >= AttackspeedOffset / (2*weaponAttackSpeed))
            {
                isAttacking = false;
                //콜라이더 꺼주기
                if(_meleeCollider != null) _meleeCollider.enabled = false;
                //애니메이션 비활성화 시켜주기.
                if(_spriteRenderer != null) _spriteRenderer.enabled = false;
                cooltimeTimer = 0f; // 쿨타임 초기화
                activationTimer = 0f; 
            }
        }
        
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
            Debug.Log($"fireAttack : {weaponDamage}");
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
