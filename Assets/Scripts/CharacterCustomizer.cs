using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Diagnostics;

public class CharacterCustomizer : MonoBehaviour
{
    [SerializeField] private Actor currentActor;
    [SerializeField] private CustoAnimator animator;
    [SerializeField] private Transform gearOptionSelectorContainer;
    [SerializeField] private GameObject gearOptionSelectorPrefab;
    [SerializeField] private GearSlot[] gearSlots;
    [field: SerializeField] private Gear[] GearOptions;

    private int[] indexes;

    private void Start()
    {
        animator.actor = currentActor;
        animator.UpdateActor();
        PopulateAllOptionsSelectors();
    }

    private void PopulateAllOptionsSelectors()
    {
        // int i = 0;
        // foreach (var gearType in Enum.GetNames(typeof(GearType)))
        // {
        //     print(gearSlots[i]);
        //     var gearArray = GearOptions.Where(o => o.GearType.ToString() == gearType).ToArray();
        //     if(gearArray.Length > 0)
        //         PopulateOptionSelector(gearArray,currentActor.GearBody,gearSlots[i],indexes[i]);
        //     i++;
        // }
    }

    private void PopulateOptionSelector(Gear[] gearArray, Gear currentlyEquipped, GearSlot slot, int indexer, bool allowNull = false)
    {
        var obj = Instantiate(gearOptionSelectorPrefab, gearOptionSelectorContainer);
        var text = obj.GetComponentInChildren<TextMeshProUGUI>();
        for (int i = 0; i < gearArray.Length; i++)
        {
            if (gearArray[i] == currentlyEquipped)
            {
                indexer = i;
            }
        }
        text.text = gearArray[indexer].Name;
        obj.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
        {
            indexer--;
            if (!allowNull)
            {
                if (indexer < 0) indexer = gearArray.Length - 1;
                slot.gear = gearArray[indexer];
                text.text = gearArray[indexer].Name;    
            }
            else
            {
                
                if (indexer == -1)
                {
                    slot.gameObject.SetActive(false);
                    slot.gear = null;
                    text.text = "Nothing";
                }
                else
                {
                    if (indexer < -1)
                    {
                        slot.gameObject.SetActive(true);
                        indexer = gearArray.Length - 1;
                    }
                    slot.gear = gearArray[indexer];
                    text.text = gearArray[indexer].Name;
                }
            }
            
        });
        obj.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate
        {
            indexer++;
            if (!allowNull)
            {
                if (indexer >= gearArray.Length) indexer = 0;
                slot.gear = gearArray[indexer];
                text.text = gearArray[indexer].Name; 
            }
            else
            {
                if (indexer == gearArray.Length)
                {
                    slot.gameObject.SetActive(false);
                    slot.gear = null;
                    text.text = "Nothing";
                }
                else
                {
                    if (indexer > gearArray.Length)
                    {
                        slot.gameObject.SetActive(true);
                        indexer = 0;
                    }

                    slot.gear = gearArray[indexer];
                    text.text = gearArray[indexer].Name;
                }
            }
        });
    }
    
    public void SetHairColor(Color incomingColor)
    {
        currentActor.hairColor = incomingColor;
        gearSlots[6].gameObject.GetComponent<SpriteRenderer>().color = incomingColor;
    }

    [Button]
    public void SaveOutfit()
    {
        // currentActor.GearBody = bodyGearOptions[bodyIndex];
        // currentActor.GearShoes = shoeGearOptions[shoeIndex];
        // currentActor.GearPants = pantsGearOptions[pantsIndex];
        // currentActor.GearMouth = mouthGearOptions[mouthIndex];
        // currentActor.GearEyes = eyesGearOptions[eyesIndex];
        // currentActor.GearShirt = shirtGearOptions[shirtIndex];
        // currentActor.GearHair = hairGearOptions[hairIndex];
        // currentActor.GearAccessory1 = accessoryGearOptions[accessoryIndex];
        // currentActor.GearAccessory2 = accessory1GearOptions[accessory1Index];
    }
}

[Serializable]
public class GearOption
{
    
}
