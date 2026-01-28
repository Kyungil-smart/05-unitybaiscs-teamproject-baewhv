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
    weaponDamage, weaponAttackSpeed, projectileCount
}

public class CardManager : MonoBehaviour
{
    //[SerializeField] private OrbitalWeaponManager orbitalManager;
    
    // 인스펙터에서 등급에 따른 카드에서 나올 수 있는 능력치 설정
    [SerializeField] private List<CardInfoPerRarity> infoPerRarity = new List<CardInfoPerRarity>();
    public Dictionary<int, float> probabilityOfRarity = new Dictionary<int, float>(); //쓰기 애매
    
    // 카드는 1.무기 종류 랜덤, 2.등급 랜덤, 3.상승시킬 능력치 종류 랜덤으로 나옴.
    //TODO 무기를 담는 변수. 나중에 InventoryData랑 연결.....
    [SerializeField] private List<WeaponBase> weapons = new List<WeaponBase>();
    
    [Header("능력치 최대값")]
    [SerializeField] private float _maxWeaponDamage= 100000f;
    [SerializeField] private float _maxWeaponAttackSpeed = 500f;
    [SerializeField] private float _maxProjectileCount = 10;
    
    public int cardCount = 3;
    
   
    
    private void Start()
    {
        
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetWeaponList();
            Card[] cardData = new Card[cardCount];
            for (int i = 0; i < cardCount; i++)
            {
                cardData[i] = new Card();
            }
        
            cardData = DrawCard(cardCount);

            for (int i = 0; i < cardCount; i++)
            {
                ApplyCardEffect(cardData[i]);    
            }
        }
        
    }
    
    /// <summary>
    /// 레어도, 능력치, 무기종류를 랜덤으로 결정해서 카드를 만듦.
    /// 카드를 cardCount개 반환함
    /// </summary>
    public Card[] DrawCard(int count)
    {
        //1. 카드를 뽑고(0-40%, 1-30%, 2-20%, 3-10%등장확률에 따른 등급출현) ->
        //2. 등급에 해당하는 수치 4개중 랜덤 한개 선택함.
        //3. -> List<WeaponBase>.Count로 개수받은뒤에 랜덤으로 한개 뽑아서 무기 뽑음.
        //4. 뽑은무기.UpgradeWeapon(string variableName,float value)
        //이걸로 해당하는 값을 바꿔줌.
        Card[] cardData = new Card[count];
       
        //카드마다 랜덤 데이터를 저장
        for (int i = 0; i < count; i++)
        {
            cardData[i] = new Card();
            cardData[i].rarity = GetWhichRaritySelected(); //등급이 뜰 확률에 따라 등급 뽑음.
            Debug.Log(cardData[i].rarity);
            cardData[i].weapon = PickWeaponType(); //무기 종류도 랜덤하게 선택.
            cardData[i].abilityName = PickAbility(cardData[i].weapon); //동등한 확률에 따라 올릴 능력치를 뽑음.
            
        }

        return cardData;

    }

    /// <summary>
    /// 카드 클릭할때 이함수로 CardData의 정보를 무기에 반영 
    /// </summary>
    /// <param name="cardData"></param>
    public void ApplyCardEffect(Card cardData)
    {
        float amount = GetUpgradeAmount(cardData.abilityName, cardData.rarity); //등급과 능력치에 따라 상승시킬 양 계산.
        cardData.weapon.UpgradeWeapon(cardData.abilityName, amount); //무기에 적용.
        
        //만약 궤도무기를 뽑았고. abilityName = projectileCount이면 SpawnWeapon를 통해 스폰시킴.
        if ((cardData.weapon is OrbitalWeapon) && cardData.abilityName == "projectileCount")
        {
            OrbitalWeaponManager.OrbitalInstance.SpawnWeapons((cardData.weapon as OrbitalWeapon));    
        }
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
    public string PickAbility(WeaponBase weapon)
    {
        
        // cardData[i].weapon을 받아서 cardData[i].weapon.projectileCount의 값이 10이상이면 제외. 나머지도 비슷하게.
        List<int> list = new List<int>();
        list.Add(0); list.Add(1); list.Add(2);

        // if (weapon.weaponDamage >= _maxWeaponDamage)
        // {
        //     list.Remove(0);
        // }

        if (weapon.weaponAttackSpeed >= _maxWeaponAttackSpeed)
        {
            list.Remove(1);
        }
        if (weapon.projectileCount >= _maxProjectileCount)
        {
            list.Remove(2);
        }
        int randomValue = Random.Range(0, list.Count);
        
        string name = Enum.GetName(typeof(CardAbility), list[randomValue]);
        return name;
    }

    /// <summary>
    /// 무기 종류 중 랜덤하게 하나 뽑아서 무기종류 반환.
    /// </summary>
    /// <returns></returns>
    public WeaponBase PickWeaponType()
    {
        int randomValue = Random.Range(0, weapons.Count); //저장된 무기 수
        return weapons[randomValue]; //무기자체를 리턴
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
        for (int i = 0; i < OrbitalWeaponManager.OrbitalInstance._orbitalWeapons.Count; i++)
        {
            weapons.Add(OrbitalWeaponManager.OrbitalInstance._orbitalWeapons[i]);
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


