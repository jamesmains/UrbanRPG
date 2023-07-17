using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGroup : Window
{
    [SerializeField, Tooltip("Index zero is always the home window")] private Window[] windows;
    [SerializeField] private GameObject selectionButtonPrefab;
    [SerializeField] private Transform selectionButtonContainer;
    [SerializeField] private bool toggleWindowsWhenSelected;

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

    private void GotoHomeWindow()
    {
        windows[0].Show();
        for (int i = 1; i < windows.Length; i++)
        {
            windows[i].Hide();
        }
    }

    private void PopulateSelectionButtons()
    {
        for (int i = 0; i < windows.Length; i++)
        {
            var obj = Instantiate(selectionButtonPrefab, selectionButtonContainer);
            var button = obj.GetComponent<Button>();
            var image = obj.GetComponent<Image>();
            var animator = obj.GetComponent<Animator>();
            image.sprite = windows[i].windowIcon;
            int indexer = i;
            button.onClick.AddListener(delegate
            {
                animator.SetTrigger("Press");
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
