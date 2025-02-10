using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gnomes.Actor.Behavior {
    [Serializable]
    public abstract class ActorBrain : ActorBehavior {
        protected ActorBrain(Actor ownerActor) : base(ownerActor) {
        }

        protected ActorBrain() : base() {
        }

        [SerializeField, FoldoutGroup("Settings")]
        public float LeashDistance = 5f;

        [SerializeField, FoldoutGroup("Settings")]
        public float StopDistance = 1.5f;
    }

    [Serializable]
    public class MinionBrain : ActorBrain {
        public override ActorBehavior Create<T>(Actor owner) {
            return new MinionBrain(owner);
        }
        
        public MinionBrain(Actor owner) : base(owner) {
        }

        public MinionBrain() : base() {
        }

        // Todo: add seek function for finding leader?

        [SerializeField, FoldoutGroup("Debug"), ReadOnly]
        public Actor LeaderActor;

        [SerializeField, FoldoutGroup("Debug"), ReadOnly]
        private float LastTimeSignaled;

        public override void Update() {
            if (!(Time.time > LastTimeSignaled + 1f)) return;
            LastTimeSignaled = Time.time;
            Vector3 RandomNewPosition = Vector3.zero;
            RandomNewPosition.x = Random.Range(-1f, 1f);
            RandomNewPosition.z = Random.Range(-1f, 1f);
            if (LeaderActor == null)
                OwnerActor.OnMoveActor?.Invoke(RandomNewPosition, true);
            else {
                if (Vector3.Distance(OwnerActor.transform.position, LeaderActor.transform.position) < 1.5f) {
                    var moveAwayDirection = (OwnerActor.transform.position - LeaderActor.transform.position).normalized;
                    OwnerActor.OnMoveActor?.Invoke(moveAwayDirection, true);
                }
                else if (Vector3.Distance(OwnerActor.transform.position, LeaderActor.transform.position) > 2f) {
                    OwnerActor.OnMoveActor?.Invoke(LeaderActor.transform.position, false);
                }
            }
        }

        public override void FixedUpdate() {
        }
    }

    [Serializable]
    public class LeaderBrain : ActorBrain {
        public override ActorBehavior Create<T>(Actor owner) {
            return new LeaderBrain(owner);
        }

        public LeaderBrain(Actor ownerActor) : base(ownerActor) {
        }

        public LeaderBrain() : base() {
        }

        public override void Update() {
        }

        public override void FixedUpdate() {
        }
    }
}