using ParentHouse.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace ParentHouse {
    public class SkillDisplayBar : DisplayBar
    {
        [FoldoutGroup("Display")] [SerializeField] private TextMeshProUGUI levelText;
        public Skill targetSkill;

        private void Update()
        {
            UpdateSkillDisplayBar();
        }

        public void Setup()
        {
            if(nameText!=null) nameText.text = targetSkill.Name;
            if(descriptionText!=null) descriptionText.text = targetSkill.Description;
            if (iconImage != null) iconImage.sprite = targetSkill.Icon;
            UpdateSkillDisplayBar();
        }

        public void UpdateSkillDisplayBar()
        {
            if (levelText != null) levelText.text = $"Lvl {targetSkill.Level.ToString()}";
            UpdateBar((float)targetSkill.CurrentExp / (float)targetSkill.ExpRequiredForLevelUp);
        }
    }
}
