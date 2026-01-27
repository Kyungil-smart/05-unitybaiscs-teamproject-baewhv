using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot
{
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemCountText;
    
    public void SetItem(string name, int count)
    {
        itemNameText.text = name;
        itemCountText.text = count.ToString();
        //TODO: 이미지 추가 작업
        // itemIcon.sprite = Resources.Load<Sprite>($"Icons/{name}");
    }
    
    public void ClearSlot()
    {
        itemNameText.text = "";
        itemCountText.text = "";
        itemIcon.sprite = null;
    }
}
