using System;
using ParentHouse.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ParentHouse.UI {
    public class DisplayPopup : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI valueText;

        public void Setup(string value)
        {
            valueText.text = $"+{value}";
        }
    
        public void Setup(Quest quest)
        {
            valueText.text = $"{quest.QuestName}\n- Quest {GetQuestStateText(quest.CurrentState)} -";
        }

        public string GetQuestStateText(QuestState incomingState) => incomingState switch
        {
            QuestState.Completed => "Complete",
            QuestState.Started => "Started",
            QuestState.ReadyToComplete => "Ready To Turn In",
            _ => throw new ArgumentOutOfRangeException(nameof(incomingState), $"Not expected input")
        };
    
        public void Setup(Sprite icon, string value)
        {
            iconImage.sprite = icon;
            valueText.text = $"{value}";
        }

        public void Setup(Sprite icon)
        {
            iconImage.sprite = icon;
        }

        private void OnFinish()
        {
            Destroy(this.gameObject);
        }
    }
}
