using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TkrainDesigns.Extensions.Editors
{
    public static class EditorExtensions
    {
        public static VisualElement CreateSection(this Editor editor, string title)
        {
            var section = new VisualElement();
            section.style.marginTop = 5;
            section.style.marginBottom = 5;

            var titleLabel = new Label(title);
            titleLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            titleLabel.style.fontSize = 12;
            titleLabel.style.marginBottom = 3;
            titleLabel.style.marginTop = 3;
            section.Add(titleLabel);

            var box = new Box();
            box.style.paddingLeft = 5;
            box.style.paddingRight = 5;
            box.style.paddingTop = 5;
            box.style.paddingBottom = 5;
            box.style.marginLeft = 3;
            section.Add(box);

            return box;
        }

        public static PropertyField CreatePropertyField(this Editor editor, string propertyName)
        {
            var property = editor.GetProperty(propertyName);
            var field = new PropertyField(property);
            field.Bind(editor.serializedObject);
            return field;
        }

        public static IntegerField CreateIntegerField(this Editor editor, string propertyName)
        {
            var property = editor.GetProperty(propertyName);
            var field = new IntegerField();
            field.BindProperty(property);
            return field;
        }

        public static Slider CreateSlider(this Editor editor, string propertyName, float lowValue = 0, float highValue = 100)
        {
            var property = editor.GetProperty(propertyName);
            var slider = new Slider();
            slider.lowValue = lowValue;
            slider.highValue = highValue;
            slider.label = property.displayName;
            slider.showInputField = true;
            slider.BindProperty(property);
            return slider;
        }

        /// <summary>
        /// Finds a serialized property by name, falling back to the auto-property backing field.
        /// </summary>
        public static SerializedProperty GetProperty(this Editor editor, string propertyName)
        {
            var property = editor.serializedObject.FindProperty(propertyName);
            if (property == null)
            {
                property = editor.serializedObject.FindProperty($"<{propertyName}>k__BackingField");
            }
            return property;
        }

        public static SliderInt CreateSliderInt(this Editor editor, string propertyName, int lowValue = 1, int highValue = 100)
        {
            var property = editor.GetProperty(propertyName);
            var slider = new SliderInt();
            slider.lowValue = lowValue;
            slider.highValue = highValue;
            slider.label = property.displayName;
            slider.showInputField = true;
            slider.BindProperty(property);
            return slider;
        }
    }
}
