using System.Collections.Generic;
using ParentHouse.UI;
using ParentHouse.Utils;
using UnityEngine;

namespace ParentHouse {
    public class PlayerHud : WindowPanel
    {
        [SerializeField] private List<WindowPanel> HudWindows;

        private void Start()
        {
            GameEvents.ShowPlayerHud.Invoke();
        }

        public override void Show()
        {
            base.Show();
            foreach (var window in HudWindows)
            {
                window.Show();   
            }
        }

        public override void Hide()
        {
            base.Hide();
            foreach (var window in HudWindows)
            {
                window.Hide();   
            }
        }
    }
}
