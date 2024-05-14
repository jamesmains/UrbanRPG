using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse {
    [CreateAssetMenu(fileName = "Skill", menuName = "Signatures/Skill")]
    public class Skill : ScriptableObject {
        [FoldoutGroup("Details")] public string Name;
        [FoldoutGroup("Details")] public string Description;

        [FoldoutGroup("Details")] [PreviewField]
        public Sprite Icon;

        [FoldoutGroup("Data")] public int Level;
        [FoldoutGroup("Data")] public int ExpRequiredForLevelUp;
        [FoldoutGroup("Data")] public int CurrentExp;

        public void AddExperience(int incomingExperience) {
            if (Level >= 99) return;
            var remainingExp = incomingExperience - ExpRequiredForLevelUp;
            CurrentExp += incomingExperience;

            if (CurrentExp >= ExpRequiredForLevelUp)
                LevelUp();

            if (remainingExp > 0)
                AddExperience(remainingExp);

            GameEvents.OnGainExperience.Invoke();
            GameEvents.OnCreateSpriteStringPopup.Invoke(Icon, $"+{incomingExperience}");
        }

        private void LevelUp() {
            ExpRequiredForLevelUp += 100;
            CurrentExp = 0;
            Level++;
            GameEvents.OnLevelUp.Invoke();
            GameEvents.OnSendGenericMessage.Invoke($"{Name} raised to level {Level}!");
        }

        [Button]
        private void ResetSkill() {
            ExpRequiredForLevelUp = 100;
            CurrentExp = 0;
            Level = 0;
        }
    }
}