using System;
using System.Collections.Generic;
using System.Linq;
using gnomes.Actor;
using parent_house_framework.UI.Notifications;
using parent_house_framework.Utils;
using parent_house_framework.Values;
using Sirenix.OdinInspector;
using UnityEngine;

public static class ReputationTiers {
    static ReputationTiers() {
        m_reputationTiers.Clear();
        m_reputationTiers.Add(new ReputationTier(0, 10, "Distant"));
        m_reputationTiers.Add(new ReputationTier(11, 30, "Acquaintance"));
        m_reputationTiers.Add(new ReputationTier(31, 50, "Friend"));
        m_reputationTiers.Add(new ReputationTier(51, 75, "Good Friend"));
        m_reputationTiers.Add(new ReputationTier(76, 98, "Close Friend"));
        m_reputationTiers.Add(new ReputationTier(99, 100, "Best Friend"));
    }

    private static readonly List<ReputationTier> m_reputationTiers = new();

    public static string GetTierName(int value) {
        foreach (var tier in m_reputationTiers) {
            if (value >= tier.Min && value <= tier.Max)
                return tier.TierName;
        }

        return null;
    }
}

public class ReputationTier {
    public ReputationTier(int min, int max, string tierName) {
        Min = min;
        Max = max;
        TierName = tierName;
    }

    public readonly int Min;
    public readonly int Max;
    public readonly string TierName;
}

public class ReputationManager : SerializedMonoBehaviour {
    [SerializeField, FoldoutGroup("Dependencies")]
    private RectTransform ReputationDisplayContainer;
    
    [SerializeField, FoldoutGroup("Dependencies")]
    private GameObject ReputationDisplayPrefab;

    [SerializeField, FoldoutGroup("Status"), ReadOnly]
    private List<Reputation> Reputations = new();

    public static Action<ActorDetails, int> OnModifyReputation;
    
    // Currently this only is called when the tier is changed
    public static Action<ActorDetails, int> OnReputationChanged;

    private void OnEnable() {
        OnModifyReputation += HandleModifyReputationWithNpc;
    }

    private void OnDisable() {
        OnModifyReputation -= HandleModifyReputationWithNpc;
    }

    [Button]
    private void DebugModifyNpcReputation(ActorDetails details, int amount) {
        OnModifyReputation?.Invoke(details, amount);
    }

    private void HandleModifyReputationWithNpc(ActorDetails details, int amount) {
        var reputation = Reputations.FirstOrDefault(o => o.GetId() == details.GetId());
        if (reputation == null) {
            Reputations.Add(new Reputation(details, amount));
            var displayObj = Pooler.Spawn(ReputationDisplayPrefab, ReputationDisplayContainer);
            if (displayObj.TryGetComponent(out ReputationDisplay display)) {
                display.Build(details);
            }
            else {
                Debug.LogError($"Unable to find reputation display on {display.gameObject}");
            }
        }
        else reputation.Modify(amount);
    }
}

[Serializable]
public class Reputation {
    public Reputation(ActorDetails details, int initialValue) {
        m_currentReputation = new ChainedInt(100,0,false);
        m_currentReputation.ObservedValue.OnValueChanged += CheckReputationChange;
        m_actorDetails = details;
        Modify(initialValue);
    }

    private ActorDetails m_actorDetails;
    private ChainedInt m_currentReputation;

    [SerializeField]
    private int m_cachedValue;

    [SerializeField]
    private string m_currentTier;

    public void Modify(int amount) {
        m_cachedValue = m_currentReputation.Value;
        m_currentReputation.AddValue(amount);
    }

    private void CheckReputationChange(int newValue) {
        if (m_cachedValue == m_currentReputation.Value) return;

        bool gainedReputation = m_currentReputation.Value >= m_cachedValue;
        int difference = Mathf.Abs(m_currentReputation.Value - m_cachedValue);
        m_cachedValue = m_currentReputation.Value;

        string sign = gainedReputation ? "+" : "-";
        string notificationString = $"{sign} {difference} cred with {m_actorDetails.ActorName}";

        string newTier = ReputationTiers.GetTierName(newValue);

        if (newTier != m_currentTier) {
            m_currentTier = newTier;
            notificationString += $", now {m_currentTier}";
            
            // Currently this only pings when the tier is changed
            ReputationManager.OnReputationChanged?.Invoke(m_actorDetails, m_currentReputation.Value);
        }

        notificationString += gainedReputation ? "!" : "...";

        var reputationChangeNotification = new Notification(notificationString, NotificationTypes.Toaster);
        reputationChangeNotification.Send();
    }
    
    public Guid GetId() {
        return m_actorDetails.Id;
    }
}