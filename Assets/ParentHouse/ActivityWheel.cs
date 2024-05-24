using System.Collections.Generic;
using ParentHouse.UI;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace ParentHouse {
    public class ActivityWheel : WindowDisplay {
        // Things we may want
        // Get world space approx to display wheel close to where activity is
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

        [SerializeField] [FoldoutGroup("Data")]
        private float scrollSpeed;

        private ActivityTrigger activityTrigger;
        private float FacingAngle;
        private bool needsToBeNormalized;
        private List<ActivityWheelAction> WheelActions = new();
        private List<GameObject> WheelDisplayObjectPool = new();

        private void Update() {
            if (!IsActive) return;
            AnimateWheelDisplays();
        }

        protected void OnEnable() {
            for (var i = 0; i < 20; i++) {
                WheelDisplayObjectPool.Add(Instantiate(ActivityWheelObject, ActivityListContainer));
                WheelDisplayObjectPool[i].gameObject.name = $"Object {i}";
            }

            ActivityListContainer.SetChildrenActiveState(false);
        }

        private void SetCurrentActivity(ActivityTrigger incomingActivityTrigger) {
            if (incomingActivityTrigger.Activities.Count == 0) {
                Hide();
                return;
            }

            activityTrigger = incomingActivityTrigger;
            Show();
        }

        public override void Show() {
            base.Show();
            ClearWheel();
            GenerateActivityWheelDisplays();
            if (WheelActions.Count == 0) return;
            WheelActions.Reverse();
            if (WheelActions.Count > 1) {
                AngleOffset = WheelActions[^2].storedAngle;
            }
            else {
                TargetAngle = 0;
                WheelActions[0].WheelActionDisplay.SetHighlightState(true);
            }

            CheckScrollInput(1);
            FacingAngle = TargetAngle;
        }

        public override void Hide() {
            base.Hide();
        }

        private void ClearWheel() {
            FacingAngle = 0;
            TargetAngle = 0;
            selectIndex = 0;
            ActivityListContainer.SetChildrenActiveState(false);
            WheelActions.Clear();
        }

        private void InvokeSelectedActivityAction() {
            if (!IsActive || Global.PlayerLock > 0 || WheelActions.Count == 0) return;
            var offsetIndex = selectIndex + 1;
            offsetIndex = offsetIndex >= WheelActions.Count ? 0 :
                offsetIndex < 0 ? WheelActions.Count - 1 : offsetIndex;
            var t = WheelActions[offsetIndex].WheelActionDisplay.activityAction.signature.ActionTime;
            var a = WheelActions[offsetIndex].WheelAction;
            var s = WheelActions[offsetIndex].WheelActionDisplay.activityAction.signature.ActivityIcon;
            if (t == 0)
                a.Invoke();
            else GameEvents.OnStartActivity.Invoke(t, a, s);
        }

        private void CheckScrollInput(float input) {
            if (!IsActive || input == 0 || WheelActions.Count <= 1) return;
            if (needsToBeNormalized) {
                FacingAngle = TargetAngle = WheelActions[selectIndex].storedAngle;
                needsToBeNormalized = false;
            }

            input = Mathf.Clamp(input, -1, 1);
            selectIndex += (int) input;

            if (selectIndex >= WheelActions.Count) {
                TargetAngle = -AngleOffset;
                needsToBeNormalized = true;
            }
            else if (selectIndex < 0) {
                TargetAngle = WheelActions[0].storedAngle + AngleOffset;
                needsToBeNormalized = true;
            }
            else {
                TargetAngle = WheelActions[selectIndex].storedAngle;
            }

            selectIndex = selectIndex >= WheelActions.Count ? 0 :
                selectIndex < 0 ? WheelActions.Count - 1 : selectIndex;
            var offsetIndex = selectIndex + 1;
            offsetIndex = offsetIndex >= WheelActions.Count ? 0 :
                offsetIndex < 0 ? WheelActions.Count - 1 : offsetIndex;
            foreach (var activityWheelAction in WheelActions)
                activityWheelAction.WheelActionDisplay.SetHighlightState(
                    activityWheelAction.WheelActionDisplay == WheelActions[offsetIndex].WheelActionDisplay);
        }

        private void AnimateWheelDisplays() {
            for (var i = 0; i < WheelActions.Count; i++) {
                var rect = WheelActions[i].WheelActionDisplay.GetComponent<RectTransform>();

                var angle = i * Mathf.PI * 2f / WheelActions.Count + FacingAngle;
                if (!WheelActions[i].hasAngle) {
                    WheelActions[i].hasAngle = true;
                    WheelActions[i].storedAngle = angle;
                }

                var y = Mathf.Cos(angle) * Radius * VertialSquish;
                var x = Mathf.Sin(angle) * Radius;
                rect.anchoredPosition = new Vector2(x, y);

                var sizeMod = Mathf.Clamp(Mathf.InverseLerp(HeightRange.x, HeightRange.y, y), SizeBounds.x,
                    SizeBounds.y);
                var size = ControlledSize * sizeMod * SizeMultiplier;
                rect.sizeDelta = size;

                var c = rect.GetComponent<Canvas>();
                c.overrideSorting = true;
                c.sortingOrder = (int) size.y;

                // print($"X: {x}, Y: {y}, Rounded Y: {Mathf.RoundToInt(y)}," +
                //       $" Angle: {angle}, Iteration: {i}, SizeMod: {sizeMod}," +
                //       $" Actions Count: {WheelActions.Count}");
            }

            if (WheelActions.Count <= 1) return;
            FacingAngle = Mathf.Lerp(FacingAngle, TargetAngle, SpinSpeed * Time.deltaTime);
        }

        private void GenerateActivityWheelDisplays() {
            if (activityTrigger == null) return;
            for (var i = 0; i < activityTrigger.Activities.Count; i++) {
                var t = activityTrigger.Activities[i];
                SetupWheelListObject(t, WheelDisplayObjectPool[i]);
            }

            selectIndex = selectIndex >= WheelActions.Count ? WheelActions.Count - 1 :
                selectIndex < 0 ? 0 : selectIndex;
            AnimateWheelDisplays();
        }


        private bool SetupWheelListObject(ActivityAction action, GameObject displayObject, bool useIcon = true) {
            if (!action.signature.IsConditionMet() || !action.IsConditionMet()) return false;
            displayObject.SetActive(true);
            var display = displayObject.GetComponent<ActivityActionDisplay>();
            var WheelAction = new ActivityWheelAction(display, delegate {
                action.worldActions.Invoke();
                action.InvokeSpecialActions();
                action.signature.InvokeActivity();
            });
            WheelActions.Add(WheelAction);
            display.AssignAction(action, useIcon: useIcon);
            return true;
        }
    }

    public class ActivityWheelAction {
        public bool hasAngle;
        public float storedAngle;
        public UnityEvent WheelAction = new();
        public ActivityActionDisplay WheelActionDisplay;

        public ActivityWheelAction(ActivityActionDisplay wheelActionDisplay, UnityAction wheelAction) {
            WheelActionDisplay = wheelActionDisplay;
            WheelAction.AddListener(wheelAction);
        }
    }
}