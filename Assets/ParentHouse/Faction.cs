using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse {
    [CreateAssetMenu(fileName = "Faction", menuName = "Signatures/Faction")]
    public class Faction : ScriptableObject {
        public string factionName;
        public List<ReputationTier> reputationTiers = new();
        public List<AcceptableItemGifts> acceptedGifts = new();
        [Range(-1, 99)] public int currentReputation = -1;
    }

    [Serializable]
    public class ReputationTier {
        public string tierName;
        public int requiredValue;
        [PreviewField] public Sprite tierIcon;
    }

    [Serializable]
    public class AcceptableItemGifts {
        public Item giftItem;
        public int reputationChange;
    }
}