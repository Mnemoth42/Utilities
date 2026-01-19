using System.Collections.Generic;

namespace TkrainDesigns.Attributes
{
    public interface IAttributeModifierContext
    {
        IEnumerable<AttributeModifier> GetAttributeModifiers(AttributeSO attribute);
        
    }
}