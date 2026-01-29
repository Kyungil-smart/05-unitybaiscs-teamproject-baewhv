using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager WeaponInstance;
    
    //TODO 무기를 담는 변수 종류에 상관없이 담음
    public List<WeaponBase> weapons = new List<WeaponBase>();

    private void Awake()
    {
        if (WeaponInstance == null)
        {
            WeaponInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (WeaponInstance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        //리스트를 0.1초 있다가 읽어옵니다.
        StartCoroutine(FirstGetWeaponList(0.1f));
        
    }
    /// <summary>
    /// 싱글톤으로 선언된 3종류의 WeaponManager로부터 무기들 종류를 받아서 List<WeaponBase> weapons에다가 등록한다.
    /// </summary>
    public void GetWeaponList()
    {
        weapons.Clear(); //리스트 초기화 시킨 후 다시 받아온다.
        
        //OrbitalWeapon들을 가져온다.
        for (int i = 0; i < OrbitalWeaponManager.OrbitalInstance._orbitalWeapons.Count; i++)
        {
            weapons.Add(OrbitalWeaponManager.OrbitalInstance._orbitalWeapons[i]);
        }
        //다른무기들 만들면 다른무기들도 나중에 가져온다.
        for (int i = 0; i < RangedWeaponManager.RangedInstance._rangedWeapons.Count; i++)
        {
            weapons.Add(RangedWeaponManager.RangedInstance._rangedWeapons[i]);
        }

        // for (int i = 0; i < weapons.Count; i++)
        // {
        //     Debug.Log(weapons[i]._weaponName);
        // }
    }

    private IEnumerator FirstGetWeaponList(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetWeaponList();
    }
}
