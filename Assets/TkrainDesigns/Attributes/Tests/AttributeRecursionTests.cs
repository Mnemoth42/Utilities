using NUnit.Framework;
using UnityEngine;
using TkrainDesigns.Attributes;
using System.Collections.Generic;

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
                new AttributeDependency { SourceAttribute = attrB }
            });

            Assert.IsFalse(attrA.HasCircularDependency());
        }

        [Test]
        public void DirectCircularDependency_ReturnsTrue()
        {
            var attrA = ScriptableObject.CreateInstance<AttributeSO>();
            
            attrA.SetDependencies(new[]
            {
                new AttributeDependency { SourceAttribute = attrA }
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
                new AttributeDependency { SourceAttribute = attrB }
            });
            attrB.SetDependencies(new[]
            {
                new AttributeDependency { SourceAttribute = attrC }
            });
            attrC.SetDependencies(new[]
            {
                new AttributeDependency { SourceAttribute = attrA }
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
                new AttributeDependency { SourceAttribute = attrB },
                new AttributeDependency { SourceAttribute = attrC }
            });
            attrB.SetDependencies(new[]
            {
                new AttributeDependency { SourceAttribute = attrC }
            });

            Assert.IsFalse(attrA.HasCircularDependency());
        }
    }
}