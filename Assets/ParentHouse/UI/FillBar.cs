using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ParentHouse.UI {
    /// <summary>
    /// Handles the displaying of a bar fill amount.
    /// Optionally can handle the slug bar for the fill as well.
    /// </summary>
    public class FillBar : MonoBehaviour {
        [SerializeField] [FoldoutGroup("Settings")]
        protected float FillSpeed;

        [SerializeField] [FoldoutGroup("Settings")]
        protected float SlugFillSpeed;

        [SerializeField] [FoldoutGroup("Settings")]
        protected Vector2 FillSizeRange;

        [SerializeField] [FoldoutGroup("Dependencies")]
        protected RectTransform FillImageRect;

        [SerializeField] [FoldoutGroup("Dependencies")]
        protected RectTransform FillSlugImageRect;

        [SerializeField] [FoldoutGroup("Dependencies")]
        protected Image FillImage;

        [SerializeField] [FoldoutGroup("Dependencies")]
        protected Image FillSlugImage;

        // Fill bar via rect size on Rect Transform.
        public void UpdateBar(float value) {
            var size = FillImageRect.sizeDelta;
            size.x = (FillSizeRange.y - FillSizeRange.x) * value + FillSizeRange.x;

            FillImageRect.sizeDelta = Vector2.Lerp(FillImageRect.sizeDelta, size, FillSpeed * Time.deltaTime);

            if (FillSlugImageRect == null) return;

            FillSlugImageRect.sizeDelta =
                Vector2.Lerp(FillSlugImageRect.sizeDelta, size, SlugFillSpeed * Time.deltaTime);
        }

        // Fill bar via Fill Amount on an Image.
        public void UpdateRadialBar(float value) {
            FillImage.fillAmount = Mathf.Lerp(FillImage.fillAmount, value, FillSpeed * Time.deltaTime);

            if (FillSlugImage == null) return;

            FillSlugImage.fillAmount = Mathf.Lerp(FillSlugImage.fillAmount, value, SlugFillSpeed * Time.deltaTime);
        }
    }
}