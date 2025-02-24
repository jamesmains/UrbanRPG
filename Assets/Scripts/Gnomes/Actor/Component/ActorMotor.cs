using System.Collections.Generic;
using Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;

// Todo: Maybe find a new nav agent component
namespace Gnomes.Actor.Component {
    [RequireComponent(typeof(FollowerEntity))]
    public class ActorMotor : ActorComponent
    {
        [SerializeField, FoldoutGroup("Dependencies"), ReadOnly]
        public FollowerEntity Agent;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        public int FacingDirection; // -1 left, 1 right

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private float CachedStopDistance;

        protected override void OnEnable() {
            base.OnEnable();
            if(Agent == null)
                Agent = GetComponent<FollowerEntity>();
            FacingDirection = 1;
            Actor.OnMoveActor += MoveAgent;
            Actor.OnPossessed += HandlePossession;
            Actor.OnReleasePossession += HandleReleasePossession;
            Actor.OnActorSet += HandleSwapActor;
        }

        protected override void OnDisable() {
            base.OnDisable();
            Actor.OnMoveActor -= MoveAgent;
            Actor.OnPossessed -= HandlePossession;
            Actor.OnReleasePossession -= HandleReleasePossession;
            Actor.OnActorSet -= HandleSwapActor;
        }

        private void MoveAgent(Vector3 moveTarget, bool asDirection) {
            if (Actor.Dead) return;
            var targetPosition = asDirection ? transform.position + moveTarget: moveTarget;
            if (Agent.destination != targetPosition) {
                Agent.SetDestination(targetPosition);
                if(moveTarget.x != 0)
                    FacingDirection = moveTarget.x > 0 ? -1 : 1;
            }
        }

        private void HandleSwapActor(ActorDetails actorDetails) {
            CachedStopDistance = Actor.CurrentStopDistance;
            Agent.stopDistance = Actor.Possessed ? 0 : CachedStopDistance;
        }

        private void HandlePossession(Actor actor) {
            if (actor != Actor) return;
            Agent.stopDistance = 0f;
        }

        private void HandleReleasePossession(Actor actor) {
            if (actor != Actor) return;
            Agent.stopDistance = CachedStopDistance;
        }
    }
}
