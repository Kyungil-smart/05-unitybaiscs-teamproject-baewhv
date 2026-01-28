using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class DataController : MonoBehaviour

{
    public static DataController Instance;
    public SaveData currentData = new SaveData();

    void Awake() => Instance = this;
    
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
            currentData = JsonUtility.FromJson<SaveData>(json);
            
            foreach (var inv in currentData.inventory)
            {
                Debug.Log($"무기 개수: {inv.weapons.Count}, 패시브 개수: {inv.passives.Count}");
            }
        } 
    }

}