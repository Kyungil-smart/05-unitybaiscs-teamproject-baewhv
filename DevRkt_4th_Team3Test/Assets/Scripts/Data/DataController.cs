using System.Collections;
using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class DataController : MonoBehaviour

{
    public static DataController Instance;
    public GameData currentData = new GameData();

    /// <summary>
    /// 데이터 저장 (자동 저장 시스템에 연결)
    /// </summary>
    public void Save()
    {
        string json = JsonUtility.ToJson(currentData);
        PlayerPrefs.SetString("SaveSlot_1", json);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// 데이터 불러오기
    /// </summary>
    public void Load()
    {
        if (PlayerPrefs.HasKey("SaveSlot_1"))
        {
            string json = PlayerPrefs.GetString("SaveSlot_1");
            currentData = JsonUtility.FromJson<GameData>(json);
            
            for (int i = 0; i < currentData.inventory.Count; i++)
            {
                Debug.Log($"아이템 {i}번: {currentData.inventory[i]} 로드됨");
            }
        } 
    }

}