using System.Collections.Generic;
using System.Linq;
using TkrainDesigns.ScalableFloats;
using UnityEngine;

namespace TkrainDesigns.Attributes
{
    public enum AttributeModifierType
    {
        Additive,
        Multiplicative
    }

    [System.Serializable]
    public class AttributeModifier
    {
        [field: SerializeField] public AttributeModifierType ModifierType { get; private set; } = AttributeModifierType.Additive;
        [field: SerializeField] public float Value { get; private set; } = 0;
    }
    
    [System.Serializable]
    public class AttributeDependency
    {
        [field: SerializeField] public AttributeSO SourceAttribute { get; private set; }
        [field: SerializeField] public AttributeModifierType ModifierType { get; private set; }
        [field: SerializeField] public ScalableFloat Modifier { get; private set; }

        public AttributeDependency(AttributeSO sourceAttribute, AttributeModifierType modifierType, ScalableFloat modifier)
        {
            SourceAttribute = sourceAttribute;
            ModifierType = modifierType;
            Modifier = modifier;
        }
    }
    
    [CreateAssetMenu(fileName = "New Attribute", menuName = "TkrainDesigns/Attribute", order = 0)]
    // ReSharper disable once InconsistentNaming
    public class AttributeSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }= "";
        [field: SerializeField] public string Description { get; private set; }= "";
        [SerializeField] private ScalableFloat defaultValue;
        [SerializeField] private AttributeDependency[] dependencies;

        public void SetDependencies(AttributeDependency[] newDependencies)
        {
            dependencies = newDependencies;
        }
        
        public float CalculateBaseValue(float level, IAttributeBaseContext context)
        {
            float baseValue = defaultValue.Evaluate(level);
            foreach (var dependency in dependencies)
            {
                if (dependency.SourceAttribute == null)
                {
                    Debug.LogWarning($"Attribute {Name} has a null dependency.");
                    continue;
                }

                if (dependency.ModifierType == AttributeModifierType.Additive)
                {
                    float sourceValue = context.GetBaseValue(dependency.SourceAttribute);
                    baseValue += sourceValue * dependency.Modifier.Evaluate(level);
                    baseValue += GetAdditiveModifiers(context, dependency.SourceAttribute);
                }
            }

            float multipliers = 100;
            foreach (var dependency in dependencies)
            {
                if (dependency.SourceAttribute == null) continue;
                if (dependency.ModifierType == AttributeModifierType.Multiplicative)
                {
                    float sourceValue = context.GetBaseValue(dependency.SourceAttribute);
                    multipliers += sourceValue * dependency.Modifier.Evaluate(level);
                    multipliers += GetPercentageMultipliers(context, dependency.SourceAttribute);
                }
            }
            baseValue *= multipliers / 100;
            return baseValue;
        }

        public float GetAdditiveModifiers(IAttributeBaseContext context, AttributeSO attribute)
        {
            float result = 0;
            foreach (IAttributeModifierContext modifierContext in context.gameObject.GetComponents<IAttributeModifierContext>())
            {
                foreach (var attributeModifier in modifierContext.GetAttributeModifiers(attribute))
                {
                    if (attributeModifier.ModifierType == AttributeModifierType.Additive)
                    {
                        result += attributeModifier.Value;
                    }
                }
            }
            return result;
        }

        public float GetPercentageMultipliers(IAttributeBaseContext context, AttributeSO attribute)
        {
            float result = 0;
            foreach (IAttributeModifierContext modifierContext in context.gameObject.GetComponents<IAttributeModifierContext>())
            {
                foreach (AttributeModifier attributeModifier in modifierContext.GetAttributeModifiers(attribute))
                {
                    if (attributeModifier.ModifierType == AttributeModifierType.Multiplicative)
                    {
                        result += attributeModifier.Value;
                    }
                }
            }
            return result;
        }

        public IEnumerable<AttributeSO> GetDependencies()
        {
            if (dependencies == null) return Enumerable.Empty<AttributeSO>();
            return dependencies.Where(d => d.SourceAttribute != null).Select(dependency => dependency.SourceAttribute);
        }

        public bool HasCircularDependency()
        {
            HashSet<AttributeSO> visited = new HashSet<AttributeSO>();
            HashSet<AttributeSO> currentPath = new HashSet<AttributeSO>();
            return CheckRecursion(this, visited, currentPath);
        }

        private bool CheckRecursion(AttributeSO current, HashSet<AttributeSO> visited, HashSet<AttributeSO> currentPath)
        {
            if (currentPath.Contains(current)) return true;
            if (visited.Contains(current)) return false;

            visited.Add(current);
            currentPath.Add(current);

            foreach (var dependency in current.GetDependencies())
            {
                if (CheckRecursion(dependency, visited, currentPath)) return true;
            }

            currentPath.Remove(current);
            return false;
        }

        private void OnValidate()
        {
            if (HasCircularDependency())
            {
                Debug.LogError($"Circular dependency detected in Attribute: {Name}", this);
            }
        }
    }
}