using System;
using UnityEngine;

public class ReputationManager : MonoBehaviour {
    public static Action<Guid, int> OnModifyReputationWithFriend; // Applies modification to specific actor detail
    public static Action<Guid, int> OnModifyReputationWithFaction; // Applies modification to faction (not yet implemented)
}
