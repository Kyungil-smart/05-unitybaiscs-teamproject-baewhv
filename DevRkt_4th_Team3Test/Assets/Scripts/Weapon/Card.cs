using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public int rarity;
    public WeaponBase weapon; 
    public CardAbility abilityName;
    public bool isNew = false; //무기를 새로 만드는 카드를 뽑을 경우.
    public bool isPromotion = false; //todo 카드를 뽑았을때 isPromotion이 true이면 승급할 무기(card.weapon.promotionPrefab)의 정보(prefab.GetComponent<WeaponBase>();으로 얻은 정보를 보여줍니다.
    public float amountToApply; //todo 변수추가. 카드를 뽑았을때 적용할 능력치의 수치입니다.
}
