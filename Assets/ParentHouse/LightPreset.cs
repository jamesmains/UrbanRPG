using UnityEngine;

//Add this as a menu option in the "create" option when right-clicking (Create > Scriptables > Light Preset)
namespace ParentHouse {
    [System.Serializable]
    [CreateAssetMenu(fileName = "Light Preset", menuName = "Unsorted/Light Preset", order = 1)]
    public class LightPreset : ScriptableObject
    {
        public Gradient AmbientColour;
        public Gradient DirectionalColour;
        public Gradient FogColour;
    }
}