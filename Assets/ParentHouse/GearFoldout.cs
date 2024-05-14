using ParentHouse.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ParentHouse {
    public class GearFoldout : FoldoutDisplay {
        [SerializeField] private TextMeshProUGUI header;

        private void Start() {
            LayoutRebuilder.ForceRebuildLayoutImmediate(displayRect);
            Setup();
        }

        protected override void Update() {
            switch (isOpen) {
                case true when foldoutRect.sizeDelta != openSize: {
                    var size = foldoutRect.sizeDelta;
                    size.y = Mathf.Lerp(size.y, openSize.y, openSpeed * Time.deltaTime);
                    foldoutRect.sizeDelta = size;
                    break;
                }
                case false when foldoutRect.sizeDelta != closedSize: {
                    var size = foldoutRect.sizeDelta;
                    size.y = Mathf.Lerp(size.y, closedSize.y, openSpeed * Time.deltaTime);
                    foldoutRect.sizeDelta = size;
                    break;
                }
            }
        }

        public Transform GetContainer() {
            return displayRect.transform;
        }

        public void ChangeHeader(string newText) {
            header.text = newText;
        }

        private void Setup() {
            var rect = displayRect.rect;
            openSize = new Vector2(rect.width, rect.height);
            OpenFoldout();
        }

        public override void OpenFoldout() {
            isOpen = true;
        }
    }
}