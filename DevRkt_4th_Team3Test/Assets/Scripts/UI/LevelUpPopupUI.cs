using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpPopupUI : MonoBehaviour
{
    [Header("Card UI")] [SerializeField] private GameObject _cardUI;
    [SerializeField] private Transform _cardList;
    private Card[] _cards;

    [Header("System Reference")] private CardManager _cardManager;

    /// <summary>
    /// 레벨업 시 팝업 표시
    /// </summary>
    public void ShowPopup()
    {
        // 게임 정지
        Time.timeScale = 0f;
        gameObject.SetActive(true);

        if (_cardManager == null)
            _cardManager = FindObjectOfType<CardManager>();

        if (_cardManager == null) return;

        // 이전 카드 UI들 삭제
        foreach (Transform child in _cardList)
        {
            Destroy(child.gameObject);
        }

        //추가
        _cards = _cardManager.DrawCard(3);

        foreach (var card in _cards)
        {
            Debug.Log(card.abilityName);
            SetCardUI(card);
        }
    }

    public void SetCardUI(Card card)
    {
        if (_cardUI == null) return;

        // 카드 생성 및 카드가 나열될 부모 설정
        Transform cardList = transform.Find("CardList");
        GameObject newCardObj = Instantiate(_cardUI, cardList);
        CardUI cardUI = newCardObj.GetComponent<CardUI>();

        if (cardUI != null)
        {
            // CardManager의 등급 정보를 가져오기
            CardInfoPerRarity rarityInfo = _cardManager.infoPerRarity[card.rarity];
            cardUI.Setup(card, rarityInfo, OnCardSelected);
        }
    }

    
    private void OnCardSelected(Card card)
    {
        // 카드 적용
        _cardManager.CardApply(card);
    
        // 팝업 닫기
        ClosePopup();
    }
    
    public void ClosePopup()
    {
        gameObject.SetActive(false);
        // 게임 재개
        Time.timeScale = 1f;
    }
    
}