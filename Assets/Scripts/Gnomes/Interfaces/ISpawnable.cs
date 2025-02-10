using System;

namespace Gnomes.Interfaces {
    public interface ISpawnable {
        public Action<ISpawnable> OnSpawn{ get; set; }
        public Action<ISpawnable> OnDespawn { get; set; }

        public void Spawn();
        public void Despawn();
    }
}