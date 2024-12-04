using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopItemUI : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemPrice;
    public Button itemBuyButton;

    public ShopItem shopItem;

    public void SetItem(ShopItem item)
    {
        /*
            shopItem 변수가 올바르게 설정되지 않으면, 구매 버튼을 클릭했을 때 잘못되거나 기본상태?
        shopItem을 참조하게 돼서 잊지말고 참조하게 해주기.
        */
        shopItem = item;        

        itemImage.sprite = item.itemIcon;
        itemName.text = item.itemName;
        itemDescription.text = item.itemDescription;
        itemPrice.text = $"{item.itemPrice}MLP";

        itemBuyButton.onClick.RemoveAllListeners();
        itemBuyButton.onClick.AddListener(BuyItem);

        // 이미 구매한 아이템이라면 구매버튼 비활성화
        if (PlayerAttributesManager.Instance.IsItemPurchased(item))
        {
            itemBuyButton.interactable = false;
            itemBuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "구매 완료";
        }
    }

    public void BuyItem()
    {
        int playerMLP = PlayerAttributesManager.Instance.currentMLP;

        if(playerMLP >= shopItem.itemPrice)
        {
            // 가격만큼 MLP차감
            playerMLP -= shopItem.itemPrice;

            // 효과 적용되게
            ApplyItemEffect();
            // 구매 후
            itemBuyButton.interactable = false;
            itemBuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "구매 완료";

            // MLPUI업데이트 시켜주고 (from, to)니깐 여기에선 from인 currentMLP를 playerMLP로 적용
            PlayerAttributesManager.Instance.UpdateMLPUI(PlayerAttributesManager.Instance.currentMLP, playerMLP);
            PlayerAttributesManager.Instance.currentMLP = playerMLP;

            Debug.Log($"{shopItem.itemName}을 구매했습니다.");
        }
        else
        {
            Debug.Log("구매를 위한 MLP가 부족합니다");
        }
    }

    private void ApplyItemEffect()
    {
        switch (shopItem.itemType)
        {
            case ItemType.IncreaseHealth:
                PlayerAttributesManager.Instance.IncreaseStat("health", (int)shopItem.effectValue);
                break;
            case ItemType.IncreaseAttack:
                PlayerAttributesManager.Instance.IncreaseStat("attack", (int)shopItem.effectValue);
                break;
            case ItemType.IncreaseAtkSpeed:
                PlayerAttributesManager.Instance.IncreaseAttackSpeed(shopItem.effectValue);
                break;

        }
    }
}
