using ParentHouse.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace ParentHouse {
    /// <summary>
    /// Shows information about something the player is hovering their cursor over.
    /// Todo: This class is a bit of a mess and the entry point may need to be adjusted along with the fact it
    ///       derives from WindowPanel.
    /// </summary>
    public class TooltipDisplay : WindowDisplay {
        [SerializeField] [FoldoutGroup("Dependencies")]
        private TextMeshProUGUI MessageText;

        [SerializeField] [FoldoutGroup("Settings")]
        private Vector3 Offset;

        [SerializeField] [FoldoutGroup("Dependencies")]
        private RectTransform Rect;

        [SerializeField] [FoldoutGroup("Dependencies")]
        private Canvas Scaler;

        private void Update() {
            if (!IsActive) return;
            var targetPosition = Input.mousePosition / Scaler.scaleFactor + Offset;
            targetPosition.x =
                Mathf.Clamp(targetPosition.x, 0,
                    1920 - Rect.rect
                        .width); // TODO: investigate if this needs to be using reference resolution or actual
            targetPosition.y = Mathf.Clamp(targetPosition.y, 0, 1080 - Rect.rect.height);
            Rect.anchoredPosition = targetPosition;
        }


        private void ShowMessage(string message) {
            MessageText.text = message;
            Show();
        }
    }
}