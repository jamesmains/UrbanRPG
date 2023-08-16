using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine.Diagnostics;
using UnityEngine.Events;

public class CharacterCustomizer : MonoBehaviour
{
    [SerializeField] private Actor currentActor;
    [SerializeField] private CustoAnimator animator;
    [SerializeField] private TextMeshProUGUI categoryNameDisplay;
    [SerializeField] private FlexibleColorPicker hairColorPicker;
    [SerializeField] private Transform categoryContainer;
    [SerializeField] private Transform foldoutContainer;
    [SerializeField] private Transform currentFoldoutContainer;
    [SerializeField] private GameObject categoryButton;
    [SerializeField] private GameObject gearFoldout;
    [SerializeField] private GameObject gearButton;
    [field: SerializeField] private List<GearCollection> GearOptionCollections = new();

    private bool hairColorBuffer = true; // The color picker sets the color to a default color on frame 1,
                                         // his prevents it from also setting the actor's hair color to the default
    private readonly List<Window> categoryWindows = new();
    private int[] indexes;

    private void Awake()
    {
        hairColorBuffer = true;
    }

    private void Start()
    {
        animator.actor = currentActor;
        PopulateCategories();
        animator.UpdateActor(currentActor);
        hairColorPicker.SetColor(animator.actor.hairColor);
    }

    private void PopulateCategories()
    {
        for (var i = 0; i < GearOptionCollections.Count; i++)
        {
            var t = GearOptionCollections[i];
            var category = Instantiate(categoryButton, categoryContainer).GetComponent<GearCategory>();
            var foldout = Instantiate(gearFoldout, foldoutContainer).GetComponent<GearFoldout>();

            var categoryWindow = foldout.gameObject.GetComponent<Window>();
            categoryWindows.Add(categoryWindow);

            currentFoldoutContainer = foldout.GetContainer();

            foldout.ChangeHeader(t.header);         
            
            if (categoryNameDisplay != null)
            {
                var effects = category.GetComponentInChildren<MouseInteractionEffects>();
                effects.Effects.Add(new ChangeTextEffect(categoryNameDisplay,t.header));
            }
            
            var onClickCategory = new UnityAction(delegate
            {
                foreach (var window in categoryWindows)
                {
                    window.Hide();
                    window.gameObject.SetActive(false);
                }
                categoryWindow.gameObject.SetActive(true);
                categoryWindow.Show();
            });
            category.Setup(onClickCategory, t.icon);

            PopulateGearButtons(t.gear.ToArray(),t.type,t.allowNull);
        }
    }

    private void PopulateGearButtons(IEnumerable<Gear> gearArray, GearType type, bool allowNull = true)
    {
        if (allowNull)
        {
            var obj = Instantiate(gearButton, currentFoldoutContainer);
            obj.GetComponent<GearButton>().Setup(currentActor,null,type);
            // Todo, need some kind of empty icon
        }
        foreach (var gear in gearArray)
        {
            var obj = Instantiate(gearButton, currentFoldoutContainer);
            obj.GetComponent<GearButton>().Setup(currentActor,gear,type);
        }
        
    }
    
    public void SetHairColor(Color incomingColor)
    {
        if (hairColorBuffer)
        {
            hairColorBuffer = false;
            return;
        }
        currentActor.hairColor = incomingColor;
        var hair = animator.gearSlots.FirstOrDefault(o => o.gearType == GearType.Hair);
        if(hair != null) hair.gameObject.GetComponent<SpriteRenderer>().color = incomingColor;
    }

    public void SetCharacterFacingDirection(Vector2 direction)
    {
        animator.SetDirection(direction);
    }

    public void SetCharacterAnimation(int a)
    {
        animator.SetAction(a);
    }

    [Button]
    public void SaveOutfit()
    {
    }
    
}


