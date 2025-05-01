using Parent_House_Framework;
using UnityEngine;

namespace Urban.Urban_Time {
    [CreateAssetMenu(fileName = "TimeTogglePropagator", menuName = "TimeTogglePropagator")]
    public class TimeTogglePropagator : EventPropagator {
        public bool State;
        public override void Invoke() {
            TimeManager.OnToggleTimeFlow?.Invoke(State);
        }
    }
}