using System;
using UnityEngine;

namespace ParentHouse {
    [CreateAssetMenu(fileName = "Level Transition", menuName = "Signatures/Level Transition")]
    public class SceneTransition : ScriptableObject
    {
        public String TargetScene;
        public Vector3 SpawnLocation;
    }
}