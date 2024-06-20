using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollection : MonoBehaviour
{
    [field: SerializeField] public List<ScriptableCard> CommonCards { get; private set; } = new List<ScriptableCard>();
    [field: SerializeField] public List<ScriptableCard> RareCards { get; private set; } = new List<ScriptableCard>();
    [field: SerializeField] public List<ScriptableCard> EpicCards { get; private set; } = new List<ScriptableCard>();
    [field: SerializeField] public List<ScriptableCard> LegendaryCards { get; private set; } = new List<ScriptableCard>();

    public ScriptableCard GetRandomCard(CardRarity rarity)
    {
        List<ScriptableCard> selectedList = null;

        switch (rarity)
        {
            case CardRarity.Common:
                selectedList = CommonCards;
                break;
            case CardRarity.Rare:
                selectedList = RareCards;
                break;
            case CardRarity.Epic:
                selectedList = EpicCards;
                break;
            case CardRarity.Legendary:
                selectedList = LegendaryCards;
                break;
        }

        if(selectedList == null || selectedList.Count == 0)
        {
            Debug.Log($"No Cards Available for rarity {rarity}");
            return null;
        }

        return selectedList[Random.Range(0, selectedList.Count)];
    }
}
