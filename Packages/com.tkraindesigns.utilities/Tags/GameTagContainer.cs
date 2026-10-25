using System.Collections.Generic;
using UnityEngine;

namespace TkrainDesigns.Tags
{
    [System.Serializable]
    public struct GameTagContainer
    {
        [field: SerializeField] public List<GameTag> Tags { get; private set; }
        
        public GameTagContainer(List<GameTag> tags)
        {
            Tags = tags;
        }
        public bool Contains(GameTag tag) => Tags.Contains(tag);
        public int Count => Tags.Count;
        public void Clear() => Tags.Clear();
        public void Add(GameTag tag) => Tags.Add(tag);
        public void Remove(GameTag tag) => Tags.Remove(tag);
    }
}