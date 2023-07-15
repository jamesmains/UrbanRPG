using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupDisplayController : MonoBehaviour
{
    [SerializeField] private GameObject skillExpGainPopupObject;
    [SerializeField] private GameObject iconPopupObject;

    public void CreateSkillExpGainPopup(Sprite icon, int value)
    {
        Instantiate(skillExpGainPopupObject, this.transform).GetComponent<DisplayPopup>().Setup(icon,value);
    }

    public void CreateIconPopup(Sprite icon)
    {
        Instantiate(iconPopupObject, this.transform).GetComponent<DisplayPopup>().Setup(icon);
    }
}
