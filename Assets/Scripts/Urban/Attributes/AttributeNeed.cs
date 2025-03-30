using System;
using Parent_House_Framework.Values;
using UnityEngine;

namespace Urban.Attributes {
    /// <summary>
    /// Ideally the value would be based on the game time so that the value
    /// isn't depleted while pause menus or cutscenes are playing
    /// </summary>
    public class AttributeNeed : Attribute {
        public AttributeNeed(AttributeDetails details) : base(details) {
            NeedValue.OnValueChanged += ChangeValue;
        }

        ~AttributeNeed() {
            NeedValue.OnValueChanged -= ChangeValue;
        }

        // Todo: rename this variable
        public readonly ObservableValue<float> NeedValue = new();

        // To compare if value is going up or down
        private float CachedValue;

        private float CachedForgivenessTime;

        // This is for when the value hits 100% and delays until it begins to decay again
        private const float ForgivenessDelay = 5f; // Todo: Need final value

        private const float DecaySpeed = 10f; // Todo: Need final value

        public Action OnReachedFull;
        public Action OnReachedGood;
        public Action OnReachedLow;
        public Action OnReachedCritical;

        public enum NeedState {
            Critical,
            Low,
            Good,
            Full
        }

        public NeedState CurrentState = NeedState.Good;

        public void Reset() {
        }

        public void Decay() {
            if (Time.time > CachedForgivenessTime) {
                NeedValue.Value -= DecaySpeed * Time.deltaTime;
            }
        }

        private void ChangeValue(float value) {
            NeedValue.SetWithoutNotify(Mathf.Clamp(NeedValue.Value, 0, 100));
            if (value > CachedValue && value >= 100) {
                CachedForgivenessTime = Time.time + ForgivenessDelay;
                OnReachedFull?.Invoke();
                CurrentState = NeedState.Full;
            }

            CachedValue = value;
            if (CachedValue is < 33 and > 0 && CurrentState != NeedState.Low) {
                OnReachedLow?.Invoke();
                CurrentState = NeedState.Low;
            }
            else if (CachedValue <= 0 && CurrentState != NeedState.Critical) {
                OnReachedCritical?.Invoke();
                CurrentState = NeedState.Critical;
            }
            else if (CachedValue is >= 33 and < 100 && CurrentState != NeedState.Good) {
                OnReachedGood?.Invoke();
                CurrentState = NeedState.Good;
            }
        }
    }
}