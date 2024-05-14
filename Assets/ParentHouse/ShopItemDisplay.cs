using ParentHouse.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ParentHouse {
    public class ShopItemDisplay : MonoBehaviour {
        [SerializeField] [FoldoutGroup("Display")]
        private TextMeshProUGUI quantityText;

        [SerializeField] [FoldoutGroup("Display")]
        private TextMeshProUGUI nameText;

        [SerializeField] [FoldoutGroup("Display")]
        private TextMeshProUGUI descriptionText;

        [SerializeField] [FoldoutGroup("Display")]
        private TextMeshProUGUI costText;

        [SerializeField] [FoldoutGroup("Display")]
        private TextMeshProUGUI cartCalculationText;

        [SerializeField] [FoldoutGroup("Display")]
        private Image iconDisplay;

        [SerializeField] [FoldoutGroup("Display")]
        private GameObject outOfStockIndicator;

        [SerializeField] [FoldoutGroup("Display")]
        private Button decreaseCartAmountButton;

        [SerializeField] [FoldoutGroup("Display")]
        private Button increaseCartAmountButton;

        [SerializeField] private int playerWalletVariable; // todo - replace with static reference
        [SerializeField] private int cartCostVariable;
        [FoldoutGroup("Data")] public int inCartAmount;

        [SerializeField] [FoldoutGroup("Data")]
        private int lastValidInCartAmount;

        [FoldoutGroup("Data")] public int itemQuantity;
        [FoldoutGroup("Data")] public ShopItem shopItem;

        private void Awake() {
            //quantityInput.onValueChanged.AddListener(delegate { SetQuantity(quantityInput); }); 
        }

        public void Setup(ShopItem incomingItem) {
            shopItem = incomingItem;
            quantityText.text = shopItem.currentQuantity.ToString();
            nameText.text = shopItem.item.Name;
            descriptionText.text = shopItem.item.Description;
            iconDisplay.sprite = shopItem.item.Sprite;
            costText.text = shopItem.item.Value.x.ToString();
            itemQuantity = incomingItem.currentQuantity;
            outOfStockIndicator.SetActive(itemQuantity <= 0);
            AdjustCartAmount(0);
        }

        public void SetQuantity(TMP_InputField amount) {
            inCartAmount = int.Parse(amount.text);
            AdjustCartAmount(0);
        }

        public void AdjustCartAmount(int changeValue) {
            inCartAmount += changeValue;


            if (inCartAmount * shopItem.item.Value.x > playerWalletVariable) inCartAmount = lastValidInCartAmount;

            GameEvents.OnCartQuantityChange.Invoke();

            inCartAmount = inCartAmount <= 0 ? 0 : inCartAmount > itemQuantity ? itemQuantity : inCartAmount;
            lastValidInCartAmount = inCartAmount;
            UpdateItemButtons();

            if (inCartAmount == 0)
                cartCalculationText.text = "None in cart";
            else
                cartCalculationText.text =
                    $"{inCartAmount}\nx{shopItem.item.Value.x.ToString("n0").TrimStart('0')} =\n{inCartAmount * shopItem.item.Value.x:0,000}";

            quantityText.text = shopItem.currentQuantity.ToString();
            outOfStockIndicator.SetActive(itemQuantity <= 0);
        }

        public int GetCartCost() {
            return inCartAmount * (int) shopItem.item.Value.x;
        }

        public void UpdateItemButtons() {
            var availableFunds = playerWalletVariable - cartCostVariable;
            decreaseCartAmountButton.interactable = inCartAmount > 0;
            increaseCartAmountButton.interactable =
                inCartAmount < itemQuantity && shopItem.item.Value.x < availableFunds;
        }
    }
}