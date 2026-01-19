using System;
using System.Collections.Generic;
using TkrainDesigns.Core.TkrainDesigns.Core;
using UnityEngine;

namespace TkrainDesigns.Attributes
{
    
    
    public class AttributeContainer : MonoBehaviour, IAttributeBaseContext, IAttributeModifierContext, ILevelProvider
    {
        [SerializeField] private AttributeSO[] attributes;
        private Dictionary<AttributeSO, float> baseValues = new Dictionary<AttributeSO, float>();
        private Dictionary<AttributeSO, List<AttributeModifier>> modifiers = new Dictionary<AttributeSO, List<AttributeModifier>>();

        [field: SerializeField] public int Level { get; private set; } = 1;
        
        
        public event System.Action<int> OnLevelChanged;
        
        
        public float GetBaseValue(AttributeSO attribute)
        {
            if (baseValues.TryGetValue(attribute, out float baseValue))
            {
                return baseValue;
            }
            return 0f;
        }

        public float GetAttributeValue(AttributeSO attribute)
        {
            return (GetBaseValue(attribute) + attribute.GetAdditiveModifiers(this, attribute)) * attribute.GetPercentageMultipliers(this, attribute);   
        }

        public event Action<AttributeSO, float> OnAttributeChanged;


        public IEnumerable<AttributeModifier> GetAttributeModifiers(AttributeSO attribute)
        {
            if (modifiers.TryGetValue(attribute, out List<AttributeModifier> attributeModifiers))
            {
                return attributeModifiers;
            }
            return new List<AttributeModifier>();
        }

        public void SetLevel(int level)
        {
            Level = level;
            OnLevelChanged?.Invoke(level);
            CalculateBaseValues();
        }

        private void CalculateBaseValues()
        {
            baseValues.Clear();
            foreach (AttributeSO attribute in attributes) CalculateBaseValue(attribute);
        }

        private void CalculateBaseValue(AttributeSO attribute)
        {
            if (baseValues.ContainsKey(attribute)) return;
            foreach (AttributeSO attributeSO in attribute.GetDependencies())
            {
                if (!baseValues.ContainsKey(attributeSO)) CalculateBaseValue(attributeSO);
            }
            baseValues[attribute] = attribute.CalculateBaseValue(Level, this);
            AnnounceAttributeChange(attribute);
        }
        
        private void AnnounceAttributeChange(AttributeSO attribute)
        {
            OnAttributeChanged?.Invoke(attribute, GetAttributeValue(attribute));
        }
        
    }
}