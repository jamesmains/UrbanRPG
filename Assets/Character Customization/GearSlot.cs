using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class GearSlot : MonoBehaviour
{
    [SerializeField] private string TEST;
    
    [SerializeField] private Gear gear;
    [FoldoutGroup("Animations Data")] [SerializeField] private int frameIndex;
    [FoldoutGroup("Animations Data")] [SerializeField] private SpriteRenderer _spriteRenderer;
    
    [SerializeField] private AnimationSheet anim;
    [SerializeField] private List<Sprite> sheet = new();
    [SerializeField] private int startOffset;
    [SerializeField] private int frameCount;

    private int currentDirection;
    
    private void Awake()
    {
        LoadSpriteSheet(gear.GearAnimationSheets[0].ID);
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void Tick(int targetDirection, bool flip)
    {
        currentDirection = targetDirection;
        _spriteRenderer.flipX = flip;
        Animate();
    }
    
    void SetSprite()
    {
        int index = startOffset+(currentDirection * frameCount) + frameIndex;
        print($"Sheet Length {sheet.Count}, index: {index}");
        _spriteRenderer.sprite = sheet[index];
    }
    
    void Animate()
    {
        frameIndex++;
        if (frameIndex > frameCount-1)
            frameIndex = 0;
        SetSprite();
    }

    [ButtonGroup()]
    public void LoadSpriteSheet(string sheetName)
    {
        sheet.Clear();
        sheet = Resources.LoadAll<Sprite>(sheetName).ToList();
    }
    
    public void LoadAnimation(int actionIndex)
    {
        if (actionIndex == -1 || gear.GearAnimationSheets.Length == 0) return;
        anim = gear.GearAnimationSheets[actionIndex];
        startOffset = anim.startIndex;
        frameCount  = anim.frameCount;
        if (frameCount <= 0)
            frameCount = 8;
    }
    
}