using System;

namespace Gnomes.Interactions {
    [Serializable]
    public class InteractionSettings
    {
        public InteractionSettings() {
        }

        public bool RequireKeyToActivate;
        public bool ShowInteractPrompt;
        public bool ActiveOnEnable;
        public bool Toggles;
    }
}