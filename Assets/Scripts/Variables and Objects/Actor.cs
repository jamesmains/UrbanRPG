using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Actor", menuName = "Dialogue/Actor")]
public class Actor : ScriptableObject
{
    public string actorName;
    public Sprite actionIcon;
    public Color hairColor;
    public List<ReputationTier> reputationTiers = new();
    [Range(-1,100)]public int currentReputation = -1;
    [FoldoutGroup("Gear"), SerializeField] public Gear GearBody;
    [FoldoutGroup("Gear"), SerializeField] public Gear GearShoes;
    [FoldoutGroup("Gear"), SerializeField] public Gear GearPants;
    [FoldoutGroup("Gear"), SerializeField] public Gear GearMouth;
    [FoldoutGroup("Gear"), SerializeField] public Gear GearEyes;
    [FoldoutGroup("Gear"), SerializeField] public Gear GearShirt;
    [FoldoutGroup("Gear"), SerializeField] public Gear GearHair;
    [FoldoutGroup("Gear"), SerializeField] public Gear GearAccessory;

    [Button]
    public void AdjustReputation(int adjustAmount)
    {
        currentReputation -= adjustAmount;
        currentReputation = Mathf.Clamp(currentReputation, 0, 99);
        GameEvents.OnReputationChange.Raise();
    }
    
    [Button]
    public string GetCurrentReputationTier()
    {
        for (var index = reputationTiers.Count-1; index > -1; index--)
        {
            var tier = reputationTiers[index];
            if (currentReputation >= tier.requiredValue)
            {
             return tier.tierName;
            }
        }

        Debug.Log($"Reputation Name: UNKNOWN");
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