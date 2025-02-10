using System;
using UnityEngine;

namespace Gnomes.Actor.Behavior {
    [Serializable]
    public abstract class ActorBehavior {
        public Actor OwnerActor;
        public abstract void Update();
        public abstract void FixedUpdate();
        public abstract ActorBehavior Create<T>(Actor owner);

        protected ActorBehavior(Actor ownerActor) {
            OwnerActor = ownerActor;
        }
        
        protected ActorBehavior() {}
    }
}
