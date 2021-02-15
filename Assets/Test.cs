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
    }
}
