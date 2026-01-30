using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public int rarity;
    public WeaponBase weapon; 
    public CardAbility abilityName;
    public bool isNew = false; //무기를 새로 만드는 카드를 뽑을 경우.
    public bool isPromotion = false;
}
