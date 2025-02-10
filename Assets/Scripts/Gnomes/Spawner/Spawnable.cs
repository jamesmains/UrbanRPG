using System;
using Gnomes.Interfaces;
using UnityEngine;

// This is for more generic spawnables that don't really need to care about what happens when spawned.
namespace Gnomes.Spawner {
    public class Spawnable : MonoBehaviour, ISpawnable
    {
        private void OnDisable() {
            Despawn();
        }

        public Action<ISpawnable> OnSpawn { get; set; }
        public Action<ISpawnable> OnDespawn { get; set; }
        public void Spawn() {
            OnSpawn.Invoke(this);
        }

        public void Despawn() {
            OnDespawn?.Invoke(this);
        }
    }
}
