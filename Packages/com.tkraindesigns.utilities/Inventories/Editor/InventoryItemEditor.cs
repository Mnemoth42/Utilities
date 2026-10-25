using TkrainDesigns.Extensions.Editors;
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
            var identitySection = this.CreateSection("Identity");
            var uniqueIdField = this.CreatePropertyField("UniqueId");
            uniqueIdField.style.unityFontStyleAndWeight = FontStyle.Bold;
            uniqueIdField.SetEnabled(false);
            identitySection.Add(uniqueIdField);
            identitySection.Add(this.CreatePropertyField("Name"));
            identitySection.Add(this.CreatePropertyField("Description"));
            root.Add(identitySection);

            // Visual Section
            var visualSection = this.CreateSection("Visual");
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
            
            var iconProperty = this.GetProperty("Icon");
            iconImage.TrackPropertyValue(iconProperty, (p) => {
                iconImage.image = (p.objectReferenceValue as Sprite)?.texture;
            });
            iconImage.image = (iconProperty.objectReferenceValue as Sprite)?.texture;
            
            iconContainer.Add(iconImage);
            
            var iconFields = new VisualElement();
            iconFields.style.flexGrow = 1;
            iconFields.Add(this.CreatePropertyField("Icon"));
            iconFields.Add(this.CreatePropertyField("Pickup"));
            iconContainer.Add(iconFields);
            
            visualSection.Add(iconContainer);
            root.Add(visualSection);

            // Properties Section
            var propertiesSection = this.CreateSection("Properties");
            var isStackable = new Label("Stackable");
            isStackable.SetEnabled(false);
            var sliderInt = this.CreateSliderInt("StackSize");
            isStackable.TrackPropertyValue(this.GetProperty("StackSize"), (p)=> isStackable.text = p.intValue > 1?"Not Stackable":"Stackable");
            isStackable.text = this.GetProperty("StackSize").intValue > 1?"Not Stackable":"Stackable";
            propertiesSection.Add(sliderInt);
            propertiesSection.Add(isStackable);
            propertiesSection.Add(this.CreateSlider("BasePrice", 1, 1000));
            propertiesSection.Add(this.CreateSlider("BaseWeight", 0, 100));
            propertiesSection.Add(this.CreatePropertyField("IsConsumable"));
            root.Add(propertiesSection);

            // Add Script field at the bottom
            var scriptField = new PropertyField(serializedObject.FindProperty("m_Script"));
            scriptField.SetEnabled(false);
            scriptField.style.marginTop = 10;
            root.Add(scriptField);

            return root;
        }
    }
}
