using System.Collections.Generic;
using UnityEngine;

namespace TkrainDesigns.ScalableFloats
{

    [System.Serializable]
    public struct CurveTableEntry
    {
        public string name;
        public AnimationCurve curve;

        public CurveTableEntry(string name = "")
        {
            this.name = name;
            curve = new AnimationCurve();
            curve.AddKey(0, 0);
            curve.AddKey(100, 100);
        }
}
    
    [CreateAssetMenu(fileName = "Curve Table", menuName = "Tables|Curve Table", order = 0)]
    public class CurveTable : ScriptableObject
    {
        [SerializeField] CurveTableEntry[] entries;

        private Dictionary<string, CurveTableEntry> table;

        private void BuildLookup()
        {
            if (table == null)
            {
                table = new Dictionary<string, CurveTableEntry>();
                foreach (var entry in entries)
                {
                    table.Add(entry.name, entry);
                }
            }
        }
        
        public CurveTableEntry this[string key]
        {
            get
            {
                BuildLookup();
                return !table.TryGetValue(key, out var value) ? new CurveTableEntry(""): value;
            }
        }

        public CurveTableEntry this[int index]
        {
            get => entries[index];
        }

        public int Count => entries?.Length ?? 0;

        public IEnumerable<string> GetKeys()
        {
            BuildLookup();
            return table.Keys;
        }
        
        
    }
}