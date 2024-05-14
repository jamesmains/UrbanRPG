using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace SnapshotShaders.BuiltIn {
    public class ExampleScript : MonoBehaviour {
        public PostProcessVolume volume;

        // Update is called once per frame
        private void Update() {
            OilPainting oilPaintingEffect = null;
            volume.profile.TryGetSettings(out oilPaintingEffect);

            if (oilPaintingEffect != null) oilPaintingEffect.kernelSize.value = 100;
        }
    }
}