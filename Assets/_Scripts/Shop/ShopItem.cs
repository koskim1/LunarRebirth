using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ShopItem")]
public class ShopItem : ScriptableObject
{
    public Sprite itemIcon;
    public string itemName;
    public string itemDescription;
    public int itemPrice;
    

    public ItemType itemType;
    public int effectValue;
}

public enum ItemType
{
    IncreaseAttack,
    IncreaseHealth,
}
