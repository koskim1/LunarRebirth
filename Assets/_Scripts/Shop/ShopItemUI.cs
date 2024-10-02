using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopItemUI : MonoBehaviour
{
    public Image itemImage;
    public string itemName;
    public string itemDescription;
    public string itemPrice;
    public Button itemBuyButton;

    public ShopItem shopItem;

    public void SetItem(ShopItem item)
    {
        itemImage = item.itemIcon;
        itemName = item.itemName;
        itemDescription = item.itemDescription;
        itemPrice = $"{item.itemPrice}MLP";

        itemBuyButton.onClick.AddListener(BuyItem);
    }

    public void BuyItem()
    {
        int playerMLP = PlayerAttributesManager.Instance.currentMLP;

        if(playerMLP >= shopItem.itemPrice)
        {
            // 가격만큼 MLP차감
            playerMLP -= shopItem.itemPrice;
            
            // MLPUI업데이트 시켜주고 (from, to)니깐 여기에선 from인 currentMLP를 playerMLP로 적용
            PlayerAttributesManager.Instance.UpdateMLPUI(PlayerAttributesManager.Instance.currentMLP, playerMLP);
            PlayerAttributesManager.Instance.currentMLP = playerMLP;
            // 효과 적용되게

            // 구매 후
            itemBuyButton.interactable = false;
            itemBuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "구매 완료";

            Debug.Log($"{shopItem.itemName}을 구매했습니다.");
        }
        else
        {
            Debug.Log("구매를 위한 MLP가 부족합니다");
        }
    }
}
