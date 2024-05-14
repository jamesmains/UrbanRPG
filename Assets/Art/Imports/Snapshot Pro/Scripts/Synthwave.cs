using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace SnapshotShaders.BuiltIn {
    [Serializable]
    [PostProcess(typeof(SynthwaveRenderer), PostProcessEvent.BeforeStack, "Snapshot Pro/Synthwave")]
    public sealed class Synthwave : PostProcessEffectSettings {
        [Tooltip("Color of the background if Use Scene Color is turned off.")]
        public ColorParameter backgroundColor = new() {value = Color.black};

        [ColorUsage(true, true)]
        [Tooltip("Bottom color of the synthwave lines. HDR colors will glow if a Bloom effect is present.")]
        public ColorParameter lineColor1 = new() {value = Color.white};

        [ColorUsage(true, true)]
        [Tooltip("Top color of the synthwave lines. HDR colors will glow if a Bloom effect is present.")]
        public ColorParameter lineColor2 = new() {value = Color.white};

        [Range(0.0f, 2.0f)]
        [Tooltip("Controls the mix between the two line colors." +
                 " Lower values favour the top color (2). Higher values favor the bottom color (1).")]
        public FloatParameter lineColorMix = new() {value = 1.0f};

        [Tooltip("Thickness of the lines in world space units.")]
        public FloatParameter lineWidth = new() {value = 0.1f};

        [Tooltip("Falloff between synthwave lines and background color in world space units.")]
        public FloatParameter lineFalloff = new() {value = 0.05f};

        [Tooltip("Space between lines along each axis in world space units.")]
        public Vector3Parameter gapWidth = new() {value = Vector3.one};

        [Tooltip("Offset from (0, 0, 0) along each axis in world space units.")]
        public Vector3Parameter offset = new() {value = Vector3.zero};

        [SerializeField] [Tooltip("Synthwave lines are shown only along these axes.")]
        public AxisMaskParameter axisMask = new() {value = AxisMask.XZ};

        [Tooltip("Use the Scene Color instead of Background Color?")]
        public BoolParameter useSceneColor = new() {value = false};
    }

    public sealed class SynthwaveRenderer : PostProcessEffectRenderer<Synthwave> {
        private static Dictionary<AxisMask, Vector3> axisMasks = new() {
            {AxisMask.XY, new Vector3(1.0f, 1.0f, 0.0f)},
            {AxisMask.XZ, new Vector3(1.0f, 0.0f, 1.0f)},
            {AxisMask.YZ, new Vector3(0.0f, 1.0f, 1.0f)},
            {AxisMask.XYZ, new Vector3(1.0f, 1.0f, 1.0f)}
        };

        public override void Render(PostProcessRenderContext context) {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/SnapshotPro/Synthwave"));

            var view = context.camera.worldToCameraMatrix;
            var proj = GL.GetGPUProjectionMatrix(context.camera.projectionMatrix, true);

            var clipToWorld = Matrix4x4.Inverse(proj * view);

            if (settings.useSceneColor) {
                sheet.EnableKeyword("USE_SCENE_TEXTURE_ON");
            }
            else {
                sheet.DisableKeyword("USE_SCENE_TEXTURE_ON");
                sheet.properties.SetColor("_BackgroundColor", settings.backgroundColor);
            }

            sheet.properties.SetMatrix("_ClipToWorld", clipToWorld);
            sheet.properties.SetColor("_LineColor1", settings.lineColor1);
            sheet.properties.SetColor("_LineColor2", settings.lineColor2);
            sheet.properties.SetFloat("_LineColorMix", settings.lineColorMix);
            sheet.properties.SetFloat("_LineWidth", settings.lineWidth);
            sheet.properties.SetFloat("_LineFalloff", settings.lineFalloff);
            sheet.properties.SetVector("_GapWidth", settings.gapWidth);
            sheet.properties.SetVector("_Offset", settings.offset);
            sheet.properties.SetVector("_AxisMask", axisMasks[settings.axisMask]);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }

    [Serializable]
    public enum AxisMask {
        XY,
        XZ,
        YZ,
        XYZ
    }

    [Serializable]
    public sealed class AxisMaskParameter : ParameterOverride<AxisMask> {
    }
}