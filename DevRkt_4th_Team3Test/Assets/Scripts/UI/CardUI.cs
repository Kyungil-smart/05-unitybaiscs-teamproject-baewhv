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
    private System.Action<Card> _onClickCallback;
    
    /// <summary>
    /// 데이터를 받아서 Card UI에 표시
    /// </summary>
    public void Setup(Card data, CardInfoPerRarity rarityInfo, System.Action<Card> callback)
    {
        if (data == null || data.weapon == null)
        {
            Debug.LogError("전달된 카드 데이터나 무기 정보가 Null입니다!");
            return;
        }
        
        _cardData = data;
        _onClickCallback = callback;
        
        //이름 표시
        if (_weaponNameText != null && data.weapon._weaponName != null)
        {
            _weaponNameText.text = data.weapon._weaponName;
        }
        //이미지 표시
        if (_weaponIcon != null && data.weapon.weaponSprite != null)
        {
            _weaponIcon.sprite = data.weapon.weaponSprite; 
        }

        if (_rarityText != null)
        {
            _weaponIcon.preserveAspect = true;
        }
        //등급 표시
        if (_rarityFrame != null && rarityInfo.RarityColor != null)
        {
            Color _color = rarityInfo.RarityColor;
            _color.a = 250f;
            _rarityFrame.color = _color;
        }

        if (_rarityText != null && rarityInfo.RarityName != null)
        {
            _rarityText.text = rarityInfo.RarityName;
        }
        //새로 얻은 아이템 인지 표시
        if (data.isNew)
        {
            _newTag.SetActive(true);
            
        }

        if (data.weapon == null) return;
        
        if (_damageText != null && data.weapon.weaponDamage != null)
        {
            _damageText.text = $"Damage: +{data.weapon.weaponDamage}%";
        }
        if (_speedText != null && data.weapon.weaponAttackSpeed != null)
        {
            _speedText.text = $"Speed: +{data.weapon.weaponAttackSpeed}";
        }
        if (_rangeText != null && data.weapon.projectileCount != null)
        {
            _rangeText.text = $"Range: +{data.weapon.projectileCount}%";
        }
    }

    public void OnClickCard()
    {
        if (_cardData == null) return;
        _onClickCallback?.Invoke(_cardData);
    }
}