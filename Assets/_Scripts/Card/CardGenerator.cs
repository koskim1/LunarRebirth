using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator
{
    private CardCollection cardCollection;

    public CardGenerator(CardCollection cardCollection)
    {
        this.cardCollection = cardCollection;
    }

    public ScriptableCard GenerateCard()
    {
        float randomValue = Random.Range(0f, 100f);

        if(randomValue < 60f)
        {
            if (cardCollection.CommonCards.Count == 0) throw new System.Exception("No Common Cards");
            return cardCollection.CommonCards[Random.Range(0, cardCollection.CommonCards.Count)];
        }
        else if(randomValue < 90f)
        {
            if (cardCollection.RareCards.Count == 0) throw new System.Exception("No RareCards Cards");
            return cardCollection.RareCards[Random.Range(0, cardCollection.RareCards.Count)];
        }
        else if(randomValue < 99f)
        {
            if (cardCollection.EpicCards.Count == 0) throw new System.Exception("No EpicCards Cards");
            return cardCollection.EpicCards[Random.Range(0, cardCollection.EpicCards.Count)];
        }
        else
        {
            if (cardCollection.LegendaryCards.Count == 0) throw new System.Exception("No LegendaryCards Cards");
            return cardCollection.LegendaryCards[Random.Range(0, cardCollection.LegendaryCards.Count)];
        }
    }
}
