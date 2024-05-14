#if UNITY_EDITOR


using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

// for InternalSpriteUtility

// for Cast()

/// <summary>
///     Found at https://blog.floriancourgey.com/2021/04/unity-bulk-slice-sprite-csharp
///     Removed allow list, fixed some bugs
/// </summary>
public class SpriteSlicer : MonoBehaviour {
    private static readonly int[] skipPoints = {8, 9, 14, 19, 24, 29, 34, 39, 44, 49, 54, 59, 64};

    //[MenuItem("Tools/FCO/Compute Accessory Sprites")]
    private static void ComputeAccessorySprites() {
        const int sliceWidth = 19;
        const int sliceHeight = 32;
        const int pixelsPerUnit = 32;
        const int stoppingPoint = 34;

        var folder = "spriteProcessing";

        var textures = Resources.LoadAll(folder, typeof(Texture2D)).Cast<Texture2D>().ToArray();

        foreach (var texture in textures) {
            var tex = texture;
            var path = AssetDatabase.GetAssetPath(tex);

            var ti = AssetImporter.GetAtPath(path) as TextureImporter;
            ti.isReadable = true;
            ti.textureType = TextureImporterType.Sprite;
            ti.spriteImportMode = SpriteImportMode.Multiple;
            ti.spritePixelsPerUnit = pixelsPerUnit;
            ti.filterMode = FilterMode.Point;
            ti.textureCompression = TextureImporterCompression.Uncompressed;

            var newData = new List<SpriteMetaData>();

            var rects = InternalSpriteUtility.GenerateGridSpriteRectangles(
                tex, Vector2.zero, new Vector2(sliceWidth, sliceHeight), Vector2.zero);
            for (var i = 0; i < rects.Length; i++) {
                if (i == stoppingPoint) break;
                if (skipPoints.Contains(i)) continue;
                var smd = new SpriteMetaData();
                smd.rect = rects[i];
                smd.pivot = new Vector2(0.5f, 0f);
                smd.alignment = (int) SpriteAlignment.Center;
                smd.name = tex.name + "_" + i; // name_41
                newData.Add(smd);
            }

            ti.alphaSource = TextureImporterAlphaSource.FromInput;

#pragma warning disable 618
            ti.spritesheet = newData.ToArray();
#pragma warning restore 618
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
    }

    [MenuItem("Tools/FCO/Compute Gear Sprites")]
    private static void ComputeCoreSprites() {
        const int sliceWidth = 19;
        const int sliceHeight = 32;
        const int pixelsPerUnit = 32;
        const int stoppingPoint = 64;

        var folder = "spriteProcessing";

        var textures = Resources.LoadAll(folder, typeof(Texture2D)).Cast<Texture2D>().ToArray();

        foreach (var texture in textures) {
            var tex = texture;
            var path = AssetDatabase.GetAssetPath(tex);

            var ti = AssetImporter.GetAtPath(path) as TextureImporter;
            ti.isReadable = true;
            ti.textureType = TextureImporterType.Sprite;
            ti.spriteImportMode = SpriteImportMode.Multiple;
            ti.spritePixelsPerUnit = pixelsPerUnit;
            ti.filterMode = FilterMode.Point;
            ti.textureCompression = TextureImporterCompression.Uncompressed;

            var newData = new List<SpriteMetaData>();

            var rects = InternalSpriteUtility.GenerateGridSpriteRectangles(
                tex, Vector2.zero, new Vector2(sliceWidth, sliceHeight), Vector2.zero);
            for (var i = 0; i < rects.Length; i++) {
                if (skipPoints.Contains(i)) continue;
                if (i == stoppingPoint) break;
                var smd = new SpriteMetaData();
                smd.rect = rects[i];
                smd.pivot = new Vector2(0.5f, 0.5f);
                smd.alignment = (int) SpriteAlignment.Center;
                smd.name = tex.name + "_" + i; // name_41
                newData.Add(smd);
            }

            ti.alphaSource = TextureImporterAlphaSource.FromInput;

#pragma warning disable 618
            ti.spritesheet = newData.ToArray();
#pragma warning restore 618
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
    }
}
#endif