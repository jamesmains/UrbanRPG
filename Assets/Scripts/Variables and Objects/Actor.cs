using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Actor", menuName = "Signatures/Actor")]
public class Actor : ScriptableObject
{
    public string actorName;
    public Sprite actionIcon;
    public Color hairColor;
    public List<ReputationTier> reputationTiers = new();
    public List<AcceptableItemGifts> acceptedGifts = new();
    public AudioClip talkSfx;
    [Range(-1,99)]public int currentReputation = -1;
    public List<GearOption> EquippedGear;
    
    [Button]
    public void AdjustReputation(int adjustAmount)
    {
        string currentTier = GetCurrentReputationTier();
        currentReputation += adjustAmount;
        currentReputation = Mathf.Clamp(currentReputation, 0, 99);
        
        string newTier = GetCurrentReputationTier();
        if (newTier != currentTier)
        {
            GameEvents.OnSendReputationChangeMessage.Raise(this);
        }
    }
    
    [Button]
    public string GetCurrentReputationTier()
    {
        for (var index = reputationTiers.Count-1; index > -1; index--)
        {
            var tier = reputationTiers[index];
            if (currentReputation >= tier.requiredValue) return tier.tierName;
        }
        return "UNKNOWN";
    }
}

[Serializable]
public class ReputationTier
{
    public string tierName;
    public int requiredValue;
    [PreviewField] public Sprite tierIcon;
}

[Serializable]
public class AcceptableItemGifts
{
    public Item giftItem;
    public int reputationChange;
}