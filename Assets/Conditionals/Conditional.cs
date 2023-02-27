using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using Sirenix.OdinInspector;
using UnityEngine;

// [CreateAssetMenu(fileName = "Condition",menuName = "Condition")]
// public class Conditional : SerializedScriptableObject
// {
//     [field: SerializeField] private List<Condition> Conditions { get; set; } = new ();
//     public bool IsConditionMet()
//     {
//         return Conditions.TrueForAll(c => c.IsConditionMet());
//     }
// }

[Serializable]
public abstract class Condition
{
    public abstract bool IsConditionMet();
    public abstract void Use();
}

public class AndCondition : Condition
{
    [field: SerializeField]
    public List<Condition> Conditions { get; private set; } = new();
    
    public override bool IsConditionMet()
    {
        return Conditions.TrueForAll(c => c.IsConditionMet());
    }
    public override void Use()
    {
        Conditions.ForEach(c => c.Use());
    }
}

public class OrCondition : Condition
{
    [field: SerializeField]
    public List<Condition> Conditions { get; private set; } = new();

    public override bool IsConditionMet()
    {
        return Conditions.Exists(c => c.IsConditionMet());
    }
    
    public override void Use()
    {
        Conditions.ForEach(c => c.Use());
    }
}

public class NotCondition : Condition
{
    [field: SerializeField]
    public List<Condition> Conditions { get; private set; } = new();

    public override bool IsConditionMet()
    {
        return !Conditions.Exists(c => c.IsConditionMet());
    }
    
    public override void Use()
    {
        Conditions.ForEach(c => c.Use());
    }
}

public class QuestStateConditional : Condition
{
    [field: SerializeField]
    public Quest TargetQuest { get; private set; }
    [field: SerializeField]
    public QuestState TargetState { get; private set; }
    public override bool IsConditionMet()
    {
        return TargetQuest.QuestState == TargetState;
    }

    public override void Use()
    {
        
    }
}

public class ItemConditional : Condition
{
    [field: SerializeField]
    public Item TargetItem { get; private set; }
    [field: SerializeField]
    public int ItemAmount { get; private set; }
    [field: SerializeField]
    public bool UseAny { get; private set; }
    [field: SerializeField]
    public Inventory TargetInventory { get; private set; }

    protected int _useAmount = 0;
    
    public override bool IsConditionMet()
    {
        if (!TargetInventory.HasItem(TargetItem))
        {
            return false;
        }
        return UseAny ? TargetInventory.ItemList[TargetItem] > 0 : TargetInventory.ItemList[TargetItem] >= ItemAmount;
    }

    public override void Use()
    {
        int availableAmount = TargetInventory.ItemList[TargetItem] >= ItemAmount
            ? ItemAmount
            : TargetInventory.ItemList[TargetItem];
        _useAmount = availableAmount;
        TargetInventory.TryUseItem(TargetItem,_useAmount);
    }
}

public class QuestItemConditional : ItemConditional
{
    [field: SerializeField]
    public Quest TargetQuest { get; private set; }
    [field: SerializeField]
    public QuestTaskSignature TargetTaskSignature { get; private set; }
    
    public override void Use()
    {
        int availableAmount = TargetInventory.ItemList[TargetItem] >= ItemAmount
            ? ItemAmount
            : TargetInventory.ItemList[TargetItem];
        _useAmount = TargetQuest.TryCompleteTask(TargetTaskSignature, availableAmount);
        TargetInventory.TryUseItem(TargetItem,_useAmount);
    }
}