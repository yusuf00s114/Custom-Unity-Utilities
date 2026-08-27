using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[Tooltip("First, picks a random color between minRGBValues and maxRGBValues.\n" +
         "Then, rounds the values of the RGB. The precision is determined by the 'precision' parameter. \n" +
         "Finally, picks a random value between minHSV_v and maxHSV_v, converts the color to HSV space, assigns the V component" +
         "to that value, and converts the color back to RGB space.")]
public class MaterialColorRandomizer : MonoBehaviour
{
    [SerializeField] Color minRGBValues = new Color(0f, 0f, 0f, 1f);
    [SerializeField] Color maxRGBValues = new Color(1f, 1f, 1f, 1f);
    [Tooltip("To how many decimals should the color values be rounded after being chosen at random.")]
    [SerializeField] private int precision = 2;
    [Tooltip("Minimum value of the value parameter of the color in HSV space")]
    [SerializeField] private float minHSV_v = 0.7f;
    [Tooltip("Maximum value of the value parameter of the color in HSV space")]
    [SerializeField] private float maxHSV_v = 1f;
    [SerializeField] List<Renderer> renderers;

    private void Awake()
    {
        foreach (var r in renderers)
        {
            Color c = new Color(
                MathF.Round(Random.Range(minRGBValues.r, maxRGBValues.r), precision),
                MathF.Round(Random.Range(minRGBValues.g, maxRGBValues.g), precision),
                MathF.Round(Random.Range(minRGBValues.b, maxRGBValues.b), precision),
                MathF.Round(Random.Range(minRGBValues.a, maxRGBValues.a), precision)
            );
            float randomHSV_v =  Random.Range(minHSV_v, maxHSV_v);
            Color.RGBToHSV(c, out float h, out float s, out float v);
            v = randomHSV_v;
            c = Color.HSVToRGB(h, s, v);
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
