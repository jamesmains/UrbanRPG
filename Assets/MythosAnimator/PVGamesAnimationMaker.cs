using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
[Serializable]
public class PVGamesAnimationMakerLayer
{
   public Sprite[] spriteSheet;
   public AnimationSheet animation;
   public Image preview;

   public int lastKnownFrame;

   public bool toBeDeleted = false;
   public bool saved;

   public PVGamesAnimationMakerLayer(AnimationSheet incomingAnimation)
   {
       animation = incomingAnimation;
       Load();
       lastKnownFrame = animation.startIndex;
   }
   
   public void Load()
   {
       // spriteSheet = Resources.LoadAll<Sprite>("Mythos/"+animation.ID);
   }

   public int[] GetFrameData()
   {
       int[] data = new int[5];
       
       data[0] = lastKnownFrame;        // Current Frame
       data[1] = spriteSheet.Length-1;  // Total frames
       data[2] = animation.startIndex;  
       data[3] = animation.stopIndex;
       data[4] = animation.frameCount;

       return data;
   }
}

public class PVGamesAnimationMaker : MonoBehaviour
{
    #region Variables-------------------------------------------------------------------------------------
    [Header("Animation")]
    [SerializeField] private List<PVGamesAnimationMakerLayer> layers = new List<PVGamesAnimationMakerLayer>();
    public                      AnimationSheet incomingAnimation ;
    [SerializeField] private    AnimationSheet currentAnimation  ;
    
    [Header("Displays")] 
    [SerializeField] private TextMeshProUGUI        debugOutput             ;
    [SerializeField] private Transform              demoAnimatorHolder      ;
    [SerializeField] private GameObject             demoAnimatorPrefab      ;
    [SerializeField] private Transform              previewHolder           ;
    [SerializeField] private GameObject             previewItemPrefab       ;
    [SerializeField] private PVGamesAnimationHelper layerDisplay            ;
    [SerializeField] private TextMeshProUGUI        currentAnimationDisplay ;

    [Header("Controls")] 
    [SerializeField] private Slider animationBoundsScrub        ;
    [SerializeField] private Slider animationEntireSheetScrub   ;
    [SerializeField] private Slider frameCountSlider            ;
    
    [Header("Data")]
    [SerializeField] private int layerIndex     ;
    [SerializeField] private int gotoFrameValue ;
    [SerializeField] private int currentFrame   ;
    [SerializeField] private int totalFrames    ;
    [SerializeField] private int startFrame     ;
    [SerializeField] private int stopFrame      ;
    [SerializeField] private int frameCount     ;

    public int lastKnownSheet;
    #endregion

    private void Start()
    {
        UpdateDisplay();
    }
    void LoadAnimation()
    {
        if(currentAnimation!=null)
            SaveFrameData();
        PVGamesAnimationMakerLayer newLayer = new PVGamesAnimationMakerLayer(incomingAnimation);
        var img = Instantiate(previewItemPrefab, previewHolder).GetComponent<Image>();
        newLayer.preview = img;
        
        incomingAnimation = null;

        //var setupData = newLayer.GetFrameData();
        //currentFrame             = setupData[0];
        //totalFrames              = setupData[1];
        //startFrame               = setupData[2];
        //stopFrame                = setupData[3];
        //frameCount               = setupData[4];
        //frameCountSlider.value   = frameCount;
        
        layers.Add(newLayer);
        layerIndex = layers.Count - 1; // since it adds it to the end of the list
        currentAnimation = layers[layerIndex].animation;
        
        LoadFrameData();
        UpdateControlsBounds();
        UpdateDisplay();
    }
    public void SaveAnimation()
    {
        if (layers.Count == 0) return; // Nothing to save
        EditorUtility.SetDirty(layers[layerIndex].animation);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        layers[layerIndex].saved = true;
        UpdateDebugOutput();
    }
    public void ApplyToDemo()
    {
        for (int i = 0; i < demoAnimatorHolder.childCount; i++)
        {
            Destroy(demoAnimatorHolder.GetChild(i).gameObject);
        }

        for (var i = 0; i < layers.Count; i++)
        {
            var layer = layers[i];
            var obj = Instantiate(demoAnimatorPrefab, demoAnimatorHolder);
            // obj.GetComponent<PVGamesAnimator>().LoadAnimation(layer.animation);
            obj.GetComponent<SpriteRenderer>().sortingOrder = i;
        }
    }

