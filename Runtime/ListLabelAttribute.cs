using UnityEngine;

// Created by Gemini

public class ListLabelAttribute : PropertyAttribute
{
    public readonly string PropertyName;
    public readonly string Prefix;

    /// <param name="propertyName">The name of the variable inside the class (e.g., "day")</param>
    /// <param name="prefix">Optional text to put before the variable (e.g., "Day ")</param>
    public ListLabelAttribute(string propertyName, string prefix = "")
    {
        PropertyName = propertyName;
        Prefix = prefix;
    }
}