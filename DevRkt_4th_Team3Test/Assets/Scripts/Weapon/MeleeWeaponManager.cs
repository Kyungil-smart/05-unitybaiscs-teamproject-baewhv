using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponManager : MonoBehaviour
{
    public static MeleeWeaponManager MeleeInstance;
    //무기 프리펩 저장
    [SerializeField] private List<GameObject> prefabs =  new List<GameObject>();
    //무기들 종류마다 저장
    public List<MeleeWeapon> _meleeWeapons = new List<MeleeWeapon>();
    public GameObject _player;

    private float timer = 0;
    private void Awake()
    {
        if (MeleeInstance != null && MeleeInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            MeleeInstance = this; // ....
            DontDestroyOnLoad(gameObject);
        }
        CreatePrefabsObject();
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
    
    // 프리팹으로부터 객체를 만들어서 _meleeWeapons에 등록하는 함수
    private void CreatePrefabsObject()
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            
            if (prefabs[i] != null)
            {
                GameObject go = Instantiate(prefabs[i], transform);
                MeleeWeapon rw = go.GetComponent<MeleeWeapon>();
                _meleeWeapons.Add(rw);
            }
        }
    }
}
