using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gnomes.Actor.Extension.Spawner {
    public class ActorSpawner : Gnomes.Spawner.Spawner {
        [SerializeField, FoldoutGroup("Settings")]
        private List<ActorDetails> ActorPool;
    
        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private int ActorSpawnIndex;

        protected override void Spawn() {
            base.Spawn();
            var spawnedActor = CurrentSpawningObject.GetComponent<Actor>();
            spawnedActor.SwapActor(ActorPool[GetIndex(ref ActorSpawnIndex, ActorPool.Count)]);
        }
    }
}
