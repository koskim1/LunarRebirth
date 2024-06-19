using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds all data for each individual card
/// </summary>
/// 

[CreateAssetMenu(menuName = "CardData")] // lets you create a new CardData Objcet withe the right-click menu in the editor
public class ScriptableCard : ScriptableObject
{
    // field : SerializeField lets you reveal properties in the inspector like they were public fields
    [field: SerializeField] public string CardName { get; set; }
    [field:SerializeField, TextArea] public string CardDescription { get; set; }
    [field: SerializeField] public Sprite Image { get; set; }

    [field:SerializeField] public CardElement Element { get; set; }
    [field: SerializeField] public CardRarity Rarity { get; set; }

    public void Initialize(string cardName, string cardDescription, Sprite image, CardElement element, CardRarity rarity)
    {
        CardName = cardName;
        CardDescription = cardDescription;
        Image = image;
        Element = element;
        Rarity = rarity;
    }
}

public enum CardElement
{
    Basic,
    Ice,
    Fire,
    Lightning
}

public enum CardRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}
