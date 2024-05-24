using System.Collections.Generic;
using ParentHouse.UI;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ParentHouse {
    public class SkillsWindow : WindowDisplay {
        [SerializeField] private RectTransform skillsDisplayContainer;
        [SerializeField] private GameObject skillsDisplayListPrefab;
        [SerializeField] private List<Skill> skills = new();


        public override void Show() {
            base.Show();
            UpdateDisplays();
        }

        public override void Hide() {
            base.Hide();
            //foreach (var skillDisplay in skillDisplays) skillDisplay.CloseFoldout();
        }

        private void PopulateSkills() {
            foreach (var skill in skills) {
                // var SkillDisplay = Instantiate(skillsDisplayListPrefab, skillsDisplayContainer)
                //     .GetComponent<SkillDisplay>();
                // SkillDisplay.Setup(skill);
                // skillDisplays.Add(SkillDisplay);
            }
        }

        private void UpdateDisplays() {
            // foreach (var sklillDisplay in skillDisplays) {
            //     sklillDisplay.gameObject.SetActive(sklillDisplay.heldSkill.Level != 0);
            //     sklillDisplay.UpdateDisplay();
            // }
        }


#if UNITY_EDITOR
        [Button]
        public void FindAssetsByType() {
            skills.Clear();
            var assets = AssetDatabase.FindAssets("t:Skill");

            foreach (var t in assets) {
                var assetPath = AssetDatabase.GUIDToAssetPath(t);
                var asset = AssetDatabase.LoadAssetAtPath<Skill>(assetPath);
                skills.Add(asset);
            }
        }
#endif
    }
}