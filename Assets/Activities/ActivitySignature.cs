using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using UnityEngine;

[CreateAssetMenu(fileName = "Activity Signature", menuName = "Scriptable Objects/Activity Signature")]
public class ActivitySignature : ScriptableObject
{
    public ActionType actionType;
    public string ActionName;
    public Sprite ActionIcon;
    public RequiredItem[] RequiredItems;
    public RequiredQuest[] RequiredQuests;
    public Inventory[] TargetInventories;

    public bool CanDo()
    {
        bool hasItems = false;
        int itemHitCount = 0;
        if (RequiredItems.Length > 0)
        {
            foreach (RequiredItem requiredItem in RequiredItems)
            {
                foreach (Inventory inventory in TargetInventories)
                {
                    if (inventory.HasItem(requiredItem.Item))
                    {
                        if (inventory.ItemList[requiredItem.Item] >= requiredItem.Quantity || (requiredItem.UseAny && inventory.ItemList[requiredItem.Item] > 0))
                        {
                            foreach (RequiredQuest quest in RequiredQuests)
                            {
                                if (quest.Quest.currentTaskSign == requiredItem.TaskSignature)
                                {
                                    itemHitCount++;
                                }
                            }
                            
                        }
                    }
                }
            }

            if (itemHitCount >= RequiredItems.Length)
                hasItems = true;
        }
        else hasItems = true;

        bool hasQuest = false;
        int questHitCount = 0;
        if (RequiredQuests.Length > 0)
        {
            foreach (RequiredQuest requiredQuest in RequiredQuests)
            {
                switch (requiredQuest.RequiredState)
                {
                    case QuestState.NotStarted:
                        if (!requiredQuest.Quest.isQuestStarted)
                        {
                            questHitCount++;
                        }
                        break;
                    case QuestState.Started:
                        if (requiredQuest.Quest.isQuestStarted)
                        {
                            questHitCount++;
                        }
                        break;
                    case QuestState.Completed:
                        if (requiredQuest.Quest.isQuestComplete)
                        {
                            questHitCount++;
                        }
                        break;
                }
            }
            if (questHitCount >= RequiredQuests.Length)
                hasQuest = true;
        }
        else hasQuest = true;

        return hasItems && hasQuest;
    }

    public void TryUseItems()
    {
        foreach (RequiredItem requiredItem in RequiredItems)
        {
            foreach (Inventory inventory in TargetInventories)
            {
                int availableAmount = inventory.ItemList[requiredItem.Item] >= requiredItem.Quantity
                    ? requiredItem.Quantity
                    : inventory.ItemList[requiredItem.Item];
                int useAmount = availableAmount;
                if (requiredItem.TaskSignature != null)
                {
                    useAmount = 0;
                    foreach (RequiredQuest quest in RequiredQuests)
                    {
                        useAmount += TryCompleteQuestTask(quest,requiredItem,availableAmount);
                    }
                }
                inventory.TryUseItem(requiredItem.Item,useAmount);
            }
        }
    }

    private int TryCompleteQuestTask(RequiredQuest targetQuest, RequiredItem targetItem, int amount)
    {
        return targetQuest.Quest.TryCompleteTask(targetItem.TaskSignature, amount);  
    }
}

[Serializable]
public class RequiredItem
{
    public Item Item;
    public int Quantity;
    public bool UseAny;
    public QuestSignature TaskSignature;
}

[Serializable]
public class RequiredQuest
{
    public Quest Quest;
    public QuestState RequiredState;
}
