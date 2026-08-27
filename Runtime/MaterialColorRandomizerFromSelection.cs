using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MaterialColorRandomizerFromSelection : MonoBehaviour
{
    [SerializeField] private List<Color> colors;
    [SerializeField] private List<Renderer> renderers;
    
    private void Awake()
    {
        foreach (var r in renderers)
        {
            Color c = colors.Random();
            //Logger.Log("Setting Customer " + gameObject.name + " color to " + c.r + " " + c.g + " " + c.b, LogChannel.UTILITY_SCRIPTS);
            r.material.color = c;
        }
    }

    // Unity doesn't automatically Garbage Collect Renderer.material so we have to manually destroy it
    private void OnDestroy()
    {
        foreach (var r in renderers)
        {
            Destroy(r.material);
        }
    }
}
