using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public struct CardInfoPerRarity
{
    public int Rarity;               // 0~3
    public string RarityName;        // "Rare", "Epic", "Unique", "Legendary"
    public float AppearProbability; // 등장 확률.
    public Color RarityColor; // UI에서 색상.
    
    // 등급별 능력치 증가 배율
    public float damageMultiplier;
    public float attackSpeedMultiplier;
    public int projectileAdd;
    public float rangeMultiplier;
}

public enum CardAbility //능력치 enum으로 저장
{
    weaponDamage, weaponAttackSpeed, projectileCount, weaponRange
}

public class Card : MonoBehaviour
{
    [SerializeField] private OrbitalWeaponManager orbitalManager;
    
    // 인스펙터에서 등급에 따른 카드에서 나올 수 있는 능력치 설정
    [SerializeField] private List<CardInfoPerRarity> infoPerRarity = new List<CardInfoPerRarity>();
    public Dictionary<int, float> probabilityOfRarity = new Dictionary<int, float>(); //쓰기 애매
    
    // 카드는 1.무기 종류 랜덤, 2.등급 랜덤, 3.상승시킬 능력치 종류 랜덤으로 나옴.
    //TODO 무기를 담는 변수. 나중에 InventoryData랑 연결.....
    [SerializeField] private List<WeaponBase> weapons = new List<WeaponBase>();
    
    
    private void Start()
    {
        GetWeaponList();
        for (int i = 0; i < 5; i++)
        {
            DrawCard();
        }
            
    }

    public void DrawCard()
    {
        //1. 카드를 뽑고(0-40%, 1-30%, 2-20%, 3-10%등장확률에 따른 등급출현) ->
        //2. 등급에 해당하는 수치 4개중 랜덤 한개 선택함.
        //3. -> List<WeaponBase>.Count로 개수받은뒤에 랜덤으로 한개 뽑아서 무기 뽑음.
        //4. 뽑은무기.UpgradeWeapon(string variableName,float value)
        //이걸로 해당하는 값을 바꿔줌.
        int rarity = GetWhichRaritySelected(); //등급이 뜰 확률에 따라 등급 뽑음.
        Debug.Log(rarity);
        string abilityName = PickAbility(); //동등한 확률에 따라 올릴 능력치를 뽑음.
        WeaponBase weaponType = PickWeaponType(); //무기 종류도 랜덤하게 선택.

        float amount = GetUpgradeAmount(abilityName, rarity); //등급과 능력치에 따라 상승시킬 양 계산.
        weaponType.UpgradeWeapon(abilityName, amount); //무기에 적용. 이건 나중에 함수로 빼서 선택되었을때 Upgrade하도록 바꿈.
    }

    /// <summary>
    /// 등급이 뜰 확률에 따라 등급 뽑음.
    /// </summary>
    /// <returns></returns>
    public int GetWhichRaritySelected()
    {
        float total=0;
        for (int i = 0; i < infoPerRarity.Count; i++)
        {
            total += infoPerRarity[i].AppearProbability; //합을 구함.   
        }

        float randomValue = Random.Range(0, total);

        float current = 0;
        for (int i = 0; i < infoPerRarity.Count; i++)
        {
            current += infoPerRarity[i].AppearProbability;
            if (randomValue < current)
            {
                return i;
            }
        }

        return 0;
    }

    /// <summary>
    /// 올릴 능력치를 랜덤으로 뽑는다.
    /// </summary>
    /// <returns></returns>
    public string PickAbility()
    {
        int randomValue = Random.Range(0, 4);
        string name = Enum.GetName(typeof(CardAbility), randomValue);
        return name;
    }

    /// <summary>
    /// 무기 종류 중 랜덤하게 하나 뽑아서 무기종류 반환.
    /// </summary>
    /// <returns></returns>
    public WeaponBase PickWeaponType()
    {
        int randomValue = Random.Range(0, weapons.Count);
        return weapons[randomValue];
    }
    
    /// <summary>
    /// 싱글톤으로 선언된 3종류의 WeaponManager로부터 무기들 종류를 받아서 List<WeaponBase> weapon에다가 등록한다.
    /// </summary>
    public void GetWeaponList()
    {
        //싱글톤으로 선언된 무기별 WeaponManager에서 무기종류를 더한다
        //예를 들어 OrbitalWeaponManager는 List<OrbitalWeapon>인데 OrbitalWeapon은 WeaponBase를 상속받고 있으므로 
        //List<WeaponBase> weapons에다가 집어 넣을 수 있다.. 


        weapons.Clear(); //리스트 초기화 시킨 후 다시 받아온다.
        
        //OrbitalWeapon들을 가져온다.
        for (int i = 0; i < orbitalManager._orbitalWeapons.Count; i++)
        {
            weapons.Add(orbitalManager._orbitalWeapons[i]);
        }
        //다른무기들도 나중에 가져온다.
    }
    
    /// <summary>
    /// 등급에 따라 상승시킬 수치값을 얻음
    /// </summary>
    /// <param name="variableName"></param>
    /// <param name="rarity"></param>
    /// <returns></returns>
    public float GetUpgradeAmount(string variableName,int rarity)
    {
        switch (variableName)
        {
            case "weaponDamage" :
                return infoPerRarity[rarity].damageMultiplier;
            case "weaponAttackSpeed" :
                return infoPerRarity[rarity].attackSpeedMultiplier;
            case "projectileCount" :
                return infoPerRarity[rarity].projectileAdd;
            case "weaponRange" :
                return infoPerRarity[rarity].rangeMultiplier;
        }
        return 0;
    }
}


