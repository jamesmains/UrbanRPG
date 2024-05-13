using ParentHouse.UI;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ParentHouse {
    public class CalendarDayDisplay : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        [SerializeField] private Image frameIcon;
        [SerializeField] private TextMeshProUGUI dayText;
        [SerializeField] private Image eventIndicator;
        [SerializeField] private Color highlightColor;
        [SerializeField] private Color currentDayColor;
        [SerializeField] private Color normalColor;

        private UnityAction onInteract;
        [HideInInspector]public WindowPanel parentWindow;
        private bool isMouseOver;
        private bool isCurrentDay;
    
        int clicked = 0;
        float clicktime = 0;
        float clickdelay = 0.5f;

        public static CalendarDayDisplay LastInteractedDayDisplay;

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }
    
        public void Setup(bool currentDayHighlight, int day, bool hasEvents, WindowPanel parent)
        {
            isCurrentDay = currentDayHighlight;
            frameIcon.color = currentDayHighlight ? currentDayColor : normalColor;
            dayText.text = (day+1).ToString();
            parentWindow = parent;
            eventIndicator.enabled = hasEvents;
        }

        public void ToggleTemporaryHighlight(bool state)
        {
            if (!parentWindow.IsActive) return;
            if (LastInteractedDayDisplay != null && LastInteractedDayDisplay != this)
            {
                LastInteractedDayDisplay.ToggleTemporaryHighlight(false);
            }

            LastInteractedDayDisplay = this;
            frameIcon.color = state ? highlightColor : isCurrentDay ? currentDayColor : normalColor;
        }

        public void Interact()
        {
            if (!parentWindow.IsActive) return;
            if (isMouseOver)
            {
                // if (IsDoubleClick())
                // {
                //     print("Double click");
                //     GameEvents.OpenCalendarDayDetails.Raise();
                // }
                // else
                onInteract.Invoke();
            }
        }

        private bool IsDoubleClick()
        {
            clicked++;
            if (clicked == 1) clicktime = UnityEngine.Time.time;
 
            if (clicked > 1 && UnityEngine.Time.time - clicktime < clickdelay)
            {
                clicked = 0;
                clicktime = 0;
                return true;

            }
            else if (clicked > 2 || UnityEngine.Time.time - clicktime > 1) clicked = 0;

            return false;
        }

        public void AssignInteract(UnityAction interactActions)
        {
            onInteract = interactActions;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isMouseOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isMouseOver = false;
        }
    }
}
