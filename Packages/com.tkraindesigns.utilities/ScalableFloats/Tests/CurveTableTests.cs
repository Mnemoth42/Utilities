using NUnit.Framework;
using UnityEngine;

namespace TkrainDesigns.ScalableFloats.Tests
{
    public class CurveTableTests
    {
        [Test]
        public void Indexer_ReturnsCorrectEntry()
        {
            var table = ScriptableObject.CreateInstance<CurveTable>();
            
            // Use reflection to set private 'entries' field
            var entriesField = typeof(CurveTable).GetField("entries", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var entry = new CurveTableEntry { name = "TestCurve", curve = new AnimationCurve() };
            entriesField.SetValue(table, new[] { entry });

            // Test string indexer
            Assert.AreEqual("TestCurve", table["TestCurve"].name);
            
            // Test int indexer
            Assert.AreEqual("TestCurve", table[0].name);
            
            // Test Count
            Assert.AreEqual(1, table.Count);
        }
    }
}
