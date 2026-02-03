using System.Collections.Generic;

[System.Serializable]
public class ItemBase 
{
    public string itemName;
    public int count;
}

[System.Serializable]
public class Item<T> : ItemBase
{
    public T data; // 구체적인 성능 데이터
}
[System.Serializable]
public class InventoryData
{
    public List<WeaponData> weapons = new List<WeaponData>();
    public List<PassiveItemData> passives = new List<PassiveItemData>();
    //TODO: 다른 것도 넣을 것 있으면 작성
}

