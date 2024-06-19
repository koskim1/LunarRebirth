using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator
{
    public List<ScriptableCard> commonCards = new List<ScriptableCard>();
    public List<ScriptableCard> rareCards = new List<ScriptableCard>();
    public List<ScriptableCard> epicCards = new List<ScriptableCard>();
    public List<ScriptableCard> legendaryCards = new List<ScriptableCard>();

    public CardGenerator()
    {
        // 카드 목록 초기화
        // Add example cards to each list
        commonCards.Add(new ScriptableCard { CardName = "Common Card 1", CardDescription = "Common Description 1", Rarity = CardRarity.Common });
        rareCards.Add(new ScriptableCard { CardName = "Rare Card 1", CardDescription = "Rare Description 1", Rarity = CardRarity.Rare });
        epicCards.Add(new ScriptableCard { CardName = "Epic Card 1", CardDescription = "Epic Description 1", Rarity = CardRarity.Epic });
        legendaryCards.Add(new ScriptableCard { CardName = "Legendary Card 1", CardDescription = "Legendary Description 1", Rarity = CardRarity.Legendary });

        // 확인용 로그 출력
        Debug.Log($"Common Cards: {commonCards.Count}");
        Debug.Log($"Rare Cards: {rareCards.Count}");
        Debug.Log($"Epic Cards: {epicCards.Count}");
        Debug.Log($"Legendary Cards: {legendaryCards.Count}");
    }

    private ScriptableCard CreateCard(string name, string description, Sprite image, CardElement element, CardRarity rarity)
    {
        ScriptableCard card = ScriptableObject.CreateInstance<ScriptableCard>();
        card.Initialize(name, description, image, element, rarity);
        return card;
    }

    public ScriptableCard GenerateCard()
    {
        float randomValue = Random.Range(0f, 100f);

        if(randomValue < 70f)
        {
            return commonCards[Random.Range(0, commonCards.Count)];
        }
        else if(randomValue < 90f)
        {
            return rareCards[Random.Range(0, rareCards.Count)];
        }
        else if(randomValue < 99f)
        {
            return epicCards[Random.Range(0, epicCards.Count)];
        }
        else
        {
            return legendaryCards[Random.Range(0, legendaryCards.Count)];
        }
    }
}
