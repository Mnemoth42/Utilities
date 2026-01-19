namespace TkrainDesigns.Core.TkrainDesigns.Core
{
    public interface ILevelProvider
    {
        int Level { get; }
        event System.Action<int> OnLevelChanged;
    }
}