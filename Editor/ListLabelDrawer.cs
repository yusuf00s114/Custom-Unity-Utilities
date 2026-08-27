using UnityEditor;
using UnityEngine;

// Created by Gemini

[CustomPropertyDrawer(typeof(ListLabelAttribute))]
public class ListLabelDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ListLabelAttribute listLabel = attribute as ListLabelAttribute;

        // Find the specific property ("day") inside your serialized class (DailyData)
        SerializedProperty targetProp = property.FindPropertyRelative(listLabel.PropertyName);

        if (targetProp != null)
        {
            string valueStr = "";

            // Handle different data types just in case you reuse this later
            switch (targetProp.propertyType)
            {
                case SerializedPropertyType.Integer:
                    valueStr = targetProp.intValue.ToString();
                    break;
                case SerializedPropertyType.String:
                    valueStr = targetProp.stringValue;
                    break;
                case SerializedPropertyType.Float:
                    valueStr = targetProp.floatValue.ToString();
                    break;
                case SerializedPropertyType.Enum:
                    valueStr = targetProp.enumDisplayNames[targetProp.enumValueIndex];
                    break;
                default:
                    valueStr = "Unsupported Type";
                    break;
            }

            // Replace "Element X" with your custom string (e.g., "Day 1")
            label.text = $"{listLabel.Prefix}{valueStr}";
        }

        // Draw the default property field, but with the modified label
        EditorGUI.PropertyField(position, property, label, true);
    }

    // Required so that lists with foldouts and multiple fields size correctly
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true);
    }
}