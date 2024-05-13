using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse.Utils {
    [CreateAssetMenu(fileName = "PVGames Animation Sheet", menuName = "Unsorted/PVGames Animation Sheet", order = 1)]
    public class AnimationSheet : ScriptableObject
    {
        [FoldoutGroup("Data")] public int startIndex;
        [FoldoutGroup("Data")] public int frameCount;
     
// #if UNITY_EDITOR
//     [Button]
//     public void SetID()
//     {
//         string relativePath = Path.GetDirectoryName(Application.dataPath+"/"+AssetDatabase.GetAssetPath(this).Split("Assets/")[1]);
//         var relativeDir = Directory.GetFiles(relativePath).ToList();
//         var targetString = relativeDir.FindAll(o => o.Contains(".png") && !o.Contains(".meta"));
//         ID = targetString[0].Split("Resources\\")[1].Replace(".png","");
//     }
// #endif
    }
}