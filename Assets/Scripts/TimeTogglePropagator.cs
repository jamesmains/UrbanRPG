using UnityEngine;

[CreateAssetMenu(fileName = "TimeTogglePropagator", menuName = "TimeTogglePropagator")]
public class TimeTogglePropagator : EventPropagator {
    public bool State;
    public override void Invoke() {
        TimeManager.OnToggleTimeFlow?.Invoke(State);
    }
}