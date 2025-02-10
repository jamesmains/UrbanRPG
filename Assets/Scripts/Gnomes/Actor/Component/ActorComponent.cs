using Sirenix.OdinInspector;
using UnityEngine;

namespace Gnomes.Actor.Component {
    public class ActorComponent: MonoBehaviour {
    
        [SerializeField, FoldoutGroup("Dependencies"),ReadOnly]
        public Actor Actor;

        protected virtual void OnEnable() {
            if(Actor == null) Actor = GetComponent<Actor>();
        
            Actor.OnActorSet += HandleActorChanged;
            Actor.OnDeath += HandleActorDeath;
        }
    
        protected virtual void OnDisable() {
            Actor.OnActorSet -= HandleActorChanged;
            Actor.OnDeath -= HandleActorDeath;
        }
    
        protected virtual void HandleActorChanged(ActorDetails newDetails) {
        
        }

        protected virtual void HandleActorDeath() {
        
        }
    }
}