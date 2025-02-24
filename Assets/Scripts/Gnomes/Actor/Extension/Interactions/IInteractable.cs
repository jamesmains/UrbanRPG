namespace Gnomes.Actor.Extension.Interactions {
    public interface IInteractable {
        public void NotifyEntry();
        public void NotifyExit();
        public void Activate();
        public void Deactivate();
        public bool RequiresButtonPressToActivate();
        public bool RequiresButtonPressToDeactivate();
    }
}