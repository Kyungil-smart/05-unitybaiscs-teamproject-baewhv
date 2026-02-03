using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image itemIcon;
    
    public void SetItem(WeaponBase weapon)
    {
        itemIcon.sprite = weapon.weaponSprite;
    }
}
