using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityEnsure
{
    public class DefineSymbols: EditorWindow
    {
        List<string> defineSymbolsList;
        BuildTargetGroup currentSelectedPlatform;
        static DefineSymbols window;

        static readonly List<BuildTargetGroup> targetGroup = new List<BuildTargetGroup>
        {
            BuildTargetGroup.Standalone,
            BuildTargetGroup.Android,
            BuildTargetGroup.iOS,
            BuildTargetGroup.WebGL,
        };
        
        [MenuItem("Window/Editor Extensions/Scripting Define Symbols %i")]
        public static void Init()
        {
            window = (DefineSymbols) GetWindow(typeof(DefineSymbols), true);

            window.currentSelectedPlatform = BuildTargetGroup.Standalone;

            window.defineSymbolsList = new List<string>();

            GetAllSymbolsFromPlayerSettings(window.currentSelectedPlatform, ref window.defineSymbolsList);

            window.Show();
        }

        void OnGUI()
        {
            if (window == null)
            {
                // sometimes  object instance loses, could n't find reason for this
                Debug.LogError("Something went wrong");
                window = GetWindow<DefineSymbols>();
                //return;
            }

            GUILayout.Label("Add or Remove Scripting Define symbols", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();

            for (int i = 0; i < targetGroup.Count; i++)
            {
                BuildTargetGroup iterTarget = targetGroup[i];

                GUIStyle btnStyle = new GUIStyle(GUI.skin.button);

                if (window.currentSelectedPlatform == iterTarget)
                {
                    btnStyle.normal.background = btnStyle.active.background;
                }

                if (GUILayout.Button(iterTarget.ToString().ToUpper(), btnStyle))
                {
                    GUI.FocusControl("");
                    List<string> getsavedData = new List<string>();

                    GetAllSymbolsFromPlayerSettings(window.currentSelectedPlatform, ref getsavedData);


                    List<string> list = new List<string>();

                    window.defineSymbolsList.RemoveAll(x => x == "");

                    if (window.defineSymbolsList.Count > getsavedData.Count)
                    {
                        list = window.defineSymbolsList.Except(getsavedData).ToList();
                    }
                    else
                    {
                        list = getsavedData.Except(window.defineSymbolsList).ToList();
                    }


                    if (list.Count > 0)
                    {
                        bool _event = EditorUtility.DisplayDialog("Unapplied changes", "Unapplied changes for " + window.currentSelectedPlatform, "Apply", "Revert");

                        if (_event)
                        {
                            window.ApplyAllChangesToPlayerSettings(window.currentSelectedPlatform, defineSymbolsList);
                        }
                    }

                    window.currentSelectedPlatform = iterTarget;

                    GetAllSymbolsFromPlayerSettings(window.currentSelectedPlatform, ref window.defineSymbolsList);
                }
            }

            GUILayout.EndHorizontal();

            for (int j = 0; j < window.defineSymbolsList.Count; j++)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label(j.ToString(), GUILayout.MaxWidth(20.0f));

                window.defineSymbolsList[j] = EditorGUILayout.TextField(window.defineSymbolsList[j].ToUpper(), GUILayout.MaxWidth(500.0f));

                if (GUILayout.Button("-", GUILayout.MaxWidth(40.0f)))
                {
                    window.defineSymbolsList.RemoveAt(j);
                }

                GUILayout.EndHorizontal();
            }


            if (GUILayout.Button("+", GUILayout.Width(50)))
            {
                window.defineSymbolsList.Add("");
            }

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Apply Changes", GUILayout.Width(200)))
            {
                window.ApplyAllChangesToPlayerSettings(window.currentSelectedPlatform, defineSymbolsList);
            }


            if (GUILayout.Button("Open Player Settings Menu", GUILayout.Width(300)))
            {
                EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player");
            }

            GUILayout.EndHorizontal();
        }

        public static void GetAllSymbolsFromPlayerSettings(BuildTargetGroup targetGroup, ref List<string> definedList)
        {
            definedList.Clear();

            string scriptingDeineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

            char[] stringSeparators = {';'};

            string[] symbolsStringArray = scriptingDeineSymbols.Split(stringSeparators, StringSplitOptions.None);

            for (int i = 0; i < symbolsStringArray.Length; i++)
            {
                if (symbolsStringArray[i] != string.Empty)

                {
                    definedList.Add(symbolsStringArray[i]);
                }
            }
        }

        public void ApplyAllChangesToPlayerSettings(BuildTargetGroup targetGroup, List<string> definedList)
        {
            string defineSymbols = "";

            for (int i = 0; i < definedList.Count; i++)
            {
                defineSymbols += definedList[i] + ",";
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defineSymbols);
        }

        /// <summary>
        ///     Add define symbols as soon as Unity gets done compiling.
        /// </summary>
        public static void Add(string define, bool applyToAll = true)
        {
            foreach (BuildTargetGroup buildTargetGroup in targetGroup)
            {
                if (!applyToAll && buildTargetGroup != EditorUserBuildSettings.selectedBuildTargetGroup)
                {
                    continue;
                }
                
                string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
                List<string> allDefines = definesString.Split(';').ToList();
                if (!allDefines.Contains(define))
                {
                    allDefines.Add(define);
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", allDefines.ToArray()));
                }
            }
        }

        public static void Remove(string define, bool applyToAll = true)
        {
            foreach (BuildTargetGroup buildTargetGroup in targetGroup)
            {
                if (!applyToAll && buildTargetGroup != EditorUserBuildSettings.selectedBuildTargetGroup)
                {
                    continue;
                }
                
                string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
                List<string> allDefines = definesString.Split(';').ToList();
                if (allDefines.Contains(define))
                {
                    allDefines.Remove(define);
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", allDefines.ToArray()));
                }
            }
        }
    }
}