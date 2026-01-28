using System.Collections.Generic;

[System.Serializable]    
public class SaveData
    {
        public List<InventoryData> inventory = new List<InventoryData>();
        
        //TODO: 플레이어 데이터
       private int Level;
       private int CurrentHP;
       private int x;
       private int y;
    }
