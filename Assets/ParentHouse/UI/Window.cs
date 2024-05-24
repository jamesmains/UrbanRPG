using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse.UI {
    [CreateAssetMenu(fileName = "Window", menuName = "UI/Window")]
    public class Window : ScriptableObject {
        public static readonly List<WindowDisplay> AllWindowDisplays = new();

        [Button("Show Window")]
        public void Open() {
            OpenWindowDisplay(this);
        }

        [Button("Hide Window")]
        public void Close() {
            CloseWindowDisplay(this);
        }
        
        private void OpenWindowDisplay(Window signature) {
            var window = AllWindowDisplays.FirstOrDefault(o => o.Signature == signature);
            if (window != null)
                window.Show();
        }

        private void CloseWindowDisplay(Window signature) {
            var window = AllWindowDisplays.FirstOrDefault(o => o.Signature == signature);
            if (window != null)
                window.Hide();
        }
    }
}