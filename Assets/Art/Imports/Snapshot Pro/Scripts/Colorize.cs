using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace SnapshotShaders.BuiltIn {
    [Serializable]
    [PostProcess(typeof(ColorizeRenderer), PostProcessEvent.AfterStack, "Snapshot Pro/Colorize")]
    public sealed class Colorize : PostProcessEffectSettings {
        [Tooltip("Tint colour to use.")] public ColorParameter tintColor = new() {value = Color.white};
    }

    public sealed class ColorizeRenderer : PostProcessEffectRenderer<Colorize> {
        public override void Render(PostProcessRenderContext context) {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/Colorize"));
            sheet.properties.SetColor("_TintColor", settings.tintColor);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}