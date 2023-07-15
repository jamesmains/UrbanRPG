using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDisplay : MonoBehaviour
{
    [SerializeField, FoldoutGroup("Display")] private TextMeshProUGUI quantityText;
    [SerializeField, FoldoutGroup("Display")] private TextMeshProUGUI nameText;
    [SerializeField, FoldoutGroup("Display")] private TextMeshProUGUI descriptionText;
    [SerializeField, FoldoutGroup("Display")] private TextMeshProUGUI costText;
    [SerializeField, FoldoutGroup("Display")] private TextMeshProUGUI cartCalculationText;
    [SerializeField, FoldoutGroup("Display")] private Image iconDisplay;
    [SerializeField, FoldoutGroup("Display")] private TMP_InputField quantityInput;

    [SerializeField, FoldoutGroup("Display")] private Button decreaseCartAmountButton;
    [SerializeField, FoldoutGroup("Display")] private Button increaseCartAmountButton;
    
    [SerializeField] private IntVariable playerWalletVariable;
    [SerializeField] private IntVariable cartCostVariable;
    [FoldoutGroup("Data")] public int inCartAmount;
    [SerializeField, FoldoutGroup("Data")] private int lastValidInCartAmount;
    [FoldoutGroup("Data")] public int itemQuantity;
    [FoldoutGroup("Data")] public Item item;

    private void Awake()
    {
        //quantityInput.onValueChanged.AddListener(delegate { SetQuantity(quantityInput); }); 
    }

    public void Setup(Item incomingItem, int quantity)
    {
        item = incomingItem;
        quantityText.text = quantity.ToString();
        nameText.text = item.Name;
        descriptionText.text = item.Description;
        iconDisplay.sprite = item.Sprite;
        costText.text = item.BuyValue.ToString();
        itemQuantity = quantity;
        AdjustCartAmount(0);
    }

    public void SetQuantity(TMP_InputField amount)
    {
        inCartAmount = int.Parse(amount.text);
        AdjustCartAmount(0);
    }
    
    public void AdjustCartAmount(int changeValue)
    {
        inCartAmount += changeValue;
        
        
        if (inCartAmount * item.BuyValue > playerWalletVariable.Value)
        {
            inCartAmount = lastValidInCartAmount;
        }

        GameEvents.OnCartQuantityChange.Raise();
        
        inCartAmount = inCartAmount <= 0 ? 0 : inCartAmount > itemQuantity ? itemQuantity : inCartAmount;
        lastValidInCartAmount = inCartAmount;
        UpdateItemButtons();
        
        if (inCartAmount == 0)
        {
            cartCalculationText.text = "None in cart";
        }
        else
        {
            cartCalculationText.text = $"{inCartAmount}\nx{item.BuyValue.ToString("n0").TrimStart('0')} =\n{(inCartAmount*item.BuyValue):0,000}";
        }
    }
    
    public int GetCartCost()
    {
        return inCartAmount * item.BuyValue;
    }

    public void UpdateItemButtons()
    {
        int availableFunds = playerWalletVariable.Value - cartCostVariable.Value;
        decreaseCartAmountButton.interactable = inCartAmount > 0;
        increaseCartAmountButton.interactable = inCartAmount < itemQuantity && (item.BuyValue < availableFunds);
    }
}
