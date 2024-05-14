using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse.UI {
    /// <summary>
    /// Handles generic menu display.
    /// It also handles the static calls for hiding or showing window panels.
    /// </summary>
    public class WindowPanel : MonoBehaviour {
        private static readonly List<WindowPanel> AllWindows = new();
        [FoldoutGroup("Settings")] public WindowSignature Signature;

        [FoldoutGroup("Dependencies")] [Required]
        public CanvasGroup CanvasGroup;

        [FoldoutGroup("Status")] public bool IsActive;

        private void OnEnable() {
            Hide();
            AllWindows.Add(this);
        }

        private void OnDisable() {
            AllWindows.Remove(this);
        }

        public static void OpenWindow(WindowSignature signature) {
            var window = AllWindows.FirstOrDefault(o => o.Signature == signature);
            if (window != null)
                window.Show();
        }

        public static void CloseWindow(WindowSignature signature) {
            var window = AllWindows.FirstOrDefault(o => o.Signature == signature);
            if (window != null)
                window.Hide();
        }

        public void Toggle() {
            if (IsActive)
                Hide();
            else Show();
        }

        [Button("Show Window")]
        public virtual void Show() {
            CanvasGroup.alpha = 1;
            CanvasGroup.blocksRaycasts = true;
            IsActive = true;
            AllWindows.Add(this);
        }

        [Button("Hide Window")]
        public virtual void Hide() {
            CanvasGroup.alpha = 0;
            CanvasGroup.blocksRaycasts = false;
            IsActive = false;
            AllWindows.Remove(this);
        }
    }
}