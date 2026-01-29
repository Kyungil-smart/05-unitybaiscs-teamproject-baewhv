using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image _weaponIcon;
    [SerializeField] private TextMeshProUGUI _weaponNameText;
    [SerializeField] private TextMeshProUGUI _abilityInfoText;
    [SerializeField] private Image _rarityFrame;
    [SerializeField] private GameObject _newTag;

    private Card _cardData;

    public void Setup(Card data, CardInfoPerRarity rarityInfo)
    {
        _cardData = data;

        //_weaponIcon.sprite = data.weapon.weaponSprite; 
        //_weaponNameText.text = data.weapon.weaponName;
        
        _abilityInfoText.text = $"{data.abilityName} Up!";
        _rarityFrame.color = rarityInfo.RarityColor;
        
        _newTag.SetActive(data.isNew);
    }

    public void OnClickCard()
    {
        FindObjectOfType<CardManager>().ApplyCardEffect(_cardData);
        // TODO: 팝업 닫기 및 게임 재개 로직 추가 필요
    }
}