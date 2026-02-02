using System.Collections.Generic;
using TkrainDesigns.Attributes;
using TkrainDesigns.ScalableFloats;
using UnityEngine;

namespace TkrainDesigns.Effects
{
    public enum DurationType
    {
        Instant,
        Duration,
        Infinite,
    }
    
    [CreateAssetMenu(fileName = "Effect", menuName = "TkrainDesigns/Effects", order = 0)]
    public class EffectSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }= "";
        [field: SerializeField] public string Description { get; private set; }= "";
        [field: SerializeField] public DurationType DurationType { get; private set; } = DurationType.Instant;
        [field: SerializeField] public ScalableFloat Duration { get; private set; }
        [field: SerializeField] public ScalableFloat Period { get; private set; }
        [field: SerializeField] public List<AttributeModifier> AttributeModifier { get; private set; }
        
    }
}