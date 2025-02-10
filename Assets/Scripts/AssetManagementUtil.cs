using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace I302.Manu
{
#if UNITY_EDITOR
    public static class AssetManagementUtil
    {
        public static List<T> GetAllScriptableObjectInstances<T>() where T : ScriptableObject
        {
            return AssetDatabase.FindAssets($"t: {typeof(T).Name}").ToList()
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .ToList();
        }
    }
#endif
}
