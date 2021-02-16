using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GG.UnityEnsure
{
    public class DefineSymbolsViewer: EditorWindow
    {
        List<string> defineSymbolsList;
        BuildTargetGroup currentSelectedPlatform;
        static DefineSymbolsViewer _window;

        static readonly List<BuildTargetGroup> TargetGroup = new List<BuildTargetGroup>
        {
            BuildTargetGroup.Standalone,
            BuildTargetGroup.Android,
            BuildTargetGroup.iOS,
            BuildTargetGroup.WebGL,
        };
        
        [MenuItem("Window/Editor Extensions/Define Symbols")]
        public static void Init()
        {
            _window = (DefineSymbolsViewer) GetWindow(typeof(DefineSymbolsViewer), true);

            _window.currentSelectedPlatform = BuildTargetGroup.Standalone;

            _window.defineSymbolsList = new List<string>();

            GetAllSymbolsFromPlayerSettings(_window.currentSelectedPlatform, ref _window.defineSymbolsList);

            _window.Show();
        }

        void OnGUI()
        {
            if (_window == null)
            {
                // sometimes  object instance loses, could n't find reason for this
                Debug.LogError("Something went wrong");
                _window = GetWindow<DefineSymbolsViewer>();
            }

            GUILayout.Label("Add or Remove Scripting Define symbols", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();

            for (int i = 0; i < TargetGroup.Count; i++)
            {
                BuildTargetGroup iterTarget = TargetGroup[i];

                GUIStyle btnStyle = new GUIStyle(GUI.skin.button);

                if (_window.currentSelectedPlatform == iterTarget)
                {
                    btnStyle.normal.background = btnStyle.active.background;
                }

                if (GUILayout.Button(iterTarget.ToString().ToUpper(), btnStyle))
                {
                    GUI.FocusControl("");
                    List<string> getsavedData = new List<string>();

                    GetAllSymbolsFromPlayerSettings(_window.currentSelectedPlatform, ref getsavedData);


                    List<string> list = new List<string>();

                    _window.defineSymbolsList.RemoveAll(x => x == "");

                    if (_window.defineSymbolsList.Count > getsavedData.Count)
                    {
                        list = _window.defineSymbolsList.Except(getsavedData).ToList();
                    }
                    else
                    {
                        list = getsavedData.Except(_window.defineSymbolsList).ToList();
                    }


                    if (list.Count > 0)
                    {
                        bool _event = EditorUtility.DisplayDialog("Unapplied changes", "Unapplied changes for " + _window.currentSelectedPlatform, "Apply", "Revert");

                        if (_event)
                        {
                            _window.ApplyAllChangesToPlayerSettings(_window.currentSelectedPlatform, defineSymbolsList);
                        }
                    }

                    _window.currentSelectedPlatform = iterTarget;

                    GetAllSymbolsFromPlayerSettings(_window.currentSelectedPlatform, ref _window.defineSymbolsList);
                }
            }

            GUILayout.EndHorizontal();

            for (int j = 0; j < _window.defineSymbolsList.Count; j++)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label(j.ToString(), GUILayout.MaxWidth(20.0f));

                _window.defineSymbolsList[j] = EditorGUILayout.TextField(_window.defineSymbolsList[j].ToUpper(), GUILayout.MaxWidth(500.0f));

                if (GUILayout.Button("-", GUILayout.MaxWidth(40.0f)))
                {
                    _window.defineSymbolsList.RemoveAt(j);
                }

                GUILayout.EndHorizontal();
            }


            if (GUILayout.Button("+", GUILayout.Width(50)))
            {
                _window.defineSymbolsList.Add("");
            }

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Apply Changes", GUILayout.Width(200)))
            {
                _window.ApplyAllChangesToPlayerSettings(_window.currentSelectedPlatform, defineSymbolsList);
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
            List<BuildTargetGroup> groups = new List<BuildTargetGroup>();
            if (applyToAll)
            {
                groups = TargetGroup;
            }
            else
            {
                groups.Add(EditorUserBuildSettings.selectedBuildTargetGroup);
            }
                
            Add(define, groups);
        }
        
        /// <summary>
        ///     Add define symbols as soon as Unity gets done compiling.
        /// </summary>
        public static void Add(string define, IEnumerable<BuildTargetGroup> groups)
        {
            foreach (BuildTargetGroup buildTargetGroup in groups)
            {
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
            foreach (BuildTargetGroup buildTargetGroup in TargetGroup)
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