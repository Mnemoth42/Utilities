using UnityEditor;
using UnityEngine;

namespace TkrainDesigns.ScalableFloats
{
    [CustomPropertyDrawer(typeof(ScalableFloat))]
    public class ScalableFloatDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
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
            SerializedProperty curveProp = property.FindPropertyRelative("<Curve>k__BackingField");
            
            // Draw fields
            EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
            EditorGUI.PropertyField(curveRect, curveProp, GUIContent.none);
            
            // Restore indent
            EditorGUI.indentLevel = indent;
            
            EditorGUI.EndProperty();
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}