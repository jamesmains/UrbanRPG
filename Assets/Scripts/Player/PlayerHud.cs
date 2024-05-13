using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHud : Window
{
    [SerializeField] private List<Window> HudWindows;
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

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
