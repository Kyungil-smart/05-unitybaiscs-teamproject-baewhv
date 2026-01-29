using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedProjectile : MonoBehaviour
{
    private Transform target; // 추적할 적
    private Vector3 lastTargetPosition; // 적이 죽었을 때를 대비한 마지막 위치
    private float damage;
    private float speed = 8f; // 투사체 이동 속도

    // 초기화 함수 (RangedWeapon에서 호출)
    public void Init(Transform targetTransform, float weaponDamage, RangedWeapon weapon)
    {
        target = targetTransform;
        damage = weaponDamage;
        
        // 타겟이 있으면 초기 위치 저장
        if (target != null)
        {
            lastTargetPosition = target.position;
        }
    }

    private void Update()
    {
        // 1. 타겟이 살아있는 경우
        if (target != null)
        {
            lastTargetPosition = target.position; // 계속 위치 갱신
            // 적을 향해 이동
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            transform.LookAt(target);

            // if (transform.position == target.position)
            // {
            //     Destroy(gameObject);
            // }
        }
        // 2. 타겟이 죽거나 사라진 경우 (예외처리)
        else
        {
            // 마지막으로 기억하는 위치로 이동
            transform.position = Vector3.MoveTowards(transform.position, lastTargetPosition, speed * Time.deltaTime);

            // 마지막 위치에 거의 도달했으면 파괴
            if (Vector3.Distance(transform.position, lastTargetPosition) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 지정된 적과 충돌 시
        if (other.CompareTag("Enemy") && other.transform == target)
        {
            MonsterState monster = other.GetComponent<MonsterState>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
            }
            
            Destroy(gameObject);
        }
    }
}
