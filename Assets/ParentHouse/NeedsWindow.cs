using System;
using System.Collections.Generic;
using ParentHouse.UI;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ParentHouse {
    public class NeedsWindow : WindowPanel {
        [SerializeField] private RectTransform needsDisplayContainer;
        [SerializeField] private GameObject needsDisplayListPrefab;
        [SerializeField] private List<Need> needs = new();
        public List<NeedDisplayObject> displayObjects = new();

        public void PopulateSkillsDisplays() {
            if (needsDisplayContainer.childCount > 0) return;
            foreach (var need in needs)
                SpawnListObject(Instantiate(needsDisplayListPrefab, needsDisplayContainer), need);
        }

        public void SpawnListObject(GameObject obj, Need need) {
            var newDisplayObject = new NeedDisplayObject();
            newDisplayObject.obj = obj;
            newDisplayObject.bar = obj.GetComponent<NeedDisplayBar>();
            newDisplayObject.bar.targetNeed = need;
            newDisplayObject.need = need;
            newDisplayObject.bar.Setup();
            displayObjects.Add(newDisplayObject);
        }

#if UNITY_EDITOR
        [Button]
        public void FindAssetsByType() {
            needs.Clear();
            var assets = AssetDatabase.FindAssets("t:Need");

            foreach (var t in assets) {
                var assetPath = AssetDatabase.GUIDToAssetPath(t);
                var asset = AssetDatabase.LoadAssetAtPath<Need>(assetPath);
                needs.Add(asset);
            }
        }

        [Button]
        private void ClearDisplays() {
            needsDisplayContainer.DestroyChildrenInEditor();
            displayObjects.Clear();
        }
#endif
    }

    [Serializable]
    public class NeedDisplayObject {
        public NeedDisplayBar bar;
        public Need need;
        public GameObject obj;
    }
}