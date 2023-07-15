using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class GearSlot : MonoBehaviour
{
    [SerializeField] public Gear gear;
    [FoldoutGroup("Display")] [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private List<Sprite> sheet = new();
    
    [FoldoutGroup("Data")] [SerializeField] private int frameIndex;
    [FoldoutGroup("Data")] [SerializeField] private int startIndex;
    [FoldoutGroup("Data")] [SerializeField] private int frameCount;
    [FoldoutGroup("Data")] [SerializeField] private AnimationSheet[] animationData;

    private int currentDirection;
    private Gear lastKnownGear;
    
    private void Start()
    {
        LoadSpriteSheet(gear.spriteSheetId);
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void Tick(int targetDirection, bool flip)
    {
        if (lastKnownGear != gear)
        {
            lastKnownGear = gear;
            LoadSpriteSheet(gear.spriteSheetId);
        }
        currentDirection = targetDirection;
        _spriteRenderer.flipX = flip;
        Animate();
    }
    
    void SetSprite()
    {
        int index = startIndex+(currentDirection * frameCount) + frameIndex;
        if(UrbanDebugger.DebugLevel>=1)
        {
            Debug.Log($"Sheet Length {sheet.Count}, index: {index} (GearSlot.cs)");
        }
        _spriteRenderer.sprite = sheet[index];
    }
    
    void Animate()
    {
        frameIndex++;
        if (frameIndex > frameCount-1)
            frameIndex = 0;
        SetSprite();
    }
    
    public void LoadSpriteSheet(string sheetName)
    {
        sheet.Clear();
        sheet = Resources.LoadAll<Sprite>(sheetName).ToList();
    }
    
    public void LoadAnimation(int actionIndex)
    {
        if (actionIndex == -1) return;
        
        startIndex = animationData[actionIndex].startIndex;
        frameCount  = animationData[actionIndex].frameCount;
        if (frameCount <= 0)
            frameCount = 8;
    }
    
}