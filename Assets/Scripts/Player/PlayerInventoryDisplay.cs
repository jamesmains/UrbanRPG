using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventoryDisplay : MonoBehaviour {
    [SerializeField] [FoldoutGroup("Dependencies")]
    private GameObject InventoryListObject;

    [SerializeField] [FoldoutGroup("Dependencies")]
    private TextMeshProUGUI InfoText; // Displays Unique / Total Item #
    
    [SerializeField] [FoldoutGroup("Settings")]
    private RectTransform Content;

    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private List<ItemDisplay> CachedDisplays = new();

    private void OnEnable() {
        Inventory.OnInventoryChange.AddListener(UpdateInventoryDisplay);
    }

    private void OnDisable() {
        Inventory.OnInventoryChange.RemoveListener(UpdateInventoryDisplay);
    }

    private void UpdateInventoryDisplay() {
        int uniqueItems = 0;
        int totalItems = 0;
        foreach (Transform child in Content) {
            child.gameObject.SetActive(false);
        }

        foreach (var item in Inventory.Singleton.InventoryItems) {
            var obj = Pooler.Spawn(InventoryListObject, Content);
            obj.GetComponent<ItemDisplay>().SetItem(item);
            totalItems += item.Quantity;
            uniqueItems++;
        }
        InfoText.text = $"Unique Items: {uniqueItems}, Total Items: {totalItems}";
    }
}