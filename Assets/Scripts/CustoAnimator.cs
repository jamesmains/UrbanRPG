using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CustoAnimator : MonoBehaviour
{
    [SerializeField] public Actor actor;
    [FoldoutGroup("Data")] public int currentDirection;
    [FoldoutGroup("Data")] public int RawDirection;
    [FoldoutGroup("Data")] [SerializeField] private float frameRate = 0.15f;
    [SerializeField] public GearSlot[] gearSlots;
    [SerializeField] private List<InputToDirectionDefiner> inputAdjustments = new List<InputToDirectionDefiner>();
    [SerializeField] private List<SpriteSheetOffsetDefiner> sheetOffsetAdjustments = new List<SpriteSheetOffsetDefiner>();

    private int actionIndex = -1;
    private bool flip;
    private float frameTimer;

    private void OnEnable()
    {
        if (actor != null)
        {
            gearSlots[0].gear = actor.GearBody;
            gearSlots[1].gear = actor.GearShoes;
            gearSlots[2].gear = actor.GearPants;
            gearSlots[3].gear = actor.GearMouth;
            gearSlots[4].gear = actor.GearEyes;
            gearSlots[5].gear = actor.GearShirt;
            gearSlots[6].gear = actor.GearHair;
            gearSlots[6].gameObject.GetComponent<SpriteRenderer>().color = actor.hairColor;
            gearSlots[7].gear = actor.GearAccessory;
        }
        
        foreach (var gearSlot in gearSlots)
        {
            if (gearSlot.gear == null)
            {
                gearSlot.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateActor(Actor incomingActor = null)
    {
        if (actor != null)
        {
            gearSlots[0].gear = actor.GearBody;
            gearSlots[1].gear = actor.GearShoes;
            gearSlots[2].gear = actor.GearPants;
            gearSlots[3].gear = actor.GearMouth;
            gearSlots[4].gear = actor.GearEyes;
            gearSlots[5].gear = actor.GearShirt;
            gearSlots[6].gear = actor.GearHair;
            gearSlots[6].gameObject.GetComponent<SpriteRenderer>().color = actor.hairColor;
            gearSlots[7].gear = actor.GearAccessory;
        }
        
        foreach (var gearSlot in gearSlots)
        {
            if (gearSlot.gear == null)
            {
                gearSlot.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (frameTimer < 0)
        {
            foreach (var slot in gearSlots)
            {
                if(!slot.gameObject.activeSelf) continue;
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