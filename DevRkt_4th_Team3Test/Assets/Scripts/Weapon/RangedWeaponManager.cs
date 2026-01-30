using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponManager : MonoBehaviour
{
    public static RangedWeaponManager RangedInstance;

    // 인스펙터에서 배치한 RangedWeapon 스크립트들을 할당
    public List<GameObject> _rangedWeaponPrefabs = new List<GameObject>();
    // 실제 게임 내에서 관리될 생성된 무기들
    public List<RangedWeapon> _rangedWeapons = new List<RangedWeapon>();
    public GameObject _player;

    private void Awake()
    {
        if (RangedInstance != null && RangedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            RangedInstance = this; // ....
            DontDestroyOnLoad(gameObject);
        }
        CreatePrefabsObject();
    }

    private void Start()
    {
        
    }
    
    private void OnDestroy()
    {
        if (RangedInstance == this) 
        {
            RangedInstance = null;
        }
    }
    // 프리팹으로부터 객체를 만들어서 _rangedWeapons에 등록하는 함수
    private void CreatePrefabsObject()
    {
        for (int i = 0; i < _rangedWeaponPrefabs.Count; i++)
        {
            
            if (_rangedWeaponPrefabs[i] != null)
            {
                GameObject go = Instantiate(_rangedWeaponPrefabs[i], transform);
                RangedWeapon rw = go.GetComponent<RangedWeapon>();
                _rangedWeapons.Add(rw);
            }
        }
    }
}