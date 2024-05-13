using ParentHouse.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace ParentHouse {
    public class Tooltip : WindowPanel
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Vector3 offset;
        [FoldoutGroup("Details")][SerializeField] private RectTransform rect;
        [FoldoutGroup("Details")][SerializeField] private Canvas scaler;


        private void ShowMessage(string message)
        {
            messageText.text = message;
            Show();
        }

        private void Update()
        {
            if (!IsActive) return;
            var targetPosition = (Input.mousePosition / scaler.scaleFactor) +offset;
            targetPosition.x = Mathf.Clamp(targetPosition.x, 0, 1920 - (rect.rect.width)); // TODO: investigate if this needs to be using reference resolution or actual
            targetPosition.y = Mathf.Clamp(targetPosition.y, 0, 1080 - (rect.rect.height));
            rect.anchoredPosition = targetPosition;
        }
    }
}
