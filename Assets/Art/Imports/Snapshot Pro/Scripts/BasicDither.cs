using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace SnapshotShaders.BuiltIn {
    [Serializable]
    [PostProcess(typeof(BasicDitherRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/BasicDither")]
    public sealed class BasicDither : PostProcessEffectSettings {
        [Tooltip("Noise texture to use for dither thresholding.")]
        public TextureParameter noiseTex = new();

        [Range(0.1f, 100.0f)] [Tooltip("Size of the noise texture.")]
        public FloatParameter noiseSize = new() {value = 1.0f};

        [Range(-0.5f, 0.5f)] [Tooltip("Offset used when calculating luminance threshold.")]
        public FloatParameter thresholdOffset = new() {value = 0.0f};

        [Tooltip("Color to use for dark sections of the image.")]
        public ColorParameter darkColor = new() {value = Color.black};

        [Tooltip("Color to use for light sections of the image.")]
        public ColorParameter lightColor = new() {value = Color.white};

        [Tooltip("Use the Scene Color instead of Light Color?")]
        public BoolParameter useSceneColor = new() {value = false};
    }

    public sealed class BasicDitherRenderer : PostProcessEffectRenderer<BasicDither> {
        public override void Render(PostProcessRenderContext context) {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/BasicDither"));

            if (settings.useSceneColor)
                sheet.EnableKeyword("USE_SCENE_TEXTURE_ON");
            else
                sheet.DisableKeyword("USE_SCENE_TEXTURE_ON");

            if (settings.noiseTex != null)
                sheet.properties.SetTexture("_NoiseTex", settings.noiseTex);
            else
                sheet.properties.SetTexture("_NoiseTex", Texture2D.whiteTexture);

            sheet.properties.SetFloat("_NoiseSize", settings.noiseSize);
            sheet.properties.SetFloat("_ThresholdOffset", settings.thresholdOffset);
            sheet.properties.SetColor("_DarkColor", settings.darkColor);
            sheet.properties.SetColor("_LightColor", settings.lightColor);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}