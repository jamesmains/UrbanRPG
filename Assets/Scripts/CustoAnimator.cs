using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class CustoAnimator : MonoBehaviour
{
    [SerializeField] public Actor actor;
    [FoldoutGroup("Data")] public int currentDirection;
    [FoldoutGroup("Data")] public int RawDirection;
    [FoldoutGroup("Data")] [SerializeField] private float frameRate = 0.15f;
    [SerializeField] public List<GearOption> gearOptions;
    [SerializeField] public List<GearSlot> gearSlots;
    [SerializeField] private List<InputToDirectionDefiner> inputAdjustments = new List<InputToDirectionDefiner>();
    [SerializeField] private List<SpriteSheetOffsetDefiner> sheetOffsetAdjustments = new List<SpriteSheetOffsetDefiner>();

    private int actionIndex = -1;
    private bool flip;
    private float frameTimer;

    private void OnEnable()
    {
        ChangeDirection(new Vector2(0,-1),0);
        UpdateActor(actor);
        GameEvents.OnUpdateOutfit += UpdateActor;
    }

    private void OnDisable()
    {
        GameEvents.OnUpdateOutfit -= UpdateActor;
    }

    [Button]
    public void UpdateActor(Actor incomingActor)
    {
        if (incomingActor != actor) return;
        foreach (var slot in gearSlots)
        {
            slot.gear = null;
            foreach (var gearOption in actor.EquippedGear.Where(gearOption => slot.gearType == gearOption.type && gearOption.gear != null))
            {
                slot.gameObject.SetActive(true);
                slot.ChangeGear(gearOption.gear);

                if(gearOption.type == GearType.Hair)
                    slot.gameObject.GetComponent<SpriteRenderer>().color = actor.hairColor;
                slot.ResetAnimation();
                break;
            }
        }
        
        foreach (var gearSlot in gearSlots.Where(gearSlot => gearSlot.gear == null))
        {
            gearSlot.ClearAnimation();
            gearSlot.gameObject.SetActive(false);
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

    public void SyncFrames()
    {
        var i = from gearSlot in gearSlots
            orderby gearSlot.frameIndex descending select gearSlot;
        int index = i.First().frameIndex;
        foreach (var gearSlot in gearSlots)
        {
            gearSlot.frameIndex = index;
        }
    }
    
    public void ChangeDirection(Vector2 rawDirection, int action)
    {
        SetAction(action);
        
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
    
    public void SetDirection(Vector2 d)
    {
        ChangeDirection(d,actionIndex);
    }

    public void SetAction(int a)
    {
        if (actionIndex == a) return;
        actionIndex = a;
        foreach (var slot in gearSlots.Where(slot => slot.gameObject.activeSelf))
        {
            slot.LoadAnimation(actionIndex);
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



