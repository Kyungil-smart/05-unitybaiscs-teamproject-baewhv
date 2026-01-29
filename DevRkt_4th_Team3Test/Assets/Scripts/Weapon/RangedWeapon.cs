using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : WeaponBase
{
    //RangedWeapon - 충돌 범위가지고 기본적인 무기정보들을 갖는다.
    //프리펩 클래스 - OnTriggerEnter를 따로 둬가지고 
    
    //1. 무기가 활성화 되어있을때 -> _rangedWeapons[i].isActive = true?
    //2. GetNearestEnemy로 가장 가까운 적의 위치를 받아서
    //3. 활성화된 무기를 발사함.
    //-- 무기 발사 --
    //(받은 적 transform - 플레이어transform).Normarlized로 방향 계산. 
    //프리펩을 플레이어 위치에서 방향벡터 * (프리펩의 시작지점과 플레이어사이의간격) 의 위치에 프리펩으로 인스턴스 생성.
    //인스턴스를 Translate(적의 위치)로 보냄 (GetnearestEnemy로 받은 적의 위치를 계속 계산해서 그쪽으로 보낸다.)
    //인스턴스가 적과 닿았을때 인스턴스 파괴하고 적에게 데미지를 준다.
    //-- 예외처리 --
    //적이 인스턴스가 적에게 이동하는 사이에 죽으면 죽기전 마지막 적의 위치로 가서 인스턴스를 파괴시킨다.

    private float timer = 0f;
    [SerializeField] private float spawnOffset = 0.7f; // 플레이어 몸체보다 약간 앞에서 생성
    public GameObject _player;
    private float AttackspeedOffset = 10f;
    public float startTime = 0f;
    private void Start()
    {
        _player = RangedWeaponManager.RangedInstance._player;
        if (_player == null)
        {
            Debug.Log("플레이어를 못찾았습니다.");
        }
        else
        {
            //.Log(_player.name);    
        }
        
    }
    private void Update()
    {
        
        //무기가 안보이는 이유 : RangedWeapon 프리펩을 생성해야하는데 생성을 못하고 있다.
        transform.position = _player.transform.position;
        // 무기가 활성화 되어있을때만 공격
        if (!isActive) return;
        timer += Time.deltaTime;

        
        // 공속에 따른 쿨타임 체크
        if (timer >= AttackspeedOffset / weaponAttackSpeed)
        {
            // 가장가까운적찾기
            Transform target = GetNearestEnemy();
            if (target == null)
            {
                Debug.Log("적이 범위내에 없습니다");
            }
            // 적이 있을때 projectile 발사
            if (target != null)
            {
                Fire(target);
                timer = 0f; // 쿨타임 초기화
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);  
        Gizmos.DrawWireSphere(transform.position, weaponRange);  // 
    }
    Transform GetNearestEnemy()
    {
        // 범위 내의 모든 콜라이더 검출 (LayerMask를 쓰면 더 최적화 가능)
        Collider[] hits = Physics.OverlapSphere(transform.position, weaponRange, LayerMask.GetMask("Enemy"));
        
        Transform nearestEnemy = null; 
        float minDistance = 100000000f; 

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                
                // 더 가까운 적을 발견하면 갱신
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = hit.transform;
                }
            }
        }

        return nearestEnemy;
    }
    
    private void Fire(Transform target)
    {
        
        //방향 계산
        Vector3 direction = (target.position - transform.position).normalized;

        // 프리펩의 스폰지점을 계산.
        Vector3 spawnPosition = transform.position + (direction * spawnOffset);
        
        // 인스턴스 생성
        GameObject projectileObj = Instantiate(objectPrefab, spawnPosition, Quaternion.identity); //Quaternion.identity 방향. 나중에바꾸기. 현재는 RangedProjectile에서 방향변경.
        
        // 생성된 투사체 세팅
        RangedProjectile projectileScript = projectileObj.GetComponent<RangedProjectile>();
        if (projectileScript != null)
        {
            // 인스턴스를 적의 위치로 보냄 (업데이트에서 계속 추적함)
            projectileScript.Init(target, weaponDamage, this);
        }
        
    }
}
