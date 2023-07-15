using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ShopDisplay : MonoBehaviour
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

    private void Awake()
    {
        OpenShop(debugShop);
    }

    public void OpenShop(Shop incomingShop)
    {
        currentShop = incomingShop;
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
        foreach (var oldObj in shopItemDisplays)
        {
            Destroy(oldObj.gameObject);
        }
        shopItemDisplays.Clear();
        foreach (var i in currentShop.storeItems)
        {
            var obj = Instantiate(shopItemDisplayObject,shopItemDisplayObjectContainer);
            ShopItemDisplay shopItemDisplay = obj.GetComponent<ShopItemDisplay>();
            shopItemDisplay.Setup(i, 999);
            shopItemDisplays.Add(shopItemDisplay);
        }
    }

    public void ProcessCheckout()
    {
        playerWalletVariable.Value -= cartCostVariable.Value;
        GameEvents.OnUpdateMoneyDisplay.Raise();
        foreach (var shopItemDisplay in shopItemDisplays)
        {
            currentShop.targetInventory.TryAddItem(shopItemDisplay.item, shopItemDisplay.inCartAmount);
            shopItemDisplay.itemQuantity -= shopItemDisplay.inCartAmount;
            shopItemDisplay.inCartAmount = 0;
            shopItemDisplay.AdjustCartAmount(0);
        }
        GameEvents.OnUpdateInventory.Raise();
    }

    public void CancelCheckout()
    {
        foreach (var shopItemDisplay in shopItemDisplays)
        {
            shopItemDisplay.AdjustCartAmount(-999);
        }
    }
}
