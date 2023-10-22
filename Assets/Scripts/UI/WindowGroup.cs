using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowGroup : Window
{
    [SerializeField] private Window[] windows;
    [SerializeField] private Window homeWindow;
    [SerializeField] private TextMeshProUGUI windowNameDisplay;
    [SerializeField] private GameObject selectionButtonPrefab;
    [SerializeField] private Transform selectionButtonContainer;
    [SerializeField] private bool toggleWindowsWhenSelected;

    public WindowGroup(Window[] w, int homeWindowIndex = 0)
    {
        windows = w;
        homeWindow = windows[homeWindowIndex];
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
        if(homeWindow != null) homeWindow.Show();
    }

    private void PopulateSelectionButtons()
    {
        for (int i = 0; i < windows.Length; i++)
        {
            var cachedWindow = windows[i];
            if(cachedWindow.excludeInLists) continue;
            var obj = Instantiate(selectionButtonPrefab, selectionButtonContainer);
            var button = obj.GetComponentInChildren<Button>();
            var image = obj.transform.GetChild(0).GetComponent<Image>();
            image.sprite = cachedWindow.windowIcon;
            int indexer = i;

            if (windowNameDisplay != null)
            {
                var effects = button.gameObject.AddComponent<MouseInteractionEffects>();
                effects.Effects.Add(new ChangeTextEffect(windowNameDisplay,cachedWindow.windowName));
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
