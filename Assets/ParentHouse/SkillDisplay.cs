using ParentHouse.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ParentHouse {
    public class SkillDisplay : FoldoutDisplay {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI skillDescriptionText;
        [SerializeField] private Image skillIcon;

        [SerializeField] [FoldoutGroup("Debug")] [ReadOnly]
        public Skill heldSkill;

        private void OnEnable() {
        }

        private void OnDisable() {
        }

        public void Setup(Skill skill) {
            heldSkill = skill;
            nameText.text = heldSkill.Name;
            skillIcon.sprite = heldSkill.Icon;
            closedSize = new Vector2(displayRect.rect.width, 50);
            UpdateDisplay();
        }

        public void UpdateDisplay() {
            if (isOpen)
                skillDescriptionText.text =
                    $"Level {heldSkill.Level} | {heldSkill.CurrentExp}/{heldSkill.ExpRequiredForLevelUp}\n{heldSkill.Description}";
        }

        public override void OpenFoldout() {
            skillDescriptionText.text =
                $"Level {heldSkill.Level} | {heldSkill.CurrentExp}/{heldSkill.ExpRequiredForLevelUp}\n{heldSkill.Description}";
            base.OpenFoldout();
        }

        public override void CloseFoldout() {
            skillDescriptionText.text = "";
            base.CloseFoldout();
        }
    }
}