namespace Gnomes.Actor.Behavior {
    public class ActorAimBehavior : ActorBehavior {
        public override ActorBehavior Create<T>(Actor owner) {
            return new ActorAimBehavior(owner);
        }

        public ActorAimBehavior(Actor ownerActor) : base(ownerActor) {
        }

        public override void Update() {
            throw new System.NotImplementedException();
        }

        public override void FixedUpdate() {
            throw new System.NotImplementedException();
        }
    }
}