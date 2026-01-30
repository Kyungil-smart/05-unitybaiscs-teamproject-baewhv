using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponManager : MonoBehaviour
{
    public static MeleeWeaponManager MeleeInstance;
    //무기 프리펩 저장
    [SerializeField] private List<GameObject> prefabs =  new List<GameObject>();
    //궤도무기들 종류마다 저장
    public List<MeleeWeapon> _meleeWeapons = new List<MeleeWeapon>();
    [SerializeField] private GameObject _player;

    private float timer = 0;
    //플레이어 따라다님. transform.position = _player.transform.position
    //기본값f/공격속도의 속도로 데미지만큼 공격, (OntriggerEnter에서 특정초가 지날때마다 공격? 코루틴?
    //start부터 시작하는 타이머 - (공격속도 바뀌는것은 타이머 사이클 끝난직후부터 적용.)
    
    private void Start()
    {
        
    }
    
    private void Update()
    {
        timer += Time.deltaTime;
    }
    
    private void OnDestroy()
    {
        if (MeleeInstance == this) 
        {
            MeleeInstance = null;
        }
    }
    
}
