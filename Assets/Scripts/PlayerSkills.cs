using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerSkills : MonoBehaviour {
    [SerializeField] [FoldoutGroup("Dependencies")]
    private RectTransform SkillsContent;

    [SerializeField] [FoldoutGroup("Settings")]
    private GameObject SkillInfoDisplayObject;
}
