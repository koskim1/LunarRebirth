using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Will update the UI-visuals of each card, depending on it's data
/// </summary>

public class CardUI : MonoBehaviour
{
    private Card _card;

    [Header("Prefabs Elements")]
    [SerializeField] private Image _cardImage;
    [SerializeField] private Image _elementBackground;
    [SerializeField] private Image _rarityBackground;

    [SerializeField] private TextMeshProUGUI _cardName;
    [SerializeField] private TextMeshProUGUI _cardType;
    [SerializeField] private TextMeshProUGUI _cardDescription;

    [Header("Sprite Assets")]
    [SerializeField] private Sprite _fireElementBackground;
    [SerializeField] private Sprite _iceElementBackground;
    [SerializeField] private Sprite _lightningElementBackground;

    [SerializeField] private Sprite _commonRarityBackground;
    [SerializeField] private Sprite _rareRarityBackground;
    [SerializeField] private Sprite _epicRarityBackground;
    [SerializeField] private Sprite _legendaryRarityBackground;

    private void Awake()
    {
        _card = GetComponent<Card>();
        SetCardUI();
    }

    private void OnValidate()
    {
        Awake();
    }

    private void SetCardUI()
    {
        if(_card != null && _card.CardData != null)
        {
            SetCardTexts();
            SetRarityBackground();
            SetElementFrame();
            SetCardImage();
        }
    }

    private void SetCardTexts()
    {
        _cardName.text = _card.CardData.CardName;
        _cardDescription.text = _card.CardData.CardDescription;
    }


    private void SetRarityBackground()
    {
        switch (_card.CardData.Rarity)
        {
            case CardRarity.Common:
                _rarityBackground.sprite = _commonRarityBackground;
                _cardType.text = "Common";
                break;
            case CardRarity.Rare:
                _rarityBackground.sprite = _rareRarityBackground;
                _cardType.text = "Rare";
                break;
            case CardRarity.Epic:
                _rarityBackground.sprite = _epicRarityBackground;
                _cardType.text = "Epic";
                break;
            case CardRarity.Legendary:
                _rarityBackground.sprite = _legendaryRarityBackground;
                _cardType.text = "Legendary";
                break;
        }
    }

    private void SetElementFrame()
    {
        switch (_card.CardData.Element)
        {
            case CardElement.Basic:
                // do nothing - basic background
                break;
            case CardElement.Ice:
                _elementBackground.sprite = _iceElementBackground;
                break;
            case CardElement.Fire:
                _elementBackground.sprite = _fireElementBackground;
                break;
            case CardElement.Lightning:
                _elementBackground.sprite = _lightningElementBackground;
                break;
        }
    }

    private void SetCardImage()
    {
        _cardImage.sprite = _card.CardData.Image;
    }


}
