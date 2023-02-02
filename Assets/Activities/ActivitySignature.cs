using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using UnityEngine;

[CreateAssetMenu(fileName = "Activity Signature", menuName = "Scriptable Objects/Activity Signature")]
public class ActivitySignature : ScriptableObject
{
    public ActionType actionType;
    public string ActionName;
    public Sprite ActionIcon;
    public Item RequiredItem;
    public int RequiredItemQuantity;
}
