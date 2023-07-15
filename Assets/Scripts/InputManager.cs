using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    [SerializeField] private List<Keybind> DefaultKeybinds;
    [SerializeField] private List<Keybind> CurrentKeybinds;

    private void OnEnable()
    {
        LoadKeybinds();
        for (int i = 0; i < CurrentKeybinds.Count; i++)
        {
            switch (CurrentKeybinds[i].InputAction)
            {
                case InputActionName.MoveLeft:
                    CurrentKeybinds[i].OnButtonDown.AddListener(delegate { GameEvents.OnMoveLeftButtonDown.Raise(); });
                    CurrentKeybinds[i].OnButtonUp.AddListener(delegate { GameEvents.OnMoveLeftButtonUp.Raise(); });
                    CurrentKeybinds[i].OnButtonHeld.AddListener(delegate { GameEvents.OnMoveLeftButtonHeld.Raise(); });
                    CurrentKeybinds[i].OnButtonReleased.AddListener(delegate { GameEvents.OnMoveLeftButtonReleased.Raise(); });
                    break;
                case InputActionName.MoveRight:
                    CurrentKeybinds[i].OnButtonDown.AddListener(delegate { GameEvents.OnMoveRightButtonDown.Raise(); });
                    CurrentKeybinds[i].OnButtonUp.AddListener(delegate { GameEvents.OnMoveRightButtonUp.Raise(); });
                    CurrentKeybinds[i].OnButtonHeld.AddListener(delegate { GameEvents.OnMoveRightButtonHeld.Raise(); });
                    CurrentKeybinds[i].OnButtonReleased.AddListener(delegate { GameEvents.OnMoveRightButtonReleased.Raise(); });
                    break;
                case InputActionName.MoveUp:
                    CurrentKeybinds[i].OnButtonDown.AddListener(delegate { GameEvents.OnMoveUpButtonDown.Raise(); });
                    CurrentKeybinds[i].OnButtonUp.AddListener(delegate { GameEvents.OnMoveUpButtonUp.Raise(); });
                    CurrentKeybinds[i].OnButtonHeld.AddListener(delegate { GameEvents.OnMoveUpButtonHeld.Raise(); });
                    CurrentKeybinds[i].OnButtonReleased.AddListener(delegate { GameEvents.OnMoveUpButtonReleased.Raise(); });
                    break;
                case InputActionName.MoveDown:
                    CurrentKeybinds[i].OnButtonDown.AddListener(delegate { GameEvents.OnMoveDownButtonDown.Raise(); });
                    CurrentKeybinds[i].OnButtonUp.AddListener(delegate { GameEvents.OnMoveDownButtonUp.Raise(); });
                    CurrentKeybinds[i].OnButtonHeld.AddListener(delegate { GameEvents.OnMoveDownButtonHeld.Raise(); });
                    CurrentKeybinds[i].OnButtonReleased.AddListener(delegate { GameEvents.OnMoveDownButtonReleased.Raise(); });
                    break;
                case InputActionName.Interact:
                    CurrentKeybinds[i].OnButtonDown.AddListener(delegate { GameEvents.OnInteractButtonDown.Raise(); });
                    CurrentKeybinds[i].OnButtonUp.AddListener(delegate { GameEvents.OnInteractButtonUp.Raise(); });
                    CurrentKeybinds[i].OnButtonHeld.AddListener(delegate { GameEvents.OnInteractButtonHeld.Raise(); });
                    CurrentKeybinds[i].OnButtonReleased.AddListener(delegate { GameEvents.OnInteractButtonReleased.Raise(); });
                    break;
                case InputActionName.ToggleRide:
                    CurrentKeybinds[i].OnButtonDown.AddListener(delegate { GameEvents.OnRideButtonDown.Raise(); });
                    CurrentKeybinds[i].OnButtonUp.AddListener(delegate { GameEvents.OnRideButtonUp.Raise(); });
                    CurrentKeybinds[i].OnButtonHeld.AddListener(delegate { GameEvents.OnRideButtonHeld.Raise(); });
                    CurrentKeybinds[i].OnButtonReleased.AddListener(delegate { GameEvents.OnRideButtonReleased.Raise(); });
                    break;
                case InputActionName.Scroll:
                    CurrentKeybinds[i].OnButtonHeld.AddListener(delegate { GameEvents.OnMouseScroll.Raise(Input.mouseScrollDelta.y); });
                    break;
                case InputActionName.PrimaryMouseButton:
                    CurrentKeybinds[i].OnButtonDown.AddListener(delegate { GameEvents.OnPrimaryMouseButtonDown.Raise(); });
                    CurrentKeybinds[i].OnButtonUp.AddListener(delegate { GameEvents.OnPrimaryMouseButtonUp.Raise(); });
                    CurrentKeybinds[i].OnButtonHeld.AddListener(delegate { GameEvents.OnPrimaryMouseButtonHeld.Raise(); });
                    CurrentKeybinds[i].OnButtonReleased.AddListener(delegate { GameEvents.OnPrimaryMouseButtonReleased.Raise(); });
                    break;
            }
        }
    }

    private void Update()
    {
        foreach (var binding in CurrentKeybinds)
        {
            if (binding.Type == InputType.Keyboard)
            {
                if (Input.GetKey(binding.Key))
                {
                    binding?.OnButtonHeld.Invoke();
                }
                
                if (Input.GetKeyDown(binding.Key))
                {
                    binding?.OnButtonDown.Invoke();
                }
                
                if (Input.GetKeyUp(binding.Key))
                {
                    binding?.OnButtonUp.Invoke();
                }

                if (!Input.GetKey(binding.Key))
                {
                    binding?.OnButtonReleased.Invoke();
                }
            }
            else if (binding.Type == InputType.MouseWheel)
            {
                binding?.OnButtonHeld.Invoke();
            }
            else if (binding.Type == InputType.Gamepad)
            {
                // todo add gamepad support
            }
        }
    }

    public void ChangeKeybind()
    {
        
    }
    
    private void LoadKeybinds()
    {
        CurrentKeybinds = DefaultKeybinds;
    }
    
    private void SaveKeybinds()
    {
        
    }
    
    public void ResetKeybinds()
    {
        CurrentKeybinds = DefaultKeybinds;
        // Save Keybinds
    }
}

[Serializable]
public class Keybind
{
    public InputType Type;
    public InputActionName InputAction;
    [ShowIf("Type",InputType.Keyboard)]public KeyCode Key;
    [ShowIf("Type",InputType.Gamepad)]public string ButtonName;
    [HideInInspector]public UnityEvent OnButtonDown;
    [HideInInspector]public UnityEvent OnButtonUp;
    [HideInInspector]public UnityEvent OnButtonHeld;
    [HideInInspector]public UnityEvent OnButtonReleased;
}