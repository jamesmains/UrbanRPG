using System;
using System.Collections;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using Sirenix.OdinInspector;
using UnityEngine;

public interface Interactable {
    public void Interact();
    public void ExitInteraction();
    public int Priority { get; }
}
