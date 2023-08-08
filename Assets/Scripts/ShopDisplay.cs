using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ShopDisplay : Window
{
    [SerializeField, FoldoutGroup("Display")] private GameObject shopItemDisplayObject;
    [SerializeField, FoldoutGroup("Display")] private TextMeshProUGUI showNameText;
    [SerializeField, FoldoutGroup("Display")] private TextMeshProUGUI cartCostText;
    [SerializeField] private IntVariable cartCostVariable;
    [SerializeField] private IntVariable playerWalletVariable;
    [SerializeField] private List<ShopItemDisplay> shopItemDisplays = new();
    [SerializeField] private RectTransform shopItemDisplayObjectContainer;
    [SerializeField] private Shop debugShop;
    private Shop currentShop;

    protected override void OnEnable()
    {
        base.OnEnable();
        for (int i = 0; i < 25; i++)
        {
            shopItemDisplays.Add(Instantiate(shopItemDisplayObject,shopItemDisplayObjectContainer).GetComponent<ShopItemDisplay>());
            shopItemDisplays[i].inCartAmount = 0;
            shopItemDisplays[i].gameObject.SetActive(false);
        }
        GameEvents.OnOpenShop += OpenShop;
        GameEvents.OnCartQuantityChange += UpdateCartCost;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameEvents.OnOpenShop -= OpenShop;
        GameEvents.OnCartQuantityChange -= UpdateCartCost;
    }
    
    public void OpenShop(Shop incomingShop)
    {
        Show();
        currentShop = incomingShop;
        Global.PlayerLock++;
        cartCostVariable.Value = 0;
        cartCostText.text = cartCostVariable.Value.ToString();
        PopulateShopDisplay();
    }

    public void UpdateCartCost()
    {
        cartCostVariable.Value = 0;
        foreach (var shopItemDisplay in shopItemDisplays)
        {
            cartCostVariable.Value += shopItemDisplay.GetCartCost();
        }
        
        foreach (var shopItemDisplay in shopItemDisplays)
        {
            shopItemDisplay.UpdateItemButtons();
        }
        cartCostText.text = cartCostVariable.Value.ToString();
    }
    
    public void PopulateShopDisplay()
    {
        foreach (var shopItemDisplay in shopItemDisplays)
        {
            shopItemDisplay.inCartAmount = 0;
            shopItemDisplay.gameObject.SetActive(false);
        }
        for (var index = 0; index < currentShop.storeItems.Count; index++)
        {
            shopItemDisplays[index].gameObject.SetActive(true);
            
            var i = currentShop.storeItems[index];
            shopItemDisplays[index].Setup(i, 999);
        }
    }

    public void ProcessCheckout()
    {
        playerWalletVariable.Value -= cartCostVariable.Value;
        GameEvents.OnUpdateMoneyDisplay.Raise();
        foreach (var shopItemDisplay in shopItemDisplays)
        {
            if (shopItemDisplay.inCartAmount == 0) continue;
            currentShop.targetInventory.TryAddItem(shopItemDisplay.item, shopItemDisplay.inCartAmount);
            shopItemDisplay.itemQuantity -= shopItemDisplay.inCartAmount;
            shopItemDisplay.inCartAmount = 0;
            shopItemDisplay.AdjustCartAmount(0);
        }
        GameEvents.OnUpdateInventory.Raise();
    }

    public void CloseShop()
    {
        foreach (var shopItemDisplay in shopItemDisplays)
        {
            shopItemDisplay.AdjustCartAmount(-999);
        }
        Global.PlayerLock--;
        Hide();
    }
}
