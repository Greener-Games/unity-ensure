using System.Collections;
using System.Collections.Generic;
using GG.UnityEnsure;
using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    [InitializeOnLoadMethod]
    static void EnsureScriptingDefineSymbol()
    {
        EnsureUpmScope.EnsurePackageScope("http://Test", "Test2","hello");
        EnsureUpmScope.EnsurePackageScope("http://Test2", "Test2","hello2");
        EnsureUpmScope.EnsurePackageScope("http://Test2", new List<string>(){"Test2","Test3"},"waffles");
    }
}
