using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ParentHouse.UI {
    public class WindowPanelGroup : WindowPanel
    {
        [SerializeField] private WindowPanel[] windows;
        [SerializeField] private WindowPanel homeWindowPanel;
        [SerializeField] private TextMeshProUGUI windowNameDisplay;
        [SerializeField] private GameObject selectionButtonPrefab;
        [SerializeField] private Transform selectionButtonContainer;
        [SerializeField] private bool toggleWindowsWhenSelected;

        public WindowPanelGroup(WindowPanel[] w, int homeWindowIndex = 0)
        {
            windows = w;
            homeWindowPanel = windows[homeWindowIndex];
        }
    
        public override void Show()
        {
            base.Show();
            if (selectionButtonContainer.childCount == 0)
            {
                PopulateSelectionButtons();
            }
            else
            {
                GotoHomeWindow();
            }
        }

        public override void Hide()
        {
            base.Hide();
            foreach (var t in windows)
            {
                t.Hide();
            }
        }

        public void GotoHomeWindow()
        {
            foreach (var t in windows)
            {
                t.Hide();
            }
            if(homeWindowPanel != null) homeWindowPanel.Show();
        }

        private void PopulateSelectionButtons()
        {
            for (int i = 0; i < windows.Length; i++)
            {
                var cachedWindow = windows[i];
                // if(cachedWindow.excludeInLists) continue; // What is this for?
                var obj = Instantiate(selectionButtonPrefab, selectionButtonContainer);
                var button = obj.GetComponentInChildren<Button>();
                var image = obj.transform.GetChild(0).GetComponent<Image>();
                int indexer = i;

                if (windowNameDisplay != null)
                {
                    var effects = button.gameObject.AddComponent<MouseInteractionEffects>();
                    //effects.Effects.Add(new ChangeTextEffect(windowNameDisplay,cachedWindow.WindowName));
                }
            
                button.onClick.AddListener(delegate
                {
                    for (int j = 0; j < windows.Length; j++)
                    {
                        if (j == indexer)
                        {
                            if(toggleWindowsWhenSelected) windows[j].Toggle();
                            else windows[j].Show();
                        }
                        else windows[j].Hide();
                    }
                });
            }
            GotoHomeWindow();
        }
    }
}
