using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    [Header("Image UI")] [SerializeField] private Image _weaponIcon;
    [SerializeField] private Image _rarityFrame;

    [Header("Text UI")] [SerializeField] private TextMeshProUGUI _weaponNameText;
    [SerializeField] private GameObject _newTag;
    [SerializeField] private GameObject _upTag;
    [SerializeField] private TextMeshProUGUI _rarityText;
    [SerializeField] private TextMeshProUGUI _abilityName;
    [SerializeField] private TextMeshProUGUI _amountToApply;

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
            if (data.isPromotion)
            {
                WeaponBase promotionWeapon = data.weapon.promotionPrefab.GetComponent<WeaponBase>();
                _weaponNameText.text = $"{data.weapon._weaponName}\n↓\n{promotionWeapon._weaponName}";
            }
            else
            {
                _weaponNameText.text = data.weapon._weaponName;
            }
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
        _newTag.SetActive(data.isNew);

        //승급 표시
        _upTag.SetActive(data.isPromotion);

        if (data.isPromotion)
        {
            _abilityName.enabled = false;
            _amountToApply.enabled = false;
        }
        else
        {
            if (_abilityName != null && data.abilityName != null)
            {
                _abilityName.text = GetAbilityNameKo(data.abilityName);
            }

            if (_amountToApply != null && data.amountToApply != null)
            {
                _amountToApply.text = $"{data.amountToApply}";
            }
        }
   
    }

    private string GetAbilityNameKo(CardAbility text)
    {
        switch (text)
        {
            case CardAbility.projectileCount: return "투사체 개수";
            case CardAbility.weaponAttackSpeed: return "무기 공격 속도";
            case CardAbility.weaponDamage: return "무기 공격력";
            case CardAbility.weaponRange:  return "무기 사거리";
            default: return $"{text}";
        }
    }

    public void OnClickCard()
    {
        if (_cardData == null) return;
        _onClickCallback?.Invoke(_cardData);
    }
}