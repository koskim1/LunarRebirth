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
            // ���ݸ�ŭ MLP����
            playerMLP -= shopItem.itemPrice;
            
            // MLPUI������Ʈ �����ְ� (from, to)�ϱ� ���⿡�� from�� currentMLP�� playerMLP�� ����
            PlayerAttributesManager.Instance.UpdateMLPUI(PlayerAttributesManager.Instance.currentMLP, playerMLP);
            PlayerAttributesManager.Instance.currentMLP = playerMLP;
            // ȿ�� ����ǰ�

            // ���� ��
            itemBuyButton.interactable = false;
            itemBuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "���� �Ϸ�";

            Debug.Log($"{shopItem.itemName}�� �����߽��ϴ�.");
        }
        else
        {
            Debug.Log("���Ÿ� ���� MLP�� �����մϴ�");
        }
    }
}
