using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Item", menuName = "Items and Inventory/Item")]
public class Item : SerializedScriptableObject
{
    [FoldoutGroup("Details")][field: SerializeField,PreviewField] public Sprite Sprite { get; private set; }
    [FoldoutGroup("Details")][field: SerializeField] public string Name { get; protected set; }
    [FoldoutGroup("Details")][field: SerializeField,TextArea] public string Description { get; private set; }
    [FoldoutGroup("Data")][field: SerializeField] public ItemType ItemType { get; protected set; }
    [FoldoutGroup("Data"),Tooltip("Buy,Sell")][field: SerializeField] public Vector2 Value { get; private set; }
    [FoldoutGroup("Data")][field: SerializeField] public bool IsConsumable { get; private set; }
    [FoldoutGroup("Data")][field: SerializeField] public bool IsIdentified { get; private set; }
    
    [field: SerializeField,PropertyOrder(70),Space(10)] 
    public UnityEvent OnPickupItem = new();
    
    [field: SerializeField,PropertyOrder(80),Space(10)] 
    public List<ItemEffect> ItemEffects { get; set; } = new();
    
    [field: SerializeField] public int StackLimit { get; private set; } = 999;
}

[Serializable]
public abstract class ItemEffect
{
    public string effectText;
    public abstract void OnConsume();
    [PropertyOrder(100)]public UnityEvent OnConsumeEvent = new();
}

public class RestoreNeedEffect: ItemEffect
{
    [SerializeField] private Need TargetNeed;
    [SerializeField] private float AdjustByValue;
    public override void OnConsume()
    {
        TargetNeed.Value += AdjustByValue;
        OnConsumeEvent.Invoke();
    }
}

public class AddItemEffect : ItemEffect
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private int amount;
    [SerializeField] private Item item;
    public override void OnConsume()
    {
        inventory.TryAddItem(item, amount);
    }
}

public class RemoveItemEffect : ItemEffect
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private int amount;
    [SerializeField] private Item item;
    public override void OnConsume()
    {
        inventory.TryRemoveItem(item, amount);
    }
}

public class ItemStatusEffect : ItemEffect
{
    [SerializeField] private float tickRate;
    [SerializeField] private float duration;
    [FoldoutGroup("Debug"),ReadOnly] protected float tickTimer;
    [FoldoutGroup("Debug"),ReadOnly] protected float durationTimer;
    [FoldoutGroup("Debug"),ReadOnly] protected bool isActive;
    
    public override void OnConsume()
    {
        durationTimer = duration;
        isActive = true;
    }

    public virtual bool OnTick()
    {
        if(!isActive) OnConsume();
        if(tickTimer > 0)
            tickTimer -= Time.deltaTime;
        if(tickTimer <= 0)
            OnTickEffect();
        
        if(durationTimer > 0)
            durationTimer -= Time.deltaTime;
        if (durationTimer <= 0)
        {
            OnRemoveEffect();
            return true;
        }

        return false;
    }

    protected virtual void OnTickEffect()
    {
        tickTimer = tickRate;
    }

    public virtual void OnRemoveEffect()
    {
        isActive = false;
    }
}

public class RestoreNeedOverTimeEffect : ItemStatusEffect
{
    [SerializeField] private Need TargetNeed;
    [SerializeField] private float RestoreAmount;

    protected override void OnTickEffect()
    {
        base.OnTickEffect();
        TargetNeed.Value += RestoreAmount;
    }
}