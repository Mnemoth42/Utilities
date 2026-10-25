using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TkrainDesigns.Inventories.Editors
{
    [CustomEditor(typeof(InventoryItem), true)]
    public class InventoryItemEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            // Add spacing
            root.style.paddingTop = 5;
            root.style.paddingBottom = 5;

            // Identity Section
            var identitySection = CreateSection("Identity");
            var uniqueIdField = CreatePropertyField("UniqueId");
            uniqueIdField.style.unityFontStyleAndWeight = FontStyle.Bold;
            uniqueIdField.SetEnabled(false);
            identitySection.Add(uniqueIdField);
            identitySection.Add(CreatePropertyField("Name"));
            identitySection.Add(CreatePropertyField("Description"));
            root.Add(identitySection);

            // Visual Section
            var visualSection = CreateSection("Visual");
            var iconContainer = new VisualElement();
            iconContainer.style.flexDirection = FlexDirection.Row;
            iconContainer.style.alignItems = Align.Center;
            iconContainer.style.marginBottom = 10;
            
            var iconImage = new Image();
            iconImage.style.width = 64;
            iconImage.style.height = 64;
            iconImage.style.marginRight = 10;
            iconImage.style.borderBottomWidth = 1;
            iconImage.style.borderTopWidth = 1;
            iconImage.style.borderLeftWidth = 1;
            iconImage.style.borderRightWidth = 1;
            iconImage.style.borderBottomColor = Color.gray;
            iconImage.style.borderTopColor = Color.gray;
            iconImage.style.borderLeftColor = Color.gray;
            iconImage.style.borderRightColor = Color.gray;
            
            var iconProperty = GetProperty("Icon");
            iconImage.TrackPropertyValue(iconProperty, (p) => {
                iconImage.image = (p.objectReferenceValue as Sprite)?.texture;
            });
            iconImage.image = (iconProperty.objectReferenceValue as Sprite)?.texture;
            
            iconContainer.Add(iconImage);
            
            var iconFields = new VisualElement();
            iconFields.style.flexGrow = 1;
            iconFields.Add(CreatePropertyField("Icon"));
            iconFields.Add(CreatePropertyField("Pickup"));
            iconContainer.Add(iconFields);
            
            visualSection.Add(iconContainer);
            root.Add(visualSection);

            // Properties Section
            var propertiesSection = CreateSection("Properties");
            var isStackable = new Label("Stackable");
            isStackable.SetEnabled(false);
            var sliderInt = CreateSliderInt("StackSize");
            isStackable.TrackPropertyValue(GetProperty("StackSize"), (p)=> isStackable.text = p.intValue > 1?"Not Stackable":"Stackable");
            isStackable.text = GetProperty("StackSize").intValue > 1?"Not Stackable":"Stackable";
            propertiesSection.Add(sliderInt);
            propertiesSection.Add(isStackable);
            propertiesSection.Add(CreateSlider("BasePrice", 1, 1000));
            propertiesSection.Add(CreateSlider("BaseWeight", 0, 100));
            propertiesSection.Add(CreatePropertyField("IsConsumable"));
            root.Add(propertiesSection);

            // Add Script field at the bottom
            var scriptField = new PropertyField(serializedObject.FindProperty("m_Script"));
            scriptField.SetEnabled(false);
            scriptField.style.marginTop = 10;
            root.Add(scriptField);

            return root;
        }

        protected VisualElement CreateSection(string title)
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

        protected PropertyField CreatePropertyField(string propertyName)
        {
            var property = serializedObject.FindProperty(propertyName);
            if (property == null)
            {
                property = serializedObject.FindProperty($"<{propertyName}>k__BackingField");
            }

            var field = new PropertyField(property);
            field.Bind(serializedObject);
            return field;
        }

        protected IntegerField CreateIntegerField(string propertyName)
        {
            var property = serializedObject.FindProperty(propertyName);
            if (property == null)
            {
                property = serializedObject.FindProperty($"<{propertyName}>k__BackingField");
            }
            
            var field = new IntegerField();
            field.BindProperty(property);
            return field;
        }

        protected Slider CreateSlider(string propertyName, float lowValue = 0, float highValue = 100)
        {
            var property = GetProperty(propertyName);
            var slider = new Slider();
            slider.lowValue = lowValue;
            slider.highValue = highValue;
            slider.label = property.displayName;
            slider.showInputField = true;
            slider.BindProperty(property);
            return slider;
        }

        private SerializedProperty GetProperty(string propertyName)
        {
            var property = serializedObject.FindProperty(propertyName);
            if (property == null)
            {
                property = serializedObject.FindProperty($"<{propertyName}>k__BackingField");
            }
            return property;
        }

        protected SliderInt CreateSliderInt(string propertyName, int lowValue = 1, int highValue = 100)
        {
            var property = GetProperty(propertyName);
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
