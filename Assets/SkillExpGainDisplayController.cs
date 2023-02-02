using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillExpGainDisplayController : MonoBehaviour
{
    [SerializeField] private GameObject skillExpGainPopupObject;

    public void CreateSkillExpGainPopup(Sprite icon, int value)
    {
        Instantiate(skillExpGainPopupObject, this.transform).GetComponent<SkillExpGainDisplayPopup>().Setup(icon,value);
    }
}
