using System;
using Gnomes.Actor.Behavior;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gnomes.Actor {
    [Flags]
    public enum ActorTeam {
        NoTeam = 0,
        Team0 = 1 << 0,
        Team1 = 1 << 1,
        Team2 = 1 << 2,
        Team3 = 1 << 3,
    }
// Reminder to myself
// to compare flags (bit wise operation)
// (currentEnum & incomingEnum) != 0

    [CreateAssetMenu(fileName = "New Actor Details", menuName = "GNOME/Actor Details")]
    public class ActorDetails : SerializedScriptableObject {

        [HorizontalGroup("DetailsSplit",0.8f), VerticalGroup("DetailsSplit/Left")]
        [SerializeField] 
        public string ActorName;

        [HorizontalGroup("DetailsSplit", 0.8f), VerticalGroup("DetailsSplit/Left")] [SerializeField]
        public GameObject DespawnEffect;

        [HorizontalGroup("DetailsSplit", 0.8f), VerticalGroup("DetailsSplit/Left")] [SerializeField]
        public float DespawnTimer;

        [HorizontalGroup("DetailsSplit",0.8f), VerticalGroup("DetailsSplit/Left")]
        [SerializeField] 
        public ActorBrain BrainBehavior;
    }
}