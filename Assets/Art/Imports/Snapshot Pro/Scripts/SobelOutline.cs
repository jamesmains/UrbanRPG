using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace SnapshotShaders.BuiltIn {
    [Serializable]
    [PostProcess(typeof(SobelOutlineRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/SobelOutline")]
    public class SobelOutline : PostProcessEffectSettings {
        [Range(0.0f, 1.0f)] [Tooltip("Edge detection threshold.")]
        public FloatParameter threshold = new() {value = 0.5f};

        [Tooltip("Outline color.")] public ColorParameter outlineColor = new() {value = Color.white};

        [Tooltip("Background color.")] public ColorParameter backgroundColor = new() {value = Color.black};
    }

    public sealed class SobelOutlineRenderer : PostProcessEffectRenderer<SobelOutline> {
        public override void Render(PostProcessRenderContext context) {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/SobelOutline"));
            sheet.properties.SetFloat("_Threshold", settings.threshold);
            sheet.properties.SetColor("_OutlineColor", settings.outlineColor);
            sheet.properties.SetColor("_BackgroundColor", settings.backgroundColor);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}