using NUnit.Framework;
using UnityEngine;

namespace TkrainDesigns.Attributes.Tests
{
    public class AttributeRecursionTests
    {
        [Test]
        public void NoCircularDependency_ReturnsFalse()
        {
            var attrA = ScriptableObject.CreateInstance<AttributeSO>();
            var attrB = ScriptableObject.CreateInstance<AttributeSO>();

            attrA.SetDependencies(new[]
            {
                new AttributeDependency(attrB, AttributeModifierType.Additive, default)
            });

            Assert.IsFalse(attrA.HasCircularDependency());
        }

        [Test]
        public void DirectCircularDependency_ReturnsTrue()
        {
            var attrA = ScriptableObject.CreateInstance<AttributeSO>();
            
            attrA.SetDependencies(new[]
            {
                new AttributeDependency(attrA, AttributeModifierType.Additive, default)
            });

            Assert.IsTrue(attrA.HasCircularDependency());
        }

        [Test]
        public void IndirectCircularDependency_ReturnsTrue()
        {
            var attrA = ScriptableObject.CreateInstance<AttributeSO>();
            var attrB = ScriptableObject.CreateInstance<AttributeSO>();
            var attrC = ScriptableObject.CreateInstance<AttributeSO>();

            attrA.SetDependencies(new[]
            {
                new AttributeDependency(attrB, AttributeModifierType.Additive, default)
            });
            attrB.SetDependencies(new[]
            {
                new AttributeDependency(attrC, AttributeModifierType.Additive, default)
            });
            attrC.SetDependencies(new[]
            {
                new AttributeDependency(attrA, AttributeModifierType.Additive, default)
            });

            Assert.IsTrue(attrA.HasCircularDependency());
        }

        [Test]
        public void ComplexNoCircularDependency_ReturnsFalse()
        {
            var attrA = ScriptableObject.CreateInstance<AttributeSO>();
            var attrB = ScriptableObject.CreateInstance<AttributeSO>();
            var attrC = ScriptableObject.CreateInstance<AttributeSO>();

            // A depends on B and C
            // B depends on C
            // This is a Directed Acyclic Graph (DAG), not circular
            attrA.SetDependencies(new[]
            {
                new AttributeDependency(attrB, AttributeModifierType.Additive, default),
                new AttributeDependency(attrC, AttributeModifierType.Additive, default)
            });
            attrB.SetDependencies(new[]
            {
                new AttributeDependency(attrC, AttributeModifierType.Additive, default)
            });

            Assert.IsFalse(attrA.HasCircularDependency());
        }
    }
}