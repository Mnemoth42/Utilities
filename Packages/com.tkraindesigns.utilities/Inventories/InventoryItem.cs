using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TkrainDesigns.Core;
using UnityEngine;

namespace TkrainDesigns.Inventories
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item", order = 1)]
    public  class InventoryItem : ScriptableObject, IUniqueIdentifier
    {
        [field: SerializeField] public string UniqueId { get; private set; } = System.Guid.NewGuid().ToString();
        [field: SerializeField] public string Name { get; private set; } = "New Item";
        [field: SerializeField] public string Description { get; private set; } = "Default description";
        [field: SerializeField] public Sprite Icon { get; private set; } 
        [field: SerializeField] public GameObject Pickup { get; private set; }
        [field: Min(1)]
        [field: SerializeField] public int StackSize { get; private set; } = 1;
        public bool IsStackable => StackSize > 1;
        [field: SerializeField] public float BasePrice { get; private set; } = 1;
        [field: SerializeField] public float BaseWeight { get; private set; } = 1;
        [field: SerializeField] public bool IsConsumable { get; private set; } = false;
        
        private static Dictionary<string, InventoryItem> globalLookup = new Dictionary<string, InventoryItem>();

        protected virtual void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(UniqueId))
            {
                UniqueId = System.Guid.NewGuid().ToString();
            }

            // Prevent duplicate UniqueIds when duplicating ScriptableObjects
            if (globalLookup.TryGetValue(UniqueId, out var otherItem) && otherItem != this)
            {
                if (otherItem == null)
                {
                    globalLookup[UniqueId] = this;
                }
                else
                {
                    UniqueId = System.Guid.NewGuid().ToString();
                    globalLookup[UniqueId] = this;
                }
            }
            else
            {
                globalLookup[UniqueId] = this;
            }
        }
        
        public virtual JToken Capture()
        {
            return new JObject();
        }

        public virtual void Restore(JToken data)
        {
        }

        #region Lookup
        private static Dictionary<string, InventoryItem> items;
        
        public static InventoryItem GetFromID(string uniqueID)
        {
            BuildLookup();
            return items.GetValueOrDefault(uniqueID);
        }

        private static void BuildLookup()
        {
            if (items != null) return;
            items = new Dictionary<string, InventoryItem>();
            foreach (InventoryItem item in Resources.LoadAll<InventoryItem>(""))
            {
                if (items.TryGetValue(item.UniqueId, out var existing))
                {
                    Debug.LogError($"Duplicate item ID: {item.UniqueId}, {item.name}/{existing.name}");
                    continue;
                }
                items.Add(item.UniqueId, item);
            }
        }
        #endregion
    }
}