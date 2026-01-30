using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    [Header("Image UI")]
    [SerializeField] private Image _weaponIcon;
    [SerializeField] private Image _rarityFrame;
    
    [Header("Text UI")]
    [SerializeField] private TextMeshProUGUI _weaponNameText;
    [SerializeField] private TextMeshProUGUI _damageText;
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private TextMeshProUGUI _rangeText;
    [SerializeField] private TextMeshProUGUI _rarityText;
    [SerializeField] private GameObject _newTag;

    private Card _cardData;
    private LevelUI _levelUI;

    /// <summary>
    /// 데이터를 받아서 Card UI에 표시
    /// </summary>
    public void Setup(Card data, CardInfoPerRarity rarityInfo)
    {
        _cardData = data;
        
        //이름 표시
        _weaponNameText.text = data.weapon._weaponName;
        //이미지 표시
        _weaponIcon.sprite = data.weapon.weaponSprite; 
        _weaponIcon.preserveAspect = true;
        //등급 표시
        _rarityFrame.color = rarityInfo.RarityColor;
        _rarityText.text = rarityInfo.RarityName;
        //새로 얻은 아이템 인지 표시
        if (data.isNew)
        {
            _newTag.SetActive(true);
            
        }
        //if(data.abilityName == CardAbility.weaponDamage)
        _damageText.text = $"Damage: +{data.weapon.weaponDamage}%";
        _speedText.text = $"Speed: +{data.weapon.weaponAttackSpeed}";
        _rangeText.text = $"Range: +{data.weapon.projectileCount}%";
    }

    public void OnClickCard()
    {
        if (_cardData == null) return;

        // CardManager의 기존 ApplyCardEffect 함수를 그대로 호출
        FindObjectOfType<CardManager>().ApplyCardEffect(_cardData);
       
        // LevelUI를 통해 팝업을 닫고 게임 재개
    }
}