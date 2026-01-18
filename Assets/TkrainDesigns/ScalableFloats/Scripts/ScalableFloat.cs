using UnityEngine;

namespace TkrainDesigns.ScalableFloats
{
    [System.Serializable]
    public struct ScalableFloat
    {
        [field: SerializeField] public float Value { get; private set; } 
        
        [SerializeField] private CurveTable curveTable;
        [SerializeField] private string curveName;
        


        public float Evaluate(float level)
        {
            if(curveTable == null) return Value;
            var curve = curveTable[curveName];
            return curve.curve.Evaluate(level) * Value;

        }
        
        public int EvaluateInt(float level) => Mathf.RoundToInt(Evaluate(level));
        
        public bool EvaluateBool(float level) => Evaluate(level) > 0;
        
    }
}