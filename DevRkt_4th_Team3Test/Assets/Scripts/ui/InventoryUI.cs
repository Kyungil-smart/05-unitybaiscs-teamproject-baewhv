using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _slotPrefab; 
    private Dictionary<WeaponBase, GameObject> _weaponsList = new Dictionary<WeaponBase, GameObject>();

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        _weaponsList.Clear();

        if (WeaponManager.WeaponInstance != null)
        {
            foreach (var weapon in WeaponManager.WeaponInstance.weapons)
            {
                if (weapon.isActive) AddWeaponSlot(weapon);
            }
        }
    }
    
    public void AddWeaponSlot(WeaponBase weapon, WeaponBase oldWeapon = null)
    {
        // 승급인 경우
        // 기존 무기 슬롯 제거
        if (oldWeapon != null && _weaponsList.ContainsKey(oldWeapon))
        {
            Destroy(_weaponsList[oldWeapon]);
            _weaponsList.Remove(oldWeapon);
        }
        
        //이미 인벤토리에 있는 무기라면 중복 생성 안 함
        if (weapon == null || _weaponsList.ContainsKey(weapon)) return;

        // 슬롯 생성
        GameObject newSlot = Instantiate(_slotPrefab, transform);
        _weaponsList.Add(weapon, newSlot);

        // 슬롯 데이터 추가
        InventorySlot slotScript = newSlot.GetComponent<InventorySlot>();
        if (slotScript != null)
        {
            slotScript.SetItem(weapon);
        }
    }

    
}
