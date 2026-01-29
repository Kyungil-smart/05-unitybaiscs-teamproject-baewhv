using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpPopupUI : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private GameObject _popupObject;
    [SerializeField] private CardUI[] _uiCards;
    
    [Header("System Reference")]
    [SerializeField] private List<CardInfoPerRarity> _uiRarityInfos;
    [SerializeField] private CardManager _cardManager;

    private void Start()
    {
        if (_popupObject != null) _popupObject.SetActive(false);
    }

    /// <summary>
    /// 레벨업 시 팝업 표시
    /// </summary>
    public void ShowPopup()
    {
        // 게임 정지
        Time.timeScale = 0f;
        _popupObject.SetActive(true);

        if (_cardManager != null)
        {
            // CardManager의 기존 DrawCard 함수 사용
            Card[] drawnCards = _cardManager.DrawCard(3);

            for (int i = 0; i < _uiCards.Length; i++)
            {
                if (i < drawnCards.Length)
                {
                    _uiCards[i].gameObject.SetActive(true);
                    
                    // CardManager의 리스트에서 등급 정보를 가져옴
                    CardInfoPerRarity rarityInfo = _uiRarityInfos[drawnCards[i].rarity];
                    _uiCards[i].Setup(drawnCards[i], rarityInfo, this);
                }
                else
                {
                    _uiCards[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void ClosePopup()
    {
        _popupObject.SetActive(false);
        // 게임 재개
        Time.timeScale = 1f; 
    }
}