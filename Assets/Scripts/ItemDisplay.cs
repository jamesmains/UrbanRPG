using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : Hoverable
{
    [SerializeField] [FoldoutGroup("Dependencies")]
    private TextMeshProUGUI ItemQuantityText;

    [SerializeField] [FoldoutGroup("Dependencies")]
    private Image ItemIcon;

    public void SetItem(InventoryItemData itemToDisplay){
        ItemQuantityText.text = itemToDisplay.Quantity.ToString();
        ItemIcon.sprite = itemToDisplay.Item.Sprite;
        var textBehavior = (ShowTextOnHover) UiHoverBehaviours.FirstOrDefault(o => o.GetType().ToString() == "ShowTextOnHover");
        if (textBehavior == null) return;
        textBehavior.TextToDisplay = itemToDisplay.Item.Name;
    }
}
