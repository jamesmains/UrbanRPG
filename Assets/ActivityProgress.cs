using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActivityProgress : Window
{
    [SerializeField] private Image ActivityIcon;
    [SerializeField] private Image FillWheel;
    [SerializeField] private bool IsActive;
    [SerializeField] private float TotalTime;
    [SerializeField] private float Timer;
    [SerializeField] private UnityEvent ActivityCompleteAction = new();

    private void OnEnable()
    {
        GameEvents.OnStartActivity += StartActivity;
        GameEvents.OnPlayerMoved += CancelActivity;
    }

    private void OnDisable()
    {
        GameEvents.OnStartActivity -= StartActivity;
        GameEvents.OnPlayerMoved -= CancelActivity;
    }

    private void Update()
    {
        if (!IsActive) return;
        if (Timer < TotalTime)
        {
            Timer += Time.deltaTime * TimeManager.TimeMultiplier;
            FillWheel.fillAmount = Timer / TotalTime;
        }
        if(Timer >= TotalTime)
            FinishActivity();
    }

    public void StartActivity(float t, UnityEvent e, Sprite s)
    {
        ActivityTrigger.ActivityLock = true;
        IsActive = true;
        Timer = 0;
        TotalTime = t;
        ActivityCompleteAction = e;
        FillWheel.sprite = ActivityIcon.sprite = s;
        Show();
    }

    public void CancelActivity()
    {
        if (IsActive == false) return;
        IsActive = false;
        GameEvents.OnCancelActivity.Raise();
        Hide();
    }

    public void FinishActivity()
    {
        IsActive = false;
        ActivityCompleteAction?.Invoke();
        GameEvents.OnEndActivity.Raise();
        Hide();
    }
}
