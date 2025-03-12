using System;
using System.Collections.Generic;
using System.Linq;
using Gnomes.Actor.Behavior;
using Gnomes.Actor.Component;
using Gnomes.Interfaces;
using Parent_House_Framework.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gnomes.Actor {
    public class Actor : MonoBehaviour, ISpawnable {
        [SerializeField, FoldoutGroup("Settings")]
        [Tooltip("Leave off for Actor Spawner")]
        private bool SetActorOnStart;
    
        [SerializeField, FoldoutGroup("Settings"), ShowIf(nameof(SetActorOnStart))]
        private ActorDetails DefaultActor;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private ActorDetails CurrentActorDetails;
    
        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private bool BrainActive;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        public bool Possessed;
    
        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        public bool Dead;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private float TimeTillDespawn;
    
        [SerializeField, FoldoutGroup("Debug"), ReadOnly]
        private string BrainType;

        private ActorBrain Brain;
        public float CurrentLeashDistance => Brain?.LeashDistance ?? 1;
        public float CurrentStopDistance => Brain?.StopDistance ?? 1;

        public ActorDetails Details {
            get => CurrentActorDetails;
            private set {
                CurrentActorDetails = value;
                SwapActor();
            }
        }

        // Setup
        public Action<ActorDetails> OnActorSet;
        // Possession
        public static Action<Actor> OnTryPossess;
        public static Action<Actor> OnPossessed;
        public static Action<Actor> OnReleasePossession;
        // Movement
        public Action<Vector3, bool> OnMoveActor;
        // Weapon
        public Action<Vector2> OnAimWeapon;
        public Action OnUseWeapon;
        // Interactions
        public Action OnInteract;
        // Vitals
        public Action OnRevive;
        public Action OnDeath;
        // Spawning
        public Action<ISpawnable> OnSpawn { get; set; }
        public Action<ISpawnable> OnDespawn { get; set; }
    
        private void Awake() {
            BrainActive = true;
        }

        private void Start() {
            if (DefaultActor != null && SetActorOnStart) {
                Details = DefaultActor;
            }
        }

        private void OnEnable() {
            OnTryPossess += HandlePossession;
            OnReleasePossession += HandleReleasePossession;
            Dead = false;
        }

        private void OnDisable() {
            OnTryPossess -= HandlePossession;
            OnReleasePossession -= HandleReleasePossession;
            Despawn(); // Todo: Determine if this is a valid fallback
        }

        private void Update() {
            // Note: holds reference to TimeManager which is specific to URPG currently.
            if (!TimeManager.TimeIsRunning) return;
            if (Dead) {
                if (Time.time > TimeTillDespawn) {
                    if (Details?.DespawnEffect != null) {
                        Pooler.SpawnAt(Details.DespawnEffect, transform.position);
                    }

                    this.gameObject.SetActive(false);
                    return;
                }
            }
            else TimeTillDespawn = Details?.DespawnTimer + Time.time ?? 1f + Time.time;
            if (Brain == null || !BrainActive) return; // Check if dead before updating brain? But there may be functionality we want executed on dead actors
            Brain.Update();
        }

        private void FixedUpdate() {
            // Note: holds reference to TimeManager which is specific to URPG currently.
            if (Brain == null || !BrainActive || !TimeManager.TimeIsRunning) return; // Check if dead before updating brain?
            Brain.FixedUpdate();
        }
    
        public void Spawn() {
            OnSpawn?.Invoke(this);
            ReviveActor();
        }

        public void Despawn() {
            OnDespawn?.Invoke(this);
        }

        private void HandlePossession(Actor actor) {
            if (actor != this) return;
            BrainActive = false;
            Possessed = true;
            OnPossessed.Invoke(this);
        }

        [FoldoutGroup("Buttons"),Button]
        private void HandleReleasePossession(Actor actor) {
            if (actor != this) return;
            Possessed = false;
            BrainActive = true;
        }

        [FoldoutGroup("Buttons"),Button]
        public void ReviveActor() {
            Dead = false;
            OnRevive?.Invoke();
        }

        [FoldoutGroup("Buttons"),Button]
        public void KillActor() {
            Dead = true;
            OnDeath?.Invoke();
        }
    
        private void SwapActor() {
            OnActorSet.Invoke(Details);
            Brain = Details?.BrainBehavior?.Create<ActorBrain>(this) as ActorBrain;
            BrainType = Brain?.GetType().Name;
        }

        [FoldoutGroup("Buttons"),Button]
        public void SwapActor(ActorDetails newDetails) {
            Details = newDetails;
        }

    
    }
}