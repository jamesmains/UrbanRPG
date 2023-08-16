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
        GameEvents.ShowPlayerHud += Show;
        GameEvents.HidePlayerHud += Hide;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameEvents.ShowPlayerHud -= Show;
        GameEvents.HidePlayerHud -= Hide;
    }

    private void Start()
    {
        GameEvents.ShowPlayerHud.Raise();
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
