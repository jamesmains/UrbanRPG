using System;
using UnityEngine;
public enum ActivationType {
    Once,
    Infinite,
    Toggle
}
[Serializable]
public class InteractionSettings
{
    public InteractionSettings() {
    }
    
    public ActivationType ActivationSetting = ActivationType.Infinite;
    public ActivationType DeactivationSetting = ActivationType.Infinite;
    public bool RequireKeyToActivate;
    public bool RequireKeyToDeactivate;
    public bool ShowInteractPrompt;
    public bool ActiveOnEnable;
    
}
