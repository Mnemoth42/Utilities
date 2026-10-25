using UnityEngine;

namespace TkrainDesigns.ScalableFloats
{
    public class ScaleableFloatTester : MonoBehaviour
    {
        [field: SerializeField] public ScalableFloat float1 { get; private set; }
        [field: SerializeField] public ScalableFloat float2 { get; private set; }

        void Start()
        {
            Debug.Log(float1.Evaluate(10f));
            Debug.Log(float2.Evaluate(10f));
        }
    }
}