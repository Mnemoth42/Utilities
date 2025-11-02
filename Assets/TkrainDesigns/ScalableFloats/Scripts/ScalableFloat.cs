using UnityEngine;

namespace TkrainDesigns.ScalableFloats
{
    [System.Serializable]
    public struct ScalableFloat
    {
        [field: SerializeField] public float Value { get; private set; } 
        [field: SerializeField] public AnimationCurve Curve { get; private set; }
        
        
        public ScalableFloat(float value, AnimationCurve curve)
        {
            Value = value;
            Curve = curve;
        }
        
        public float Evaluate(float level) => Curve.Evaluate(level) * Value;
        
        public int EvaluateInt(float level) => Mathf.RoundToInt(Evaluate(level));
        
        public bool EvaluateBool(float level) => Evaluate(level) > 0;
        
    }
}