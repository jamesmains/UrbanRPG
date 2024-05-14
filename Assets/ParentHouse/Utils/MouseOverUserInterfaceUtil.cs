using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ParentHouse.Utils {
    public class MouseOverUserInterfaceUtil : MonoBehaviour {
        [SerializeField] private LayerMask UILayer;
        private int layer;

        private void Awake() {
            layer = 5;
        }

        private void Update() {
            Global.IsMouseOverUI = IsPointerOverUIElement();
        }


        //Returns 'true' if we touched or hovering on Unity UI element.
        public bool IsPointerOverUIElement() {
            return IsPointerOverUIElement(GetEventSystemRaycastResults());
        }


        //Returns 'true' if we touched or hovering on Unity UI element.
        private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults) {
            for (var index = 0; index < eventSystemRaysastResults.Count; index++) {
                var curRaysastResult = eventSystemRaysastResults[index];
                if (curRaysastResult.gameObject.layer == layer) {
                    Debug.Log($"Mouse is over {curRaysastResult.gameObject.name} (MouseOverUserInterfaceUtil.cs)");
                    return true;
                }
            }

            return false;
        }


        //Gets all event system raycast results of current mouse or touch position.
        private static List<RaycastResult> GetEventSystemRaycastResults() {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            return raycastResults;
        }
    }
}