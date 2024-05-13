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
    [SerializeField, FoldoutGroup("Display")] private TextMeshProUGUI cartCostText;
    [SerializeField] private int cartCostVariable;
    [SerializeField] private int playerWalletVariable; // Change to read out from player currency value
    [SerializeField] private List<ShopItemDisplay> shopItemDisplays = new();
    [SerializeField] private RectTransform shopItemDisplayObjectContainer;
    [SerializeField, ReadOnly] private Shop currentShop;

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
        currentShop = incomingShop;
        Global.PlayerLock++;
        cartCostVariable = 0;
        cartCostText.text = cartCostVariable.ToString();
        Show();
        CloseOtherWindows(this);
        //WindowUtility.OnOpenWindow.Raise("Pockets"); // Todo - Fix with new menu system calls
        PopulateShopDisplay();
    }

    public void UpdateCartCost()
    {
        cartCostVariable = 0;
        foreach (var shopItemDisplay in shopItemDisplays)
        {
            if(!shopItemDisplay.isActiveAndEnabled) continue;
            cartCostVariable += shopItemDisplay.GetCartCost();
        }
        
        foreach (var shopItemDisplay in shopItemDisplays)
        {
            if(!shopItemDisplay.isActiveAndEnabled) continue;
            shopItemDisplay.UpdateItemButtons();
        }
        cartCostText.text = cartCostVariable.ToString();
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
            shopItemDisplays[index].Setup(i);
        }
    }

    public void ProcessCheckout()
    {
        playerWalletVariable -= cartCostVariable;
        GameEvents.OnUpdateMoneyDisplay.Raise();
        foreach (var shopItemDisplay in shopItemDisplays)
        {
            if (shopItemDisplay.inCartAmount == 0) continue;
            int r = currentShop.targetInventory.TryAddItem(shopItemDisplay.shopItem.item, shopItemDisplay.inCartAmount);
            shopItemDisplay.itemQuantity -= shopItemDisplay.inCartAmount - r;
            shopItemDisplay.shopItem.currentQuantity -= shopItemDisplay.inCartAmount - r;
            playerWalletVariable += (int)shopItemDisplay.shopItem.item.Value.x * r;
            shopItemDisplay.inCartAmount = 0;
            if (r > 0)
            {
                GameEvents.OnSendGenericMessage.Raise("Pockets are full!");
            }
            shopItemDisplay.AdjustCartAmount(0);
        }
        GameEvents.OnUpdateInventory.Raise();
    }

    public void CloseShop()
    {
        foreach (var shopItemDisplay in shopItemDisplays)
        {
            if(!shopItemDisplay.isActiveAndEnabled) continue;
            shopItemDisplay.AdjustCartAmount(-999);
        }
        Global.PlayerLock--;
        //WindowUtility.OnCloseWindow.Raise("Pockets"); // Todo - Fix with new menu system calls
        Hide();
        GameEvents.ShowPlayerHud.Raise();
    }
}
