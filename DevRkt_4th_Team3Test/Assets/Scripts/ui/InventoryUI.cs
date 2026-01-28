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
        ClearContainers();

        if (DataController.Instance.currentData.inventory.Count > 0)
        {
            InventoryData invData = DataController.Instance.currentData.inventory[0];

            foreach (var weapon in invData.weapons)
            {
                CreateSlot(weapon, weaponContainer);
            }

            foreach (var passive in invData.passives)
            {
                CreateSlot(passive, passiveContainer);
            }
        }
    }

    private void CreateSlot(ItemBase item, Transform parent)
    {
        if (item == null) return;

        GameObject obj = Instantiate(slotPrefab, parent);
        
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
