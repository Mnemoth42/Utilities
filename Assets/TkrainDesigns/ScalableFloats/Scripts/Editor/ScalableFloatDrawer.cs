using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TkrainDesigns.ScalableFloats.Editors
{
    [CustomPropertyDrawer(typeof(ScalableFloat))]
    public class ScalableFloatDrawer : PropertyDrawer
    {
        private int preview = 1;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ScalableFloat scalableFloat = GetPropertyValue<ScalableFloat>(property);
            
            EditorGUI.BeginProperty(position, label, property);
            
            position.height = EditorGUIUtility.singleLineHeight;
            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            
            // Don't indent child fields
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            
            // Calculate rects
            float valueWidth = position.width * 0.3f;
            float curveWidth = position.width * 0.7f - 5f;
            
            Rect valueRect = new Rect(position.x, position.y, valueWidth, position.height);
            Rect curveRect = new Rect(position.x + valueWidth + 5f, position.y, curveWidth, position.height);
            
            // Get properties
            SerializedProperty valueProp = property.FindPropertyRelative("<Value>k__BackingField");
            SerializedProperty curveTable = property.FindPropertyRelative("curveTable");
            SerializedProperty curveName = property.FindPropertyRelative("curveName");
            
            // Draw fields
            EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
            EditorGUI.PropertyField(curveRect, curveTable, GUIContent.none);
            valueRect.y += EditorGUIUtility.singleLineHeight;
            curveRect.y += EditorGUIUtility.singleLineHeight;
            if (curveTable.objectReferenceValue != null && curveTable.objectReferenceValue is CurveTable table)
            {
                List<string> keys = table.GetKeys().ToList();
                int index = keys.IndexOf(curveName.stringValue);
                if(index < 0) index = 0;
                index = EditorGUI.Popup(valueRect, index, keys.ToArray());
                curveName.stringValue = keys[index];
                curveRect.width /= 3f;
                EditorGUI.LabelField(curveRect, "Preview:");
                curveRect.x += curveRect.width;
                preview = EditorGUI.IntField(curveRect, preview);
                curveRect.x += curveRect.width;
                EditorGUI.LabelField(curveRect, $" = {scalableFloat.Evaluate(preview):F2}");
                //EditorGUI.LabelField(curveRect, $"1={scalableFloat.Evaluate(1):F2}, 10={scalableFloat.Evaluate(10):F2}, 20 = {scalableFloat.Evaluate(20):F2}");
            }
            
            
            // Restore indent
            EditorGUI.indentLevel = indent;
            
            EditorGUI.EndProperty();
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.FindPropertyRelative("curveTable").objectReferenceValue != null
                ? EditorGUIUtility.singleLineHeight * 2
                : EditorGUIUtility.singleLineHeight;
        }

        private T GetPropertyValue<T>(SerializedProperty property)
        {
            object obj = property.serializedObject.targetObject;
            string[] path = property.propertyPath.Split('.');
            foreach (string part in path)
            {
                obj = GetField(obj, part);
            }
            return (T)obj;
        }

        private object GetField(object obj, string name)
        {
            if (obj == null) return null;
            
            // Handle [field: SerializeField] backing fields
            string backingFieldName = $"<{name}>k__BackingField";
            
            Type type = obj.GetType();
            FieldInfo field = type.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null)
            {
                field = type.GetField(backingFieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            }
            
            if (field == null && name.EndsWith("]"))
            {
                // Handle array/list
                int start = name.IndexOf("[");
                int end = name.IndexOf("]");
                string arrayName = name.Substring(0, start);
                int index = int.Parse(name.Substring(start + 1, end - start - 1));
                
                // Unity property path for arrays is "arrayFieldName.Array.data[index]"
                // But Split('.') might have simplified it or we need to handle "Array" and "data[index]"
                // Actually Unity property path is usually "fieldName.Array.data[index]"
                // My Split('.') will give ["fieldName", "Array", "data[0]"]
                
                field = type.GetField(arrayName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null)
                {
                    var array = field.GetValue(obj) as System.Collections.IList;
                    if (array != null && index < array.Count) return array[index];
                }
            }
            
            if (field != null) return field.GetValue(obj);
            
            // Handle "Array" and "data[index]" parts of Unity property path
            if (name == "Array") return obj;
            if (name.StartsWith("data["))
            {
                int start = name.IndexOf("[");
                int end = name.IndexOf("]");
                int index = int.Parse(name.Substring(start + 1, end - start - 1));
                var array = obj as System.Collections.IList;
                if (array != null && index < array.Count) return array[index];
            }

            return null;
        }
    }
}