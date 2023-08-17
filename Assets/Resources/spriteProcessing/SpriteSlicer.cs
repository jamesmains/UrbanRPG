#if UNITY_EDITOR


using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEditorInternal; // for InternalSpriteUtility
using System.Linq; // for Cast()

/// <summary>
/// Found at https://blog.floriancourgey.com/2021/04/unity-bulk-slice-sprite-csharp
/// Removed allow list, fixed some bugs
/// </summary>

public class SpriteSlicer : MonoBehaviour{
    private static readonly int[] skipPoints = {8, 9, 14, 19, 24, 29, 34, 39, 44, 49, 54, 59, 64};

    //[MenuItem("Tools/FCO/Compute Accessory Sprites")]
     static void ComputeAccessorySprites(){

        const int sliceWidth = 19;
        const int sliceHeight = 32;
        const int pixelsPerUnit = 32;
        const int stoppingPoint = 34;

     string folder = "spriteProcessing";

        Texture2D[] textures = Resources.LoadAll(folder, typeof(Texture2D)).Cast<Texture2D>().ToArray();

         foreach (Texture2D texture in textures){
             Texture2D tex = (Texture2D) texture;
             string path = AssetDatabase.GetAssetPath(tex);
             
             TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
             ti.isReadable = true;
             ti.textureType = TextureImporterType.Sprite;
             ti.spriteImportMode = SpriteImportMode.Multiple;
             ti.spritePixelsPerUnit = pixelsPerUnit;
             ti.filterMode = FilterMode.Point;
             ti.textureCompression = TextureImporterCompression.Uncompressed;

             List<SpriteMetaData> newData = new List<SpriteMetaData>();
            
            Rect[] rects = InternalSpriteUtility.GenerateGridSpriteRectangles(
                tex, Vector2.zero, new Vector2(sliceWidth,sliceHeight), Vector2.zero);
            for (int i = 0; i < rects.Length; i++)
            {
                if (i == stoppingPoint) break;
                if(skipPoints.Contains(i)) continue;
                SpriteMetaData smd = new SpriteMetaData();
                smd.rect = rects[i];
                smd.pivot = new Vector2(0.5f, 0f);
                smd.alignment = (int)SpriteAlignment.Center;
                smd.name = tex.name+"_"+i; // name_41
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
     static void ComputeCoreSprites(){

         const int sliceWidth = 19;
         const int sliceHeight = 32;
         const int pixelsPerUnit = 32;
         const int stoppingPoint = 64;

         string folder = "spriteProcessing";

         Texture2D[] textures = Resources.LoadAll(folder, typeof(Texture2D)).Cast<Texture2D>().ToArray();

         foreach (Texture2D texture in textures){
             Texture2D tex = (Texture2D) texture;
             string path = AssetDatabase.GetAssetPath(tex);
             
             TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
             ti.isReadable = true;
             ti.textureType = TextureImporterType.Sprite;
             ti.spriteImportMode = SpriteImportMode.Multiple;
             ti.spritePixelsPerUnit = pixelsPerUnit;
             ti.filterMode = FilterMode.Point;
             ti.textureCompression = TextureImporterCompression.Uncompressed;

             List<SpriteMetaData> newData = new List<SpriteMetaData>();
            
             Rect[] rects = InternalSpriteUtility.GenerateGridSpriteRectangles(
                 tex, Vector2.zero, new Vector2(sliceWidth,sliceHeight), Vector2.zero);
             for (int i = 0; i < rects.Length; i++)
             {
                 if(skipPoints.Contains(i)) continue;
                 if (i == stoppingPoint) break;
                 SpriteMetaData smd = new SpriteMetaData();
                 smd.rect = rects[i];
                 smd.pivot = new Vector2(0.5f, 0.5f);
                 smd.alignment = (int)SpriteAlignment.Center;
                 smd.name = tex.name+"_"+i; // name_41
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