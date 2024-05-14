using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ParentHouse {
    public class InterfaceInteractable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
        IPointerClickHandler {
        public bool canInteractWith;
        public UnityEvent onInteract;

        private void OnEnable() {
        }

        private void OnDisable() {
        }

        public void OnPointerClick(PointerEventData eventData) {
        }

        public void OnPointerEnter(PointerEventData eventData) {
            canInteractWith = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            canInteractWith = false;
        }

        private void Interact() {
            if (!canInteractWith) return;
            onInteract.Invoke();
        }
    }
}