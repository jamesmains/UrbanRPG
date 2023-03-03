using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Player/Skill")]
public class Skill : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
    public int Level;
    public int ExpRequiredForLevelUp;
    public int CurrentExp;
    public GameEvent OnLevelUp;
    public SkillGainGameEvent OnGainExperience;

    public void AddExperience(int incomingExperience)
    {
        if (Level >= 99) return;
        int remainingExp = incomingExperience - ExpRequiredForLevelUp;
        CurrentExp += incomingExperience;
        
        if(CurrentExp >= ExpRequiredForLevelUp)
            LevelUp();
        
        if(remainingExp > 0)
            AddExperience(remainingExp);
        
        OnGainExperience.Raise(Icon,incomingExperience);
    }

    private void LevelUp()
    {
        ExpRequiredForLevelUp += 100;
        CurrentExp = 0;
        Level++;
        OnLevelUp.Raise();
    }

    [Button]
    private void ResetSkill()
    {
        ExpRequiredForLevelUp = 100;
        CurrentExp = 0;
        Level = 0;
    }
}
