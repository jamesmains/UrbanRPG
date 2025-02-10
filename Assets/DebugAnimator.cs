using Sirenix.OdinInspector;
using UnityEngine;

public class DebugAnimator : MonoBehaviour {
    
    [SerializeField, FoldoutGroup("Debug")]
    private float RawAngle;
    
    [SerializeField, FoldoutGroup("Debug")]
    private float Angle;

    [SerializeField, FoldoutGroup("Debug")]
    private float RawAngleMod = 180;
    
    [SerializeField, FoldoutGroup("Debug")]
    private float RawIndex;
    
    [SerializeField, FoldoutGroup("Debug")]
    private float CircleAmount = 415;
    
    [SerializeField, FoldoutGroup("Debug")]
    private float Divisions = 8;
    
    [SerializeField, FoldoutGroup("Debug")]
    private Vector2 Direction;
    
    void Start()
    {
        SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>();
        foreach (var s in sr) {
            if (s.material.HasProperty("_BaseColor"))
            {
                s.material.SetColor("_BaseColor", s.color);
            }
        }
    }

    // Update is called once per frame
    void Update() {
        Direction.x = Input.GetAxis("Horizontal");
        Direction.y = Input.GetAxis("Vertical");
        var dir = -Direction;
        RawAngle = -(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) + RawAngleMod;
        Angle = (RawAngle + 360) % 360;
        Debug.Log($"Raw: {RawAngle}, Angle: {Angle}, Mod: {(RawAngle + 360) % 360}");
        RawIndex = (Angle / CircleAmount) * Divisions ;
        // RawIndex++;
        RawIndex %= Divisions;
    }
}
