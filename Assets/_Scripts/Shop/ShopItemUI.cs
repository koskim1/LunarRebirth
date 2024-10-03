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
        itemImage.sprite = item.itemIcon;
        itemName.text = item.itemName;
        itemDescription.text = item.itemDescription;
        itemPrice.text = $"{item.itemPrice}MLP";

        itemBuyButton.onClick.RemoveAllListeners();
        itemBuyButton.onClick.AddListener(BuyItem);

        // �̹� ������ �������̶�� ���Ź�ư ��Ȱ��ȭ
        if (PlayerAttributesManager.Instance.IsItemPurchased(item))
        {
            itemBuyButton.interactable = false;
            itemBuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "���� �Ϸ�";
        }
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
            ApplyItemEffect();
            // ���� ��
            itemBuyButton.interactable = false;
            itemBuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "���� �Ϸ�";

            Debug.Log($"{shopItem.itemName}�� �����߽��ϴ�.");
        }
        else
        {
            Debug.Log("���Ÿ� ���� MLP�� �����մϴ�");
        }
        PlayerAttributesManager.Instance.UpdateMLPUI(PlayerAttributesManager.Instance.currentMLP, playerMLP);
    }

    private void ApplyItemEffect()
    {
        switch (shopItem.itemType)
        {
            case ItemType.IncreaseHealth:
                PlayerAttributesManager.Instance.IncreaseStat("health", shopItem.effectValue);
                break;
            case ItemType.IncreaseAttack:
                PlayerAttributesManager.Instance.IncreaseStat("attack", shopItem.effectValue);
                break;
        }
    }
}
