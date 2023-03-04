using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Keybinding Layout", menuName = "Variables/Keybinding Layout")]
public class KeybindingLayoutVariable : ScriptableObject
{
    public InputType InputType;
    public List<Keybind> keybinds;
}

[Serializable]
public class Keybind
{
    public KeyCode Key;
    public KeyInteractionType KeyInteractionType;
    [Tooltip("Doubles as button name for gamepad")]
    public string ButtonName;
    public GameEvent Event;
}