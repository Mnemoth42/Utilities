using System.Collections.Generic;
using UnityEngine;

namespace TkrainDesigns.Tags
{
    [System.Serializable]
    public class GameTag
    {
        [field: SerializeField] public string Tag { get; private set; }
        

        public GameTag(string tag)
        {
            Tag = tag;
        }
        
        public bool MatchesTag(string tag) => Tag == tag;

        public bool Contains(string tag)
        {
            List<string> parts = new List<string>(Tag.Split('.'));
            List<string> tagParts = new List<string>(tag.Split('.'));
            int depth = Mathf.Min(tagParts.Count, parts.Count);
            for(int i=0;i<depth;i++)
            {
                if(parts[i] != tagParts[i]) return false;
            }
            return true;
        }
    }
}