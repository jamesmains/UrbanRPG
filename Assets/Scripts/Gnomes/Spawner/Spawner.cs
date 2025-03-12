using System;
using System.Collections.Generic;
using Gnomes.Interfaces;
using Parent_House_Framework.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Gnomes.Spawner {
    [Flags]
    public enum SpawnerType {
        Once = 0,
        AutomaticTrigger = 1 << 0,
        RequiresTrigger = 1 << 1,
        LimitedSpawns = 1 << 2,
        UnlimitedSpawns = 1 << 3,
        RandomOrder = 1 << 4,
        ForwardOrder = 1 << 5,
        ReverseOrder = 1 << 6,
        PingPongOrder = 1 << 7,
        RandomNearbyLocation = 1 << 8,
        SetOrderedLocation = 1 << 9,
        SetRandomLocation = 1 << 10,
    }

    public class Spawner : SerializedMonoBehaviour {
        [SerializeField, FoldoutGroup("Dependencies"), ShowIf(nameof(HasSetSpawnLocations))]
        private List<Transform> SpawnLocations = new();

        [SerializeField, FoldoutGroup("Settings")]
        private SpawnerType Type = SpawnerType.AutomaticTrigger & SpawnerType.UnlimitedSpawns & SpawnerType.RandomOrder;

        [SerializeField, FoldoutGroup("Settings"), ShowIf(nameof(HasLimitedMaxSpawn))]
        private int MaxSpawnedAtOnce = 10;

        [SerializeField, FoldoutGroup("Settings"), ShowIf(nameof(HasSpawnLimit))]
        private int SpawnLimit = 10;

        [SerializeField, FoldoutGroup("Settings")]
        private float SpawnFrequency = 0;

        [SerializeField, FoldoutGroup("Settings")]
        private int MaxSpawnsPerAttempt = 1;

        [SerializeField, FoldoutGroup("Settings"), HideIf(nameof(HasSetSpawnLocations))]
        private float MaxSpawnLocationDistance = 3;

        [SerializeField, FoldoutGroup("Settings")]
        private List<GameObject> SpawnPool = new();

        [SerializeField, FoldoutGroup("Status"), ReadOnly, ShowIf(nameof(HasSpawnLimit))]
        private int SpawnsRemaining;
    
        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        [Tooltip("This can be used to intercept spawning object with an override")]
        protected GameObject CurrentSpawningObject;
    
        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private readonly List<ISpawnable> SpawnedEntities = new();

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private int SpawnIndex;

        [SerializeField, FoldoutGroup("Status"), ReadOnly, ShowIf(nameof(HasSetSpawnLocations))]
        private int LocationIndex;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private float LastSpawnTime;

        [SerializeField, FoldoutGroup("Status"), ReadOnly, ShowIf(nameof(IsPingPongOrdered))]
        private int PingPongDirection = 1;

        [SerializeField, FoldoutGroup("Events")]
        private UnityEvent OnSpawn;
    
        private bool HasLimitedMaxSpawn =>
            (Type & SpawnerType.UnlimitedSpawns) > 0 || (Type & SpawnerType.LimitedSpawns) > 0;

        private bool HasSpawnLimit => (Type & SpawnerType.LimitedSpawns) > 0;
        private bool IsPingPongOrdered => (Type & SpawnerType.PingPongOrder) > 0;

        private bool HasSetSpawnLocations =>
            (Type & SpawnerType.SetOrderedLocation) > 0 || (Type & SpawnerType.SetRandomLocation) > 0;

        private void Awake() {
            Reset();
        }

        private void OnDisable() {
            Reset();
        }

        private void Update() {
            if ((Type & SpawnerType.AutomaticTrigger) == 0) return;
            if (Time.time > LastSpawnTime + SpawnFrequency) {
                TrySpawn();
                LastSpawnTime = Time.time;
            }
        }

        [Button]
        private void Reset() {
            if (HasSpawnLimit) {
                SpawnsRemaining = SpawnLimit;
            }

            SpawnIndex = 0;
        }

        [Button]
        public void TrySpawn() {
            for (int i = 0; i < MaxSpawnsPerAttempt; i++) {
                if (HasSpawnLimit && SpawnsRemaining <= 0) break;
                if (HasLimitedMaxSpawn && SpawnedEntities.Count >= MaxSpawnedAtOnce) break;
                Spawn();
            }
        }

        protected virtual void Spawn() {
            CurrentSpawningObject = Pooler.SpawnAt(SpawnPool[GetIndex(ref SpawnIndex, SpawnPool.Count)].gameObject, GetSpawnLocation());
            var spawnable = CurrentSpawningObject.GetComponent<ISpawnable>();
            SubscribeSpawn(spawnable);
            spawnable.Spawn();

            if (HasSpawnLimit) {
                SpawnsRemaining -= 1;
            }
        }

        protected virtual void HandleSpawn(ISpawnable spawnable) {
            if (SpawnedEntities.Contains(spawnable)) {
                Debug.LogError($"Spawner Err: there is a duplicate element in the SpawnedEntities list");
                return;
            }
            SpawnedEntities.Add(spawnable);
        }

        protected virtual void HandleDespawn(ISpawnable spawnable) {
            if (!SpawnedEntities.Contains(spawnable)) {
                Debug.LogError($"Spawner Err: there is a missing element in the SpawnedEntities list");
                return;
            }
            SpawnedEntities.Remove(spawnable);
            if (HasSpawnLimit && SpawnsRemaining <= 0) {
                this.gameObject.SetActive(false);
            }
        }

        private void SubscribeSpawn(ISpawnable spawnable) {
            if (spawnable == null) return;
            spawnable.OnSpawn += HandleSpawn;
            spawnable.OnDespawn += HandleDespawn;
            spawnable.OnDespawn += UnsubscribeSpawn;
        }

        private void UnsubscribeSpawn(ISpawnable spawnable) {
            spawnable.OnSpawn -= HandleSpawn;
            spawnable.OnDespawn -= HandleDespawn;
            spawnable.OnDespawn -= UnsubscribeSpawn;
        }

        // This is using a ref because inheritors may want to have a similar list format to get an index using the spawn type order
        protected int GetIndex(ref int index, int listCount) {
            if ((Type & SpawnerType.RandomOrder) > 0) {
                index = Random.Range(0, listCount);
            }
            else if ((Type & SpawnerType.ForwardOrder) > 0) {
                index++;
                index %= listCount;
            }
            else if ((Type & SpawnerType.ReverseOrder) > 0) {
                index--;
                if (index < 0) {
                    index = listCount - 1;
                }
            }
            else if ((Type & SpawnerType.PingPongOrder) > 0) {
                index += PingPongDirection;
                if (index >= listCount && PingPongDirection == 1) {
                    PingPongDirection = -1;
                }
                else if (index < 0 && PingPongDirection == -1) {
                    PingPongDirection = 1;
                }

                index = Math.Clamp(index, 0, listCount - 1);
            }
            else {
                Debug.LogError("Spawner Err: Trying to get spawn index without Order Type on" + this.gameObject.name);
            }

            return index;
        }

        private Vector3 GetSpawnLocation() {
            if (HasSetSpawnLocations && SpawnLocations.Count > 0) {
                if ((Type & SpawnerType.SetRandomLocation) > 0)
                    return SpawnLocations[Random.Range(0, SpawnLocations.Count)].position;
                else return SpawnLocations[LocationIndex++ % SpawnLocations.Count].position;
            }
            else if (!HasSetSpawnLocations) {
                Vector3 location = Vector3.zero;
                location.x = Random.Range(-MaxSpawnLocationDistance, MaxSpawnLocationDistance);
                location.z = Random.Range(-MaxSpawnLocationDistance, MaxSpawnLocationDistance);
                return transform.position + location;
            }

            Debug.LogError("Spawner Err: Couldn't find location on " + this.gameObject.name);
            return Vector3.zero;
        }
    }
}