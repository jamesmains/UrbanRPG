using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ParentHouse {
    public class ActivityActionDisplay : MonoBehaviour
    {
        public ActivityAction activityAction;
        [FoldoutGroup("Display")][SerializeField] private Image actionIconDisplay;
        [FoldoutGroup("Display")][SerializeField] private TextMeshProUGUI actionNameText;
        [FoldoutGroup("Data")][SerializeField] private Color highlightColor;
        [FoldoutGroup("Data")][SerializeField] private Color normalColor;
    
        public static ActivityActionDisplay Current;

        public void AssignAction(ActivityAction incomingAction, string extraText = "", bool useIcon = true)
        {
            activityAction = incomingAction;
            actionNameText.text = $"{activityAction.signature.ActivityName}{extraText}";
        
            if (useIcon)
                actionIconDisplay.sprite = activityAction.signature.ActivityIcon;
            else actionIconDisplay.enabled = false;
        }

        public void SetHighlightState(bool state)
        {
            actionNameText.enabled = state;
        }
    }
}
