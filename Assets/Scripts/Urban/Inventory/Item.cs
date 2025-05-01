using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Urban.Inventory {
    /// <summary>
    /// Items Todo:
    /// 1.) Make more generic
    /// |-> a.) Move details about item to class designed to be customized based on project needs
    /// |-> b.) Move or remove Item Effects
    /// </summary>

    [CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
    public class Item : ScriptableObject {
        [SerializeField, PreviewField]
        [FoldoutGroup("Settings")]
        public Sprite Sprite { get; private set; }

        [SerializeField]
        [FoldoutGroup("Settings")]
        public string Name { get; protected set; }

        [SerializeField, TextArea]
        [FoldoutGroup("Settings")]
        public string Description { get; private set; }

        [SerializeField]
        [FoldoutGroup("Settings")]
        public float Value { get; private set; }

        [SerializeField]
        [FoldoutGroup("Settings")]
        public bool IsConsumable { get; private set; }

        [SerializeField]
        [FoldoutGroup("Settings")]
        public float Price { get; private set; }

        [SerializeField]
        [FoldoutGroup("Settings")]
        public List<ItemEffect> ItemEffects { get; set; } = new();
    
        [SerializeField]
        [FoldoutGroup("Events")] [ReadOnly]
        public UnityEvent OnAddItem = new();
    
        [SerializeField]
        [FoldoutGroup("Events")] [ReadOnly]
        public UnityEvent OnRemoveItem = new();
    }

    [Serializable]
    public abstract class ItemEffect {
        public string effectText;
        public abstract void OnConsume();
        [PropertyOrder(100)] public UnityEvent OnConsumeEvent = new();
    }
//
// public class RestoreNeedEffect : ItemEffect {
//     [SerializeField] private Need TargetNeed;
//     [SerializeField] private float AdjustByValue;
//
//     public override void OnConsume() {
//         TargetNeed.Value += AdjustByValue;
//         OnConsumeEvent.Invoke();
//     }
// }

    public class AddItemEffect : ItemEffect {
        [SerializeField] private int amount;
        [SerializeField] private Item item;

        public override void OnConsume() {
            //Inventory.AddItem(item, amount);
        }
    }

    public class RemoveItemEffect : ItemEffect {
        [SerializeField] private int amount;
        [SerializeField] private Item item;

        public override void OnConsume() {
            //Inventory.RemoveItem(item, amount);
        }
    }

    [Serializable]
    public class ItemStatusEffect : ItemEffect {
        [SerializeField] private float tickRate;
        [SerializeField] private float duration;
        [FoldoutGroup("Debug"), ReadOnly] protected float tickTimer;
        [FoldoutGroup("Debug"), ReadOnly] protected float durationTimer;
        [FoldoutGroup("Debug"), ReadOnly] protected bool isActive;

        public override void OnConsume() {
            durationTimer = duration;
            isActive = true;
        }

        // True = effect is active, False = effect is finished
        public virtual bool OnTick() {
            if (!isActive) OnConsume();
            if (tickTimer > 0)
                tickTimer -= Time.deltaTime;
            if (tickTimer <= 0)
                OnTickEffect();

            if (durationTimer > 0)
                durationTimer -= Time.deltaTime;
            if (durationTimer <= 0) {
                OnRemoveEffect();
                return false;
            }

            return true;
        }

        protected virtual void OnTickEffect() {
            tickTimer = tickRate;
        }

        public virtual void OnRemoveEffect() {
            isActive = false;
        }
    }
//
// public class RestoreNeedOverTimeEffect : ItemStatusEffect {
//     [SerializeField] private Need TargetNeed;
//     [SerializeField] private float RestoreAmount;
//
//     protected override void OnTickEffect() {
//         base.OnTickEffect();
//         TargetNeed.Value += RestoreAmount;
//     }
// }
}