using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private KeybindingLayoutVariable keybindingLayoutVariable;
    private void Update()
    {
        foreach (var binding in keybindingLayoutVariable.keybinds)
        {
            if (keybindingLayoutVariable.InputType == InputType.Keyboard)
            {
                switch (binding.KeyInteractionType)
                {
                    case KeyInteractionType.Any:
                        if (Input.GetKey(binding.Key))
                        {
                            binding?.Event.Raise();
                        }
                        break;
                    case KeyInteractionType.Pressed:
                        if (Input.GetKeyDown(binding.Key))
                        {
                            binding?.Event.Raise();
                        }
                        break;
                    case KeyInteractionType.Released:
                        if (Input.GetKeyUp(binding.Key))
                        {
                            binding?.Event.Raise();
                        }
                        break;
                }
                
            }
            else if (keybindingLayoutVariable.InputType == InputType.Gamepad)
            {
                // todo add gamepad support
            }
        }
    }

    public void DebugKeyUp()
    {
        print("Key was released!");
    }

    public void DebugKeyDown()
    {
        print("Key was pressed!");
    }

    public void DebugKeyAny()
    {
        print("Key is being pressed!");
    }
}
