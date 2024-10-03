using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    public GameObject shopUI;
    public Transform itemContainer;
    public GameObject shopItemPrefab;
    public TextMeshProUGUI playerMLPText;

    // 판매할 아이템들 ScriptableObject
    public List<ShopItem> shopItems = new List<ShopItem>();

    private PlayerController playerController;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        shopUI.SetActive(false);

        playerController = FindAnyObjectByType<PlayerController>();
    }

    private void Start()
    {
        PopulateShop();
    }

    private void PopulateShop()
    {
        foreach(var item in shopItems)
        {
            Debug.Log($"아이템 생성 : {item.itemName}");
            GameObject itemGO = Instantiate(shopItemPrefab, itemContainer);
            ShopItemUI itemUI = itemGO.GetComponent<ShopItemUI>();

            itemUI.SetItem(item);
        }
    }

    public void OpenShop()
    {
        shopUI.SetActive(true);

        Time.timeScale = 0f;
        playerController.canMove = false;

        Cursor.visible = true;
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        Time.timeScale = 1f;
        playerController.canMove = true;
        Cursor.visible = false;
    }

    public void UpdateShopPlayerMLP()
    {
        playerMLPText.text = $"MLP : {PlayerAttributesManager.Instance.currentMLP}";
    }
}
