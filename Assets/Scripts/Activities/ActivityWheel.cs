using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ActivityWheel : Window
{
    // Things we may want
    // Get world space aprox to display wheel close to where activity is
    public GameObject ActivityWheelObject;
    public Transform ActivityListContainer;
    public float Radius;
    public float VertialSquish;
    public float SizeMultiplier;
    public int selectIndex;
    public float TargetAngle;
    public float AngleOffset;
    public float SpinSpeed;
    public Vector2 HeightRange;
    public Vector2 SizeBounds;
    public Vector2 ControlledSize;

    [SerializeField] [FoldoutGroup("Data")] private float scrollSpeed;
    private ActivityTrigger activityTrigger;
    private List<ActivityWheelAction> WheelActions = new();
    private List<GameObject> WheelDisplayObjectPool = new();
    private float FacingAngle;
    private bool needsToBeNormalized;

    protected override void OnEnable()
    {
        base.OnEnable();
        GameEvents.OnOpenActivityWheel += SetCurrentActivity;
        GameEvents.OnCloseActivityWheel += Hide;
        GameEvents.OnInteractButtonDown += InvokeSelectedActivityAction;
        GameEvents.OnMouseScroll += CheckScrollInput;

        for (int i = 0; i < 20; i++)
        {
            WheelDisplayObjectPool.Add(Instantiate(ActivityWheelObject, ActivityListContainer));
            WheelDisplayObjectPool[i].gameObject.name = $"Object {i}";
        }
        ActivityListContainer.SetChildrenActiveState(false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameEvents.OnOpenActivityWheel -= SetCurrentActivity;
        GameEvents.OnCloseActivityWheel -= Hide;
        GameEvents.OnInteractButtonDown -= InvokeSelectedActivityAction;
        GameEvents.OnMouseScroll -= CheckScrollInput;
    }
    
    private void SetCurrentActivity(ActivityTrigger incomingActivityTrigger)
    {
        if (incomingActivityTrigger.Activities.Count == 0)
        {
            Hide();
            return;
        }
        activityTrigger = incomingActivityTrigger;
        Show();
    }

    public override void Show()
    {
        base.Show();
        ClearWheel();
        GenerateActivityWheelDisplays();
        if (WheelActions.Count == 0) return;
        WheelActions.Reverse();
        if (WheelActions.Count > 1)
            AngleOffset = WheelActions[^2].storedAngle;
        else
        {
            TargetAngle = 0;
            WheelActions[0].WheelActionDisplay.SetHighlightState(true);
        }
        CheckScrollInput(1);
        FacingAngle = TargetAngle;
    }

    public override void Hide()
    {
        base.Hide();
    }

    private void ClearWheel()
    {
        FacingAngle = 0;
        TargetAngle = 0;
        selectIndex = 0;
        ActivityListContainer.SetChildrenActiveState(false);
        WheelActions.Clear();
    }

    private void InvokeSelectedActivityAction()
    {
        if (!isActive || Global.PlayerLock > 0 || WheelActions.Count == 0) return;
        int offsetIndex = selectIndex + 1;
        offsetIndex = offsetIndex >= WheelActions.Count ? 0 : offsetIndex < 0 ? WheelActions.Count -1 : offsetIndex;
        float t = WheelActions[offsetIndex].WheelActionDisplay.activityAction.signature.ActionTime;
        var a = WheelActions[offsetIndex].WheelAction;
        var s = WheelActions[offsetIndex].WheelActionDisplay.activityAction.signature.ActivityIcon;
        if(t == 0)
            a.Invoke();
        else GameEvents.OnStartActivity.Raise(t,a,s);
    }

    private void Update()
    {
        if (!isActive) return;
        AnimateWheelDisplays();
    }

    private void CheckScrollInput(float input)
    {
        if (!isActive || input == 0 || WheelActions.Count <= 1) return;
        if (needsToBeNormalized)
        {
            FacingAngle = TargetAngle = WheelActions[selectIndex].storedAngle;
            needsToBeNormalized = false;
        }
        input = Mathf.Clamp(input, -1, 1);
        selectIndex += (int)input;
        
        if (selectIndex >= WheelActions.Count)
        {
            TargetAngle = -AngleOffset;
            needsToBeNormalized = true;
        }
        else if (selectIndex < 0)
        {
            TargetAngle = WheelActions[0].storedAngle + AngleOffset;
            needsToBeNormalized = true;
        }
        else TargetAngle = WheelActions[selectIndex].storedAngle;
        selectIndex = selectIndex >= WheelActions.Count ? 0 : selectIndex < 0 ? WheelActions.Count -1 : selectIndex;
        int offsetIndex = selectIndex + 1;
        offsetIndex = offsetIndex >= WheelActions.Count ? 0 : offsetIndex < 0 ? WheelActions.Count -1 : offsetIndex;
        foreach (var activityWheelAction in WheelActions)
        {
            activityWheelAction.WheelActionDisplay.SetHighlightState(
                activityWheelAction.WheelActionDisplay == WheelActions[offsetIndex].WheelActionDisplay);
        }
    }

    private void AnimateWheelDisplays()
    {
        for (int i = 0; i < WheelActions.Count; i++)
        {
            var rect = WheelActions[i].WheelActionDisplay.GetComponent<RectTransform>();
            
            float angle = (i * Mathf.PI * 2f / WheelActions.Count) + FacingAngle;
            if (!WheelActions[i].hasAngle)
            {
                WheelActions[i].hasAngle = true;
                WheelActions[i].storedAngle = angle;
            }
            float y = (Mathf.Cos(angle)*Radius) * VertialSquish;
            float x = Mathf.Sin(angle)*Radius;
            rect.anchoredPosition = new Vector2(x, y);

            float sizeMod = Mathf.Clamp(Mathf.InverseLerp(HeightRange.x, HeightRange.y, y),SizeBounds.x,SizeBounds.y);
            var size = ControlledSize * sizeMod * SizeMultiplier;
            rect.sizeDelta = size;

            var c = rect.GetComponent<Canvas>();
            c.overrideSorting = true;
            c.sortingOrder = (int)size.y;
            
            // print($"X: {x}, Y: {y}, Rounded Y: {Mathf.RoundToInt(y)}," +
            //       $" Angle: {angle}, Iteration: {i}, SizeMod: {sizeMod}," +
            //       $" Actions Count: {WheelActions.Count}");
        }
        
        if (WheelActions.Count <= 1) return;
        FacingAngle = Mathf.Lerp(FacingAngle,TargetAngle,SpinSpeed*Time.deltaTime);
    }
    
    private void GenerateActivityWheelDisplays()
    {
        if (activityTrigger == null) return;
        for (var i = 0; i < activityTrigger.Activities.Count; i++)
        {
            var t = activityTrigger.Activities[i];
            SetupWheelListObject(t,WheelDisplayObjectPool[i]);
        }

        selectIndex = selectIndex >= WheelActions.Count ? WheelActions.Count -1 : selectIndex < 0 ? 0 : selectIndex;
        AnimateWheelDisplays();
    }
    

    private bool SetupWheelListObject(ActivityAction action,GameObject displayObject, bool useIcon = true)
    {
        if (!action.signature.IsConditionMet() || !action.IsConditionMet()) return false;
        displayObject.SetActive(true);
        var display = displayObject.GetComponent<ActivityActionDisplay>();
        var WheelAction = new ActivityWheelAction(display, (delegate
        {
            action.worldActions.Invoke();
            action.InvokeSpecialActions();
            action.signature.InvokeActivity();
        }));
        WheelActions.Add(WheelAction);
        display.AssignAction(action, useIcon:useIcon);
        return true;
    }
}

public class ActivityWheelAction
{
    public ActivityWheelAction(ActivityActionDisplay wheelActionDisplay, UnityAction wheelAction)
    {
        WheelActionDisplay = wheelActionDisplay;
        WheelAction.AddListener(wheelAction);
    }
    public ActivityActionDisplay WheelActionDisplay;
    public UnityEvent WheelAction = new();
    public float storedAngle;
    public bool hasAngle = false;
}

