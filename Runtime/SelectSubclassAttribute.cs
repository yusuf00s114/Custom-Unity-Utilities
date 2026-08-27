using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class SelectSubclassAttribute : PropertyAttribute
{
    public SelectSubclassAttribute(bool includeBase = false, params Type[] extraTypes)
    {
        IncludeBase = includeBase;
        ExtraTypes = extraTypes;
    }

    public bool IncludeBase { get; }
    public Type[] ExtraTypes { get; }
}