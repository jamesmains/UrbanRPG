using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace SnapshotShaders.BuiltIn {
    public class SnapToOrigin : MonoBehaviour {
        [SerializeField] private PostProcessVolume volume;

        private WorldScan worldScanEffect;

        private void Start() {
            volume.profile.TryGetSettings(out worldScanEffect);
        }

        private void Update() {
            if (worldScanEffect != null) transform.position = worldScanEffect.scanOrigin;
        }
    }
}