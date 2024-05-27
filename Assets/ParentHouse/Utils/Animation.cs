using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse.Utils {
    public enum AnimationName {
        Idle = 0,
        Walk = 10,
        Run = 20
    }
    
    [CreateAssetMenu(fileName = "Animation", menuName = "Animations/Animation")]
    public class Animation : ScriptableObject {
        private List<AnimationSetting> AnimationSettings = new();

        public AnimationSetting GetAnimationSprites(AnimationName name) {
            return AnimationSettings.FirstOrDefault(o => o.AnimationName == name);
        }
        
    }

    [Serializable]
    public class AnimationSetting {
        public AnimationName AnimationName;
        private List<Sprite> AnimationSprites;
        public int Length;
    }
}