using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ShopItem")]
public class ShopItem : ScriptableObject
{
    public Sprite itemIcon;
    public LocalizedString itemName;
    public LocalizedString itemDescription;
    public int itemPrice;
    

    public ItemType itemType;
    public float effectValue;
}

public enum ItemType
{
    IncreaseAttack,
    IncreaseHealth,
    IncreaseDefense,
    IncreaseAtkSpeed,
    HitAndRecovery,
}
