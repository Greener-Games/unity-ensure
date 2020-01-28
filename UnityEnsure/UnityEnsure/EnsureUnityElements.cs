using UnityEditor;
using UnityEngine;

namespace UnityEnsure
{
    public class EnsureUnityElements
    {
        [InitializeOnLoadMethod]
        static void EnsureScriptingDefineSymbol()
        {
        }

        /// <summary>
        /// Ensure the Define Symbol Exists
        /// </summary>
        /// <param name="list"></param>
        public static void EnsureScriptingDefineSymbol(params string[] list)
        {
            foreach (string s in list)
            {
                DefineSymbols.Add(s);
            }
        }

        /// <summary>
        /// Create a layer at the next available index. Returns silently if layer already exists.
        /// </summary>
        /// <param name="list">Name of the layers to create</param>
        public static void EnsureLayers(params string[] list)
        {
            foreach (string layer in list)
            {
                if (string.IsNullOrEmpty(layer))
                {
                    throw new System.ArgumentNullException(layer, "New layer name string is either null or empty.");
                }

                SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
                SerializedProperty layerProps = tagManager.FindProperty("layers");
                int propCount = layerProps.arraySize;

                SerializedProperty firstEmptyProp = null;

                for (int i = 0; i < propCount; i++)
                {
                    SerializedProperty layerProp = layerProps.GetArrayElementAtIndex(i);

                    string stringValue = layerProp.stringValue;

                    if (stringValue == layer) return;

                    if (i < 8 || stringValue != string.Empty) continue;

                    if (firstEmptyProp == null)
                        firstEmptyProp = layerProp;
                }

                if (firstEmptyProp == null)
                {
                    Debug.LogError("Maximum limit of " + propCount + " layers exceeded. Layer \"" + layer + "\" not created.");
                    return;
                }

                firstEmptyProp.stringValue = layer;
                tagManager.ApplyModifiedProperties();
                tagManager.Update();
            }
        }
    }
}