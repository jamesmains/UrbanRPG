#if UNITY_EDITOR


using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal; // for InternalSpriteUtility
using System.Linq; // for Cast()

/// <summary>
/// Found at https://blog.floriancourgey.com/2021/04/unity-bulk-slice-sprite-csharp
/// Removed allow list, fixed some bugs
/// </summary>
public class SpriteSlicer : MonoBehaviour{
     [MenuItem("Tools/FCO/Compute sprites")]
     static void ComputeSprites(){
        Debug.Log("ComputeSprites: start");

        const int sliceWidth = 96;
        const int sliceHeight = 64;
        const int pixelsPerUnit = 20;

        string folder = "spriteProcessing";

        Texture2D[] textures = Resources.LoadAll(folder, typeof(Texture2D)).Cast<Texture2D>().ToArray();
        Debug.Log("ComputeSprites: textures.Length: " + textures.Length);

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
            for (int i = 0; i < rects.Length; i++){
                SpriteMetaData smd = new SpriteMetaData();
                smd.rect = rects[i];
                smd.pivot = new Vector2(0.5f, 0.5f);
                smd.alignment = (int)SpriteAlignment.Center;
                smd.name = tex.name+"_"+i; // name_41
                newData.Add(smd);
            }

            ti.spritesheet = newData.ToArray();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            Debug.Log("ComputeSprites: resource ok");
         }
         Debug.Log("ComputeSprites: done");
     }
 }
#endif