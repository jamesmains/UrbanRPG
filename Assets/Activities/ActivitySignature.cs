using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Activity Signature", menuName = "Scriptable Objects/Activity Signature")]
public class ActivitySignature : SerializedScriptableObject
{
    public ActionType actionType;
    public string ActionName;
    public Sprite ActionIcon;
    // public RequiredItem[] RequiredItems;
    // public RequiredQuest[] RequiredQuests;
    // public Inventory[] TargetInventories;

    [field: SerializeField] private List<ActivityCondition> Conditions { get; set; } = new List<ActivityCondition>();
    
    public bool IsConditionMet()
    {
        return Conditions.TrueForAll(c => c.IsConditionMet());
    }

    public void TryUseItems()
    {
        Conditions.ForEach(c => c.Use());
    }
}

[Serializable]
public abstract class ActivityCondition
{
    public abstract bool IsConditionMet();
    public abstract void Use();
}

public class ActivityAndCondition : ActivityCondition
{
    [field: SerializeField]
    public List<ActivityCondition> Conditions { get; private set; } = new List<ActivityCondition>();
    
    public override bool IsConditionMet()
    {
        return Conditions.TrueForAll(c => c.IsConditionMet());
    }

    public override void Use()
    {
        Conditions.ForEach(c => c.Use());
    }
}

public class ActivityOrCondition : ActivityCondition
{
    [field: SerializeField]
    public List<ActivityCondition> Conditions { get; private set; } = new List<ActivityCondition>();

    public override bool IsConditionMet()
    {
        return Conditions.Exists(c => c.IsConditionMet());
    }

    public override void Use()
    {
        Conditions.ForEach(c => c.Use());
    }
}

public class ActivityNotCondition : ActivityCondition
{
    [field: SerializeField]
    public List<ActivityCondition> Conditions { get; private set; } = new List<ActivityCondition>();

    public override bool IsConditionMet()
    {
        return !Conditions.Exists(c => c.IsConditionMet());
    }

    public override void Use()
    {
        Conditions.ForEach(c => c.Use());
    }
}

public class ActivityQuestStateCondition : ActivityCondition
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

public class ActivityItemCondition : ActivityCondition
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

public class ActivityQuestItemCondition : ActivityItemCondition
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
