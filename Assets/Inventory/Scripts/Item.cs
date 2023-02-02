using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace I302.Manu
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/ItemsAndInventory/Item")]
    public class Item : ScriptableObject
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public ItemType ItemType { get; private set; }

#if UNITY_EDITOR
        private void OnEnable()
        {
            if (Name.IsNullOrWhitespace())
            {
                Debug.LogError($"{this} item has no name!");
            }
        }
#endif

        public virtual int Value
        {
            get => this.amount;
            set
            {
                this.amount = value;
                SaveItem();
            } 
        }

        [SerializeField] protected int amount;
        [field: SerializeField] public int StackLimit { get; private set; } = Int32.MaxValue;

        [Button]
        public void SaveItem()
        {
            SaveLoad.SaveItem(new ItemSaveData(this));
        }

        [Button]
        public void LoadItem()
        {
            ItemSaveData saveData = SaveLoad.LoadItem(Name);
            Value = saveData.Value;
        }
    }

    [Serializable]
    public class ItemSaveData
    {
        public string Name { get; private set; }
        public int Value { get; private set; }
        
        public ItemSaveData(Item item)
        {
            Name = item.Name;
            Value = item.Value;
        }
    }
}
