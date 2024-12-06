using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
public class ShopItemUI : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemPrice;
    public Button itemBuyButton;

    public ShopItem shopItem;

    private LocalizedString currentItemName;
    private LocalizedString currentItemDescription;


    //public void SetItem(ShopItem item)
    //{
    //    /*
    //        shopItem ������ �ùٸ��� �������� ������, ���� ��ư�� Ŭ������ �� �߸��ǰų� �⺻����?
    //    shopItem�� �����ϰ� �ż� �������� �����ϰ� ���ֱ�.
    //    */
    //    shopItem = item;        

    //    itemImage.sprite = item.itemIcon;
    //    itemName.text = item.itemName;
    //    itemDescription.text = item.itemDescription;
    //    itemPrice.text = $"{item.itemPrice}MLP";

    //    itemBuyButton.onClick.RemoveAllListeners();
    //    itemBuyButton.onClick.AddListener(BuyItem);

    //    // �̹� ������ �������̶�� ���Ź�ư ��Ȱ��ȭ
    //    if (PlayerAttributesManager.Instance.IsItemPurchased(item))
    //    {
    //        itemBuyButton.interactable = false;
    //        itemBuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "���� �Ϸ�";
    //    }

    // �ؿ� ���ö���¡ �������� ����,,
    //}

    public void SetItem(ShopItem item)
    {
        if (currentItemName != null)        currentItemName.StringChanged -= OnNameChanged;
        if (currentItemDescription != null) currentItemDescription.StringChanged -= OnDescriptionChanged;

        shopItem = item;

        itemImage.sprite = item.itemIcon;
        itemPrice.text = $"{item.itemPrice}MLP";

        // ���� �������� LocalizedString ���۷����� ����
        currentItemName = item.itemName;
        currentItemDescription = item.itemDescription;

        // ���ڿ� ���� �̺�Ʈ ���
        currentItemName.StringChanged += OnNameChanged;
        currentItemDescription.StringChanged += OnDescriptionChanged;

        // Locale�� �̹� �����Ǿ��ִٸ� ��� �ݿ��ǵ��� �ʱⰪ ����
        OnNameChanged(currentItemName.GetLocalizedString());
        OnDescriptionChanged(currentItemDescription.GetLocalizedString());

        itemBuyButton.onClick.RemoveAllListeners();
        itemBuyButton.onClick.AddListener(BuyItem);

        // �̹� ������ �������̶�� ���Ź�ư ��Ȱ��ȭ
        if (PlayerAttributesManager.Instance.IsItemPurchased(item))
        {
            itemBuyButton.interactable = false;
            itemBuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Purchased";
        }
    }
    private void OnNameChanged(string translatedName)
    {
        itemName.text = translatedName;
    }

    private void OnDescriptionChanged(string translatedDesc)
    {
        itemDescription.text = translatedDesc;
    }


    public void BuyItem()
    {
        int playerMLP = PlayerAttributesManager.Instance.currentMLP;

        if(playerMLP >= shopItem.itemPrice)
        {
            // ���ݸ�ŭ MLP����
            playerMLP -= shopItem.itemPrice;

            // ȿ�� ����ǰ�
            ApplyItemEffect();
            // ���� ��
            itemBuyButton.interactable = false;
            itemBuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Purchased";

            // MLPUI������Ʈ �����ְ� (from, to)�ϱ� ���⿡�� from�� currentMLP�� playerMLP�� ����
            PlayerAttributesManager.Instance.UpdateMLPUI(PlayerAttributesManager.Instance.currentMLP, playerMLP);
            PlayerAttributesManager.Instance.currentMLP = playerMLP;

            Debug.Log($"{shopItem.itemName}�� �����߽��ϴ�.");
        }
        else
        {
            Debug.Log("���Ÿ� ���� MLP�� �����մϴ�");
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
            case ItemType.HitAndRecovery:
                PlayerAttributesManager.Instance.hasLifeSteal = true;
                PlayerAttributesManager.Instance.lifeStealAmount += 5;
                break;

        }
    }
}
