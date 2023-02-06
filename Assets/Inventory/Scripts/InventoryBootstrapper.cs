using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace I302.Manu
{
    public class InventoryBootstrapper : MonoBehaviour
    {
        [SerializeField] private ItemLookupTable itemLookupTable;
        [SerializeField] private Inventory bag;
        private void Awake()
        {
            bag.LoadInventory(itemLookupTable);
            bag.LoadAllItems();
        }
    }
}
