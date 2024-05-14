using ParentHouse.UI;
using ParentHouse.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ParentHouse {
    public class ActivityProgress : WindowPanel {
        [SerializeField] private Image ActivityIcon;
        [SerializeField] private Image FillWheel;
        [SerializeField] private bool IsActive;
        [SerializeField] private float TotalTime;
        [SerializeField] private float Timer;
        [SerializeField] private UnityEvent ActivityCompleteAction = new();

        private void Update() {
            if (!IsActive) return;
            if (Timer < TotalTime) {
                Timer += Time.deltaTime * TimeManager.TimeMultiplier;
                FillWheel.fillAmount = Timer / TotalTime;
            }

            if (Timer >= TotalTime)
                FinishActivity();
        }

        public void StartActivity(float t, UnityEvent e, Sprite s) {
            ActivityTrigger.ActivityLock = true;
            IsActive = true;
            Timer = 0;
            TotalTime = t;
            ActivityCompleteAction = e;
            FillWheel.sprite = ActivityIcon.sprite = s;
            Show();
        }

        public void CancelActivity() {
            if (IsActive == false) return;
            IsActive = false;
            GameEvents.OnCancelActivity.Invoke();
            Hide();
        }

        public void FinishActivity() {
            IsActive = false;
            ActivityCompleteAction?.Invoke();
            GameEvents.OnEndActivity.Invoke();
            Hide();
        }
    }
}