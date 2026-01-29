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
    [SerializeField] private GameObject _newTag;

    private Card _cardData;
    private LevelUI _levelUI;
    private LevelUpPopupUI _levelUpPopupUI;

    /// <summary>
    /// 데이터를 받아서 Card UI에 표시
    /// </summary>
    public void Setup(Card data, CardInfoPerRarity rarityInfo, LevelUpPopupUI manager)
    {
        _cardData = data;
        
        _weaponNameText.text = data.weapon._weaponName;
        _weaponIcon.sprite = data.weapon.weaponSprite; 
        _weaponIcon.preserveAspect = true;
        
        _rarityFrame.color = rarityInfo.RarityColor;
        _newTag.SetActive(data.isNew);

        float amount = FindObjectOfType<CardManager>().GetUpgradeAmount(data.abilityName, data.rarity);
        
        if (data.abilityName == "weaponDamage") _damageText.text = $"Damage: +{amount * 100}%";
        else if (data.abilityName == "weaponAttackSpeed") _speedText.text = $"Speed: +{amount}";
        else if (data.abilityName == "projectileCount") _rangeText.text = $"Projectile: +{amount}";
        else if (data.abilityName == "weaponRange") _rangeText.text = $"Range: +{amount * 100}%";
        
        _levelUpPopupUI = manager;
    }

    public void OnClickCard()
    {
        if (_cardData == null) return;

        // CardManager의 기존 ApplyCardEffect 함수를 그대로 호출
        FindObjectOfType<CardManager>().ApplyCardEffect(_cardData);
        
        // LevelUI를 통해 팝업을 닫고 게임 재개
        if (_levelUI != null) _levelUpPopupUI.ClosePopup();
    }
}