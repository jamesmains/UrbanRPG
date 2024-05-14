using ParentHouse.Utils;
using UnityEngine;

namespace ParentHouse {
    public class SceneChangeCollisionTrigger : CollisionTrigger {
        [SerializeField] private SceneTransition TargetScene;

        private void OnEnable() {
            onEnter.AddListener(delegate { GameEvents.OnLoadNextScene.Invoke(TargetScene); });
        }

        private void OnDisable() {
            onEnter.RemoveListener(delegate { GameEvents.OnLoadNextScene.Invoke(TargetScene); });
        }
    }
}