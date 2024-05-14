using ParentHouse.UI;
using UnityEngine;

namespace ParentHouse {
    public class PopupDisplayController : MonoBehaviour {
        [SerializeField] private GameObject HorizontalPopup;
        [SerializeField] private GameObject IconPopupObject;

        private void OnEnable() {
        }

        private void OnDisable() {
        }

        public void OnCreateHorizontalPopup(Sprite icon, string value) {
            Instantiate(HorizontalPopup, transform).GetComponent<DisplayPopup>().Setup(icon, value);
        }

        public void OnCreateIconPopup(Sprite icon) {
            Instantiate(IconPopupObject, transform).GetComponent<DisplayPopup>().Setup(icon);
        }
    }
}