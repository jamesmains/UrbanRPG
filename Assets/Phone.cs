using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phone : Window
{
    [SerializeField] private Window[] appWindows;
    [SerializeField] private GameObject appButtonPrefab;
    [SerializeField] private Transform appButtonContainer;

    public override void Show()
    {
        base.Show();
        if (appButtonContainer.childCount == 0)
        {
            PopulateAppButtons();
        }
        else
        {
            GotoHomeWindow();
        }
    }

    private void GotoHomeWindow()
    {
        appWindows[0].Show();
        for (int i = 1; i < appWindows.Length; i++)
        {
            appWindows[i].Hide();
        }
    }

    private void PopulateAppButtons()
    {
        for (int i = 0; i < appWindows.Length; i++)
        {
            var obj = Instantiate(appButtonPrefab, appButtonContainer);
            var button = obj.GetComponent<Button>();
            var image = obj.GetComponent<Image>();
            var animator = obj.GetComponent<Animator>();
            image.sprite = appWindows[i].windowIcon;
            int indexer = i;
            button.onClick.AddListener(delegate
            {
                animator.SetTrigger("Press");
                for (int j = 0; j < appWindows.Length; j++)
                {
                    if (j == indexer)
                    {
                        appWindows[j].Show();
                    }
                    else appWindows[j].Hide();
                }
            });
        }
        GotoHomeWindow();
    }
}
