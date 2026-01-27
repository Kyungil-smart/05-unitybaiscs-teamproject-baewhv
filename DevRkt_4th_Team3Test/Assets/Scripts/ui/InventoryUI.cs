using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("UI Containers")]
    public Transform weaponContainer;
    public Transform passiveContainer;
    
    [Header("Prefabs")]
    public GameObject slotPrefab;
    
    private void Start()
    {
        RefreshHUD();
    }
    
    /// <summary>
    /// 전체 HUD 인벤토리 리프레시
    /// </summary>
    public void RefreshHUD()
    {
        // 1. 기존에 생성된 슬롯들 제거
        ClearContainers();

        // 2. DataController에서 현재 인벤토리 데이터 가져오기
        // SaveData.cs 구조상 inventory 리스트의 첫 번째 항목을 사용한다고 가정합니다.
        if (DataController.Instance.currentData.inventory.Count > 0)
        {
            InventoryData invData = DataController.Instance.currentData.inventory[0];

            // 3. 무기 목록 생성 (WeaponItem -> ItemBase 상속 덕분에 가능)
            foreach (var weapon in invData.weapons)
            {
                CreateSlot(weapon, weaponContainer);
            }

            // 4. 패시브 목록 생성 (PassiveItem -> ItemBase 상속 덕분에 가능)
            foreach (var passive in invData.passives)
            {
                CreateSlot(passive, passiveContainer);
            }
        }
    }
    
    /// <summary>
    /// 슬롯 생성
    /// </summary>
    private void CreateSlot(ItemBase item, Transform parent)
    {
        if (item == null) return;

        GameObject obj = Instantiate(slotPrefab, parent);
        
        // 슬롯 안에 있는 텍스트 컴포넌트에 아이템 이름 표시
        var textDisplay = obj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (textDisplay != null)
        {
            textDisplay.text = $"{item.itemName} (x{item.count})";
        }
    }
    
    private void ClearContainers()
    {
        foreach (Transform child in weaponContainer) Destroy(child.gameObject);
        foreach (Transform child in passiveContainer) Destroy(child.gameObject);
    }
    
}
