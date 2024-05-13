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
    public AudioClip talkSfx;
    public List<GearOption> EquippedGear;
    public Faction faction;
    
    [Button]
    public void AdjustReputation(int adjustAmount)
    {
        string currentTier = GetCurrentReputationTier();
        faction.currentReputation += adjustAmount;
        faction.currentReputation = Mathf.Clamp(faction.currentReputation, 0, 99);
        
        string newTier = GetCurrentReputationTier();
        if (newTier != currentTier)
        {
            GameEvents.OnSendReputationChangeMessage.Invoke(this);
        }
    }
    
    [Button]
    public string GetCurrentReputationTier()
    {
        for (var index = faction.reputationTiers.Count-1; index > -1; index--)
        {
            var tier = faction.reputationTiers[index];
            if (faction.currentReputation >= tier.requiredValue) return tier.tierName;
        }
        return "UNKNOWN";
    }
}

