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
    [FoldoutGroup("Details")][field: SerializeField] public string Description { get; private set; }
    [FoldoutGroup("Data")][field: SerializeField] public ItemType ItemType { get; protected set; }
    [FoldoutGroup("Data")][field: SerializeField] public int SellValue { get; private set; }
    [FoldoutGroup("Data")][field: SerializeField] public int BuyValue { get; private set; }
    [FoldoutGroup("Data")][field: SerializeField] public bool IsConsumable { get; private set; }
    [field: SerializeField,PropertyOrder(80),Space(10)] 
    public List<ItemEffect> ItemEffects { get; set; } = new();

#if UNITY_EDITOR
    private void OnEnable()
    {
        if (Name.IsNullOrWhitespace() && UrbanDebugger.DebugLevel >= 1)
        {
            Debug.LogError($"{this} item has no name! (Item.cs)");
        }
    }
#endif

    public virtual int Value
    {
        get => this.amount;
        set => this.amount = value;
    }

    [SerializeField] protected int amount;
    [field: SerializeField] public int StackLimit { get; private set; } = 999;
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

[Serializable]
public abstract class ItemEffect
{
    public string effectText;
    public abstract void OnConsume();
    public UnityEvent OnConsumeEvent = new();
}

public class RestoreNeedEffect: ItemEffect
{
    public Need TargetNeed;
    public float AdjustByValue;
    public override void OnConsume()
    {
        TargetNeed.Value += AdjustByValue;
        OnConsumeEvent.Invoke();
    }
}

public class ItemStatusEffect : ItemEffect
{
    public float tickRate;
    public float duration;
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
    public Need TargetNeed;
    public float RestoreAmount;

    protected override void OnTickEffect()
    {
        base.OnTickEffect();
        TargetNeed.Value += RestoreAmount;
    }
}