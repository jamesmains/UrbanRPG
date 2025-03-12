using UnityEngine.Events;

public enum NotifyState {
    Entry,
    Exit
}

namespace Gnomes.Actor.Extension.Interactions {
    public interface IInteractable {
        public void ChangeState();
        public void SetState(bool state);
        public void Notify(NotifyState state);
        public bool RequireButtonToChangeState();
    }
}