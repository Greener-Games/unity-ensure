using System.Collections;
using System.Collections.Generic;
using GG.UnityEnsure.Xr;
using UnityEngine;

public class Tester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EnsureUnityInteractionLayer.EnsureLayers("helloWorld");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
