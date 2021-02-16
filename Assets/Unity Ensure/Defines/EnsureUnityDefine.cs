using System.Collections.Generic;
using UnityEditor;

namespace GG.UnityEnsure
{
    public static class EnsureUnityDefine
    {
        /// <summary>
        /// Ensure the Define Symbol Exists
        /// </summary>
        /// <param name="defines"></param>
        public static void EnsureScriptingDefineSymbol(params string[] defines)
        {
            foreach (string s in defines)
            {
                DefineSymbolsViewer.Add(s);
            }
        }

        /// <summary>
        /// Ensure the Define Symbol Exists
        /// </summary>
        /// <param name="targetGroups">Target platforms</param>
        /// <param name="defines"></param>
        public static void EnsureScriptingDefineSymbol(List<BuildTargetGroup> targetGroups, params string[] defines)
        {
            foreach (string s in defines)
            {
                DefineSymbolsViewer.Add(s, targetGroups);
            }
        }
    }
}