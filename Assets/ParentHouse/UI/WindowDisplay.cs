using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse.UI {
    /// <summary>
    /// Handles generic menu display.
    /// </summary>
    public class WindowDisplay : MonoBehaviour {
        [FoldoutGroup("Settings")] public Window Signature;

        [FoldoutGroup("Dependencies")] [Required]
        public CanvasGroup CanvasGroup;

        [FoldoutGroup("Status")] public bool IsActive;

        private void OnEnable() {
            Hide();
            Window.AllWindowDisplays.Add(this);
        }

        private void OnDisable() {
            Window.AllWindowDisplays.Remove(this);
        }

        public virtual void Show() {
            CanvasGroup.alpha = 1;
            CanvasGroup.blocksRaycasts = true;
            IsActive = true;
        }


        public virtual void Hide() {
            CanvasGroup.alpha = 0;
            CanvasGroup.blocksRaycasts = false;
            IsActive = false;
        }
    }
}