using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cardRarity
{
    Normal,
    Rare,
    Epic,
    Legendary
}

public class Card
{
    public string name;
    public string description;
    public cardRarity rarity;
    public Ability ability;

    public Card(string name, string description, cardRarity rarity, Ability ability)
    {
        this.name = name;
        this.description = description;
        this.rarity = rarity;
        this.ability = ability;
    }
}

public class CardGenerator
{
    private List<Card> normalCards = new List<Card>();
    private List<Card> rareCards = new List<Card>();
    private List<Card> epicCards = new List<Card>();
    private List<Card> legendaryCards = new List<Card>();

    public CardGenerator()
    {
        // ���÷� �Ϻ� ī�常 �߰��մϴ�. �ʿ信 ���� �� ���� ī�带 �߰�.
        normalCards.Add(new Card("Normal card 1", "Description 1", cardRarity.Normal, null));
        rareCards.Add(new Card("Fireball", "Throws a firaball", cardRarity.Rare, new Ability("Fireball", "Throws a fireball", new FireballEffect)));

    }
}