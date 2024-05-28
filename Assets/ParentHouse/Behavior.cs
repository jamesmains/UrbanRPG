using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace ParentHouse {
    [Serializable]
    public abstract class Behavior {
        public UnityEvent OnConsumeEvent = new();
        public abstract void OnConsume();
        
        public void OnAddBehavior() {
        }

        public void OnRemoveBehavior() {
            
        }

        public void OnUpdateBehavior() {
            
        }
    }
    public class RestoreNeedEffect : Behavior {
        [SerializeField] private float AdjustByValue;
        [SerializeField] private Need TargetNeed;

        public override void OnConsume() {
            TargetNeed.Value += AdjustByValue;
            OnConsumeEvent.Invoke();
        }
    }

    public class AddItemEffect : Behavior {
        [SerializeField] [FoldoutGroup("Settings")] private int amount;
        [SerializeField] [FoldoutGroup("Settings")] private Inventory inventory;
        [SerializeField] [FoldoutGroup("Settings")] private Item item;

        public override void OnConsume() {
            inventory.AddItem(item, amount);
        }
    }

    public class RemoveItemEffect : Behavior {
        [SerializeField] private int amount;
        [SerializeField] private Inventory inventory;
        [SerializeField] private Item item;

        public override void OnConsume() {
            inventory.RemoveItem(item, amount);
        }
    }
    
    public class ItemStatusEffect : Behavior {
        [SerializeField] private float duration;
        [FoldoutGroup("Debug")] [ReadOnly] protected float durationTimer;
        [FoldoutGroup("Debug")] [ReadOnly] protected bool isActive;
        [SerializeField] private float tickRate;
        [FoldoutGroup("Debug")] [ReadOnly] protected float tickTimer;

        public override void OnConsume() {
            durationTimer = duration;
            isActive = true;
        }

        // Todo - convert to using TimeManager
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
                return true;
            }

            return false;
        }

        protected virtual void OnTickEffect() {
            tickTimer = tickRate;
        }

        public virtual void OnRemoveEffect() {
            isActive = false;
        }
    }

    public class RestoreNeedOverTimeEffect : ItemStatusEffect {
        [SerializeField] private float RestoreAmount;
        [SerializeField] private Need TargetNeed;

        protected override void OnTickEffect() {
            base.OnTickEffect();
            TargetNeed.Value += RestoreAmount;
        }
    }
}