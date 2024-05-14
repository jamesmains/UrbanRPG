using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

// Presume target is always going to be player since this relies on keyboard or gamepad input
namespace ParentHouse {
    public class InteractionTrigger : MonoBehaviour {
        public static InteractionTrigger targetTrigger; // try to prevent multiple interactions at once
        [SerializeField] private bool requireKeyPress;
        [SerializeField] private InteractionType interactionType;
        [FoldoutGroup("Data")] public bool readyToInteract;
        [SerializeField] private UnityEvent onInteract;

        private void OnEnable() {
        }

        private void OnDisable() {
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player") && interactionType == InteractionType.OnEnter && !requireKeyPress) Trigger();
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player") && readyToInteract) {
                readyToInteract = false;
                if (targetTrigger == this) targetTrigger = null;
            }
        }

        private void OnTriggerStay(Collider other) {
            if (other.CompareTag("Player") && !readyToInteract) {
                readyToInteract = true;
                targetTrigger = this;
            }
        }

        private void Trigger() {
            if (targetTrigger != this && Global.PlayerLock > 0) return;
            onInteract.Invoke();
        }
    }
}