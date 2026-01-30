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

public class CardManager : MonoBehaviour
{
    public static CardManager CardInstance;
    
    // 인스펙터에서 등급에 따른 카드에서 나올 수 있는 능력치 설정

    public List<CardInfoPerRarity> infoPerRarity = new List<CardInfoPerRarity>();
    public Dictionary<int, float> probabilityOfRarity = new Dictionary<int, float>(); //쓰기 애매
    
    // 카드는 1.무기 종류 랜덤, 2.등급 랜덤, 3.상승시킬 능력치 종류 랜덤으로 나옴.
    
    
    [Header("능력치 최대값")]
    [SerializeField] private float _maxWeaponDamage= 100000f;
    [SerializeField] private float _maxWeaponAttackSpeed = 500f;
    [SerializeField] private float _maxProjectileCount = 10;
    [SerializeField] private float _maxRangedWeaponAttackSpeed = 100f; //RangedWeapon은 별도의 공속최대값을 가집니다.
    public int cardCount = 3; //테스트용 카드 뽑을 개수.
    public int promotionCriteria = 10; // 승급할때의 레벨 기준
    
    private void Awake()
    {
        if (CardInstance == null)
        {
            CardInstance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
    }
    
    
    private void Start()
    {
        
        
    }

    private void Update()
    {
        //테스트용도. B눌러서 카드 뽑기 + 뽑은 카드 전부 적용 테스트 하실수 있습니다 
        if(Input.GetKeyDown(KeyCode.B))
        {
            WeaponManager.WeaponInstance.GetWeaponList();
            Card[] cardData = new Card[cardCount];
            for (int i = 0; i < cardCount; i++)
            {
                cardData[i] = new Card();
            }
        
            cardData = DrawCard(cardCount);

            for (int i = 0; i < cardData.Length; i++)
            {
                CardApply(cardData[i]);    
            }
            
        }
        
    }
    
    private void OnDestroy()
    {
        if (CardInstance == this) 
        {
            CardInstance = null;
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
            cardData[i].weapon = PickWeaponType(); //무기 종류도 랜덤하게 선택.

            //무기를 뽑았을때 만약 무기레벨 + 1 이 promotionCriteria이상이면 isEvolution = true;
            //TODO UI에서 card의 isPromotion이 true가 되면 cardData[i].weapon.upgradeWeapon._weaponName으로 cardData[i].weapon._weaponName를 바꾸는 승진카드를 보여주시면 좋을것같습니다.
            if (cardData[i].weapon.level + 1 >= promotionCriteria)
            {
                cardData[i].isPromotion = true;
            }
            
            //활성화 안되어있으면 활성화 시킴. 
            //cardData.isNew가 true이면 카드 클릭시 ApplyCardEffect 대신에 cardData[i].weapon.isActive = true로 바꿈. 
            if (cardData[i].weapon.isActive == false)
            {
                cardData[i].isNew = true;
            }
            cardData[i].rarity = PickRarity(); //등급이 뜰 확률에 따라 등급 뽑음.
            cardData[i].abilityName = PickAbility(cardData[i].weapon); //동등한 확률에 따라 올릴 능력치를 뽑음.
            cardData[i].amountToApply = GetUpgradeAmount(cardData[i].abilityName, cardData[i].rarity); //등급과 능력치에 따라 상승시킬 양 계산.
        }

        return cardData;

    }

    /// <summary>
    /// 카드 클릭할때 이함수로 CardData의 정보를 무기에 반영 
    /// </summary>
    /// <param name="cardData"></param>
    public void ApplyCardEffect(Card cardData)
    {
        cardData.weapon.level += 1;
        //카드를 선택했을때 무기 레벨이 10이 넘으면 승급합니다.  
        if (cardData.weapon.level >= promotionCriteria && cardData.weapon.promotionPrefab != null)
        {
            Debug.Log($"{cardData.weapon._weaponName}이 승급!");
            EvolveWeapon(cardData.weapon);
        }
        //승급을 하지 않는경우 능력치 적용
        else
        {
            cardData.weapon.UpgradeWeapon(cardData.abilityName, cardData.amountToApply); //무기에 적용.
            
            //만약 궤도무기를 뽑았으면 SpawnWeapon를 통해 스폰시킴.
             if ((cardData.weapon is OrbitalWeapon))
             {
                OrbitalWeaponManager.OrbitalInstance.SpawnWeapons((cardData.weapon as OrbitalWeapon));    
             }
        }
         
    }

    /// <summary>
    /// 카드 효과 적용. 활성화안된무기 활성화.
    /// </summary>
    /// <param name="cardData"></param>
    public void CardApply(Card cardData)
    {
            //카드를 선택했을때.(함수화 시켜줘야함 나중에)
            if (cardData.isNew == true)
            {//원래는 무기가 처음 선택되는거면 무기카드가 보이게 할려고 했는데 그건 구현하기가 어렵기 때문에 그냥 처음보는것도 능력치변경되는 걸로.
                cardData.weapon.isActive = true;
                ApplyCardEffect(cardData);
            }
            else
            {
                ApplyCardEffect(cardData);    
            }
    }
    
    /// <summary>
    /// 등급이 뜰 확률에 따라 등급 뽑음.
    /// </summary>
    /// <returns></returns>
    public int PickRarity()
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
    public CardAbility PickAbility(WeaponBase weapon)
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
        
        //RangedWeapon에 한해서는  RangedWeapon전용 attackSpeed 최대값을 적용 
        if (weapon is RangedWeapon && weapon.weaponAttackSpeed >= _maxRangedWeaponAttackSpeed)
        {
            list.Remove(2);
        }
        
        if (weapon.projectileCount >= _maxProjectileCount)
        {
            list.Remove(2);
        }

        
        
        //만약 무기가 원거리나 근거리 무기면 ProjectileCount제외
        if (!(weapon is OrbitalWeapon))
        {
            list.Remove(2);
        }
        
        int randomValue = Random.Range(0, list.Count);
        
        CardAbility name = (CardAbility)list[randomValue];
        return name;
    }

    /// <summary>
    /// 무기 종류 중 랜덤하게 하나 뽑아서 무기종류 반환.
    /// </summary>
    /// <returns></returns>
    public WeaponBase PickWeaponType()
    {
        List<WeaponBase> validWeapons = new List<WeaponBase>(); //뽑을 수 있는 무기만 뽑음(승진무기, 승진된 후 남은 기존 무기 제외)
        List<WeaponBase> allWeapons = WeaponManager.WeaponInstance.weapons; 

        for (int i = 0; i < allWeapons.Count; i++)
        {
            if (allWeapons[i].isActive == true)
            {
                validWeapons.Add(allWeapons[i]);
            }
            //승진 무기, 승진된 후 남은 기존 무기는 추가X
            else if(allWeapons[i].isPromotionWeapon == true || allWeapons[i].isUpgradeFinished == true)
            {
                continue;
            }
            else
            {
                validWeapons.Add(allWeapons[i]);
            }
        }
        
        int randomValue = Random.Range(0, validWeapons.Count); //저장된 무기 수
        return validWeapons[randomValue]; //무기자체를 리턴
    }
    
    
    
    /// <summary>
    /// 등급에 따라 상승시킬 수치값을 얻음
    /// </summary>
    /// <param name="variableName"></param>
    /// <param name="rarity"></param>
    /// <returns></returns>
    public float GetUpgradeAmount(CardAbility variableName,int rarity)
    {
        switch (variableName)
        {
            case CardAbility.weaponDamage :
                return infoPerRarity[rarity].damageMultiplier;
            case CardAbility.weaponAttackSpeed :
                return infoPerRarity[rarity].attackSpeedMultiplier;
            case CardAbility.projectileCount :
                return infoPerRarity[rarity].projectileAdd;
            case CardAbility.weaponRange :
                return infoPerRarity[rarity].rangeMultiplier;
        }
        return 0;
    }

    public void EvolveWeapon(WeaponBase weapon)
    {
        //
        WeaponBase promotionPrefabWeaponBase = weapon.promotionPrefab.GetComponent<WeaponBase>();
        string nextWeaponName = promotionPrefabWeaponBase._weaponName;
        weapon.isUpgradeFinished = true; //업그레이드 된 후 남은 무기라는 표시 해줌.
        
        //무기 전체 중에 다음 무기와 일치하는 것 있는지 확인
        for (int i = 0; i < WeaponManager.WeaponInstance.weapons.Count; i++)
        {
            //승진 무기가 있으면 기존 무기 isActive = false, 새로운 무기 isActive = true;
            if (nextWeaponName == WeaponManager.WeaponInstance.weapons[i]._weaponName)
            {
                weapon.isActive = false; //기존무기 false
                WeaponManager.WeaponInstance.weapons[i].isActive = true; //승진 무기 true
                //rangedWeapon은 그냥 해도 되는데 orbitalWeapon은 clearweapon한뒤에 spawnweapon도 함.
                if (weapon is OrbitalWeapon)
                {
                    OrbitalWeaponManager.OrbitalInstance.ClearWeapons(weapon as OrbitalWeapon);//기존 궤도 무기 제거.
                    OrbitalWeaponManager.OrbitalInstance.SpawnWeapons(
                        WeaponManager.WeaponInstance.weapons[i] as OrbitalWeapon); //승진 무기 생성.
                }

                break;
            }
        }
    }
}


