using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace GG.UnityEnsure.Xr
{
    #if Interaction_Toolkit
    public static class EnsureUnityInteractionLayer
    {
        /// <summary>
        /// Create a layer at the next available index. Returns silently if layer already exists.
        /// </summary>
        /// <param name="list">Name of the layers to create</param>
        public static void EnsureLayers(params string[] list)
        {
            foreach (string layer in list)
            {
                AddLayer(layer);
            }
        }

        static void AddLayer(string layer)
        {
            if (string.IsNullOrEmpty(layer))
            {
                throw new System.ArgumentNullException(layer, "New layer name string is either null or empty.");
            }

            SerializedObject layerManager = new SerializedObject(Resources.Load("InteractionLayerSettings"));
            SerializedProperty layerProps = layerManager.FindProperty("m_LayerNames");
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
            layerManager.ApplyModifiedProperties();
            layerManager.Update();
        }
    }
    #endif
}