    #region Layer Control-------------------------------------------------------------------------------------
    
    public void MoveLayer(int dir)
    {
        dir += layerIndex;
        if (dir >= layers.Count || dir < 0)
            return;
        
        (layers[layerIndex], layers[dir]) = (layers[dir], layers[layerIndex]);

        layerIndex = dir;
        layers[layerIndex].preview.transform.SetSiblingIndex(layerIndex);
        UpdateDisplay();
        UpdateHierarchy();
    }
    
    public void ToggleLayerVisibility()
    {
        if(layers.Count<=0) return;
        layers[layerIndex].preview.enabled = !layers[layerIndex].preview.IsActive();
    }

    public void SelectLayer(int index)
    {
        incomingAnimation = null;
        layerIndex = index;
        currentAnimation = layers[layerIndex].animation;
        
        LoadFrameData();
        UpdateControlsBounds();
        UpdateDisplay();
    }
    
    public void SelectLayer(string selectedLayer)
    {
        
        SaveFrameData();
        for (int i = 0; i < layers.Count; i++)
        {
            if (layers[i].animation.name == selectedLayer)
            {
                if (currentAnimation == layers[i].animation)
                    return;
                layerIndex = i;
                
                incomingAnimation = null;

                currentAnimation = layers[layerIndex].animation;
                LoadFrameData();
                UpdateControlsBounds();
                UpdateControlsValues(currentFrame);
                UpdateDisplay();
                break;
            }
        }
        
    }
    
    public void DeleteLayer()
    {
        if(layers.Count<=0) return;
        
        Destroy(layers[layerIndex].preview.gameObject);
        layers[layerIndex].toBeDeleted = true;
        var temp = new List<PVGamesAnimationMakerLayer>();
        foreach(var layer in layers)
            if(!layer.toBeDeleted)
                temp.Add(layer);
        layers = temp;
        currentAnimation = null;
        if (layers.Count > 1)
        {
            int neighbor = 0;
            if (layerIndex <= layers.Count - 1)
                neighbor = layers.Count - 1;
            else if(layerIndex>=0)
                neighbor = layers.Count + 1;
            incomingAnimation = layers[neighbor].animation;
            layerIndex = neighbor;
            SelectLayer(layerIndex);
        }
        else if (layers.Count == 1)
        {
            layerIndex = 0;
            SelectLayer(layerIndex);
        }
        else
        {
            layerIndex = 0;
            layerDisplay.ClearDisplay();
        }
        

        UpdateHierarchy();
        UpdateDisplay();
    }

    #endregion
    
    #region FrameControls-------------------------------------------------------------------------------------

    private void LoadFrameData()
    {
        var data = layers[layerIndex].GetFrameData();
        currentFrame             = data[0];
        totalFrames              = data[1];
        startFrame               = data[2];
        stopFrame                = data[3];
        frameCount               = data[4];
        frameCountSlider.value   = frameCount;
    }

    private void SaveFrameData()
    {
        if (layers.Count > 0)
        {
            layers[layerIndex].lastKnownFrame = currentFrame;
        }
        
    }
    
    public void SetTargetGotoFrame(string value)
    {
        if(value!=string.Empty)
            gotoFrameValue = int.Parse(value);
    }

    public void ChangeSingleFrame(int dir)
    {
        currentFrame += dir;
        
        if (currentFrame > totalFrames)
            currentFrame = 0;
        else if (currentFrame < 0)
            currentFrame = totalFrames;
        UpdateDisplay();
    }

    public void GotoFrame()
    {
        if (gotoFrameValue <= totalFrames && gotoFrameValue >= 0)
            currentFrame = gotoFrameValue;
        UpdateDisplay();
    }

    public void ScrubToFrame(int wholeRange=0)
    {
        if (animationBoundsScrub.value == currentFrame && wholeRange==0) return;
        else if (animationEntireSheetScrub.value == currentFrame && wholeRange==1) return;
        
        if(wholeRange==0)
            currentFrame = (int)animationBoundsScrub.value;
        else if(wholeRange==1)
            currentFrame = (int)animationEntireSheetScrub.value;
        
        animationEntireSheetScrub.value     = currentFrame;
        UpdateDisplay();
    }

    public void SetImportantFrame(int end)
    {
        if (end == 0) //set START
        {
            
            if (startFrame > stopFrame)
                stopFrame = startFrame;
            
            currentAnimation.startIndex = startFrame = currentFrame;
        }
        else if (end == 1) //set END
        {
            if (stopFrame < startFrame)
                stopFrame = startFrame;
            currentAnimation.stopIndex = stopFrame = currentFrame;
            animationBoundsScrub.maxValue = currentAnimation.stopIndex;
        }
        layers[layerIndex].saved = false;
        UpdateDisplay();
    }
    
    public void SetFrameCount(Slider count)
    {
        if (currentAnimation == null) return;
        frameCount = currentAnimation.frameCount = (int)count.value;
        layers[layerIndex].saved = false;
        UpdateDisplay();
    }

    #endregion

    #region Update Functions-------------------------------------------------------------------------------------

    private void Update()
    {
        if(incomingAnimation == null)
            return;
        if (currentAnimation != incomingAnimation)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                if (layers[i].animation == incomingAnimation)
                {
                    SelectLayer(i);
                    return;
                }
            }
            LoadAnimation();
                
        }

        if (stopFrame <= 0)
            stopFrame = 1;
    }
    
    void UpdateDisplay()
    {
        if (currentAnimation == null)
        {
            currentAnimationDisplay.text = "NO ANIMATION";
            return;
        }
        
        currentAnimationDisplay.text = currentAnimation.name;
        
        UpdateControlsValues(currentFrame);
        
        if(layers[layerIndex].spriteSheet!=null)
            layers[layerIndex].preview.sprite = layers[layerIndex].spriteSheet[currentFrame];


        if(lastKnownSheet != layers.Count)
            UpdateHierarchy();
        
        
        UpdateDebugOutput();
    }

    void UpdateHierarchy()
    {
        List<AnimationSheet> temp = new List<AnimationSheet>();
        for (int i = 0; i < layers.Count; i++)
        {
            temp.Add(layers[i].animation);
        }
        layerDisplay.UpdateLayerDisplay(temp.ToArray(),layerIndex);
        lastKnownSheet = layers.Count;
    }

    void UpdateDebugOutput()
    {
        debugOutput.text = "";
        if (currentAnimation == null) return;
            
        debugOutput.text += "Editing Animation: "   + currentAnimation  .name + "\n";
        debugOutput.text += "Frame: "               + currentFrame       + "\n";
        debugOutput.text += "Total Sprites: "       + totalFrames        + "\n";
        debugOutput.text += "Start Frame: "         + startFrame         + "\n";
        debugOutput.text += "Stop Frame: "          + stopFrame          + "\n";
        debugOutput.text +=                         currentAnimation    .frameCount + " frames per direction" + "\n";
        debugOutput.text +=                         "\n";
        
        if(layers[layerIndex].saved)
            debugOutput.text += "<color=\"green\">Saved"+"\n";
        else if(!layers[layerIndex].saved)
            debugOutput.text += "<color=\"red\">Not Saved"+"\n";
    }

    void UpdateControlsBounds()
    {
        animationBoundsScrub.minValue = startFrame;
        animationBoundsScrub.maxValue = stopFrame;
        animationEntireSheetScrub.maxValue = totalFrames;
    }

    void UpdateControlsValues(int v)
    {
        animationBoundsScrub.enabled = false;

        if (v <= animationBoundsScrub.maxValue &&
            v >= animationBoundsScrub.minValue)
            animationBoundsScrub.value = v;
        
        else if (v > animationBoundsScrub.maxValue)
            animationBoundsScrub.value = currentAnimation.stopIndex;
        
        else if(v < animationBoundsScrub.minValue)
            animationBoundsScrub.value = currentAnimation.startIndex;
        
        animationBoundsScrub.enabled = true;
        
        animationEntireSheetScrub.value     = v;
    }

    #endregion
    
}
#endif