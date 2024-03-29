using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Signatures/Skill")]
public class Skill : ScriptableObject
{
    [FoldoutGroup("Details")]public string Name;
    [FoldoutGroup("Details")]public string Description;
    [FoldoutGroup("Details"),PreviewField]public Sprite Icon;
    [FoldoutGroup("Data")]public int Level;
    [FoldoutGroup("Data")]public int ExpRequiredForLevelUp;
    [FoldoutGroup("Data")]public int CurrentExp;

    public void AddExperience(int incomingExperience)
    {
        if (Level >= 99) return;
        int remainingExp = incomingExperience - ExpRequiredForLevelUp;
        CurrentExp += incomingExperience;
        
        if(CurrentExp >= ExpRequiredForLevelUp)
            LevelUp();
        
        if(remainingExp > 0)
            AddExperience(remainingExp);
        
        GameEvents.OnGainExperience.Raise();
        GameEvents.OnCreateSpriteStringPopup.Raise(Icon,$"+{incomingExperience}");
    }

    private void LevelUp()
    {
        ExpRequiredForLevelUp += 100;
        CurrentExp = 0;
        Level++;
        GameEvents.OnLevelUp.Raise();
        GameEvents.OnSendGenericMessage.Raise($"{Name} raised to level {Level}!");
    }

    [Button]
    private void ResetSkill()
    {
        ExpRequiredForLevelUp = 100;
        CurrentExp = 0;
        Level = 0;
    }
}
