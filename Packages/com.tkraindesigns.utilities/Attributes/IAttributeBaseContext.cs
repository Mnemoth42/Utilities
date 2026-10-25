using UnityEngine;

namespace TkrainDesigns.Attributes
{
    public interface IAttributeEventContext
    {
        public event System.Action<AttributeSO, float> OnAttributeChanged;
    }
    public interface IAttributeBaseContext
    {
        float GetBaseValue(AttributeSO attribute);
        float GetAttributeValue(AttributeSO attribute);
        GameObject gameObject { get; }
    }
}