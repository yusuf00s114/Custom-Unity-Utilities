// Used for creating a dropdown when serializing types in the inspector
// Created by using Google Gemini and Claude.

using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(SelectSubclassAttribute))]
public class SubclassSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.ManagedReference)
        {
            EditorGUI.LabelField(position, "Use [SelectSubclassAttribute] only with [SerializeReference]");
            return;
        }

        // 1. Determine the base type of the field (e.g., ICustomerBehavior)
        Type fieldType = fieldInfo.FieldType.IsGenericType
            ? fieldInfo.FieldType.GetGenericArguments()[0]
            : fieldInfo.FieldType;

        // 2. Get the extra types from the attribute arguments
        var attr = (SelectSubclassAttribute)attribute;

        // 3. Gather all concrete implementations of the field type,
        //    filtered to only those assignable to at least one ExtraType (if any were given).
        var implementations = TypeCache.GetTypesDerivedFrom(fieldType)
        .Where(t => !t.IsAbstract && !t.IsInterface)
        .Where(t => attr.ExtraTypes == null
                 || attr.ExtraTypes.Length == 0
                 || attr.ExtraTypes.Any(et => et.IsAssignableFrom(t)))
        .Distinct()
        .OrderBy(t => t.Name)
        .ToList();

        // Prepend the base type itself if requested and it's instantiable
        if (attr.IncludeBase && !fieldType.IsAbstract && !fieldType.IsInterface)
            implementations.Insert(0, fieldType);

        string[] typeNames = implementations.Select(t => t.Name).Prepend("None").ToArray();

        string currentTypeName = property.managedReferenceFullTypename.Split(' ').Last();
        int currentIndex = implementations.FindIndex(t => t.FullName == currentTypeName) + 1;

        Rect dropdownRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        int newIndex = EditorGUI.Popup(dropdownRect, label.text, currentIndex, typeNames);

        if (newIndex != currentIndex)
        {
            property.managedReferenceValue = newIndex == 0
                ? null
                : Activator.CreateInstance(implementations[newIndex - 1]);
        }

        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true);
    }
}