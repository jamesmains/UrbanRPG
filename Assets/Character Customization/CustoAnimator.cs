using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustoAnimator : MonoBehaviour
{
    public int currentDirection;
    public int RawDirection;
    [SerializeField] private float frameRate = 30;
    [SerializeField] private GearSlot[] gearSlots;
    [SerializeField] private List<InputToDirectionDefiner> inputAdjustments = new List<InputToDirectionDefiner>();
    [SerializeField] private List<SpriteSheetOffsetDefiner> sheetOffsetAdjustments = new List<SpriteSheetOffsetDefiner>();

    public int actionIndex = -1;
    private bool flip;
    private float frameTimer;

    private void Update()
    {
        if (frameTimer < 0)
        {
            foreach (var slot in gearSlots)
            {
                slot.Tick(currentDirection,flip);
            }

            frameTimer = frameRate;
        }
        else
        {
            frameTimer -= Time.deltaTime;
        }
        
    }

    public void ChangeDirection(Vector2 rawDirection, int action)
    {
        if (actionIndex != action)
        {
            actionIndex = action;
            foreach (var slot in gearSlots)
            {
                if(!slot.gameObject.activeSelf) continue;
                slot.LoadAnimation(actionIndex);
            }
        }
        
        int newDirection = -1;
        foreach (var inputMirror in inputAdjustments)
        {
            Vector2 targetDirection = new Vector2(inputMirror.xRequirement, inputMirror.yRequirement);
            
            if (targetDirection == rawDirection)
            {
                RawDirection = newDirection = inputMirror.result;
                break;
            }
                
        }

        foreach (var sheetOffsetAdjustment in sheetOffsetAdjustments)
        {
            if (newDirection == sheetOffsetAdjustment.rawDirection)
            {
                newDirection = sheetOffsetAdjustment.mirrorTo;
                flip = sheetOffsetAdjustment.flipRenderer;
                break;
            }
        }
        if (newDirection != -1 && (int)currentDirection != newDirection)
        {
            currentDirection = newDirection;
            frameTimer = 0;
        }
    }
}
[Serializable]
public class SpriteSheetOffsetDefiner
{
    public int rawDirection;
    public int mirrorTo;
    public bool flipRenderer;
}

[Serializable]
public class InputToDirectionDefiner
{
    public float xRequirement;
    public float yRequirement;
    public int result;
}