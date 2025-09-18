using System.Collections.Generic;
using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.InventorySystem
{
    public class Inventory : Singleton<Inventory>
    {
        [SerializeField] private List<string> _inventory = new();
        
        public void AddItem(string item)
        {
            if (HasItem(item)) return;
            _inventory.Add(item);
        }
        
        public void RemoveItem(string item)
        {
            if (!HasItem(item)) return;
            _inventory.Remove(item);
        }

        public bool HasItem(string item)
        {
            return _inventory.Contains(item);
        }

        public void ClearInventory()
        {
            _inventory.Clear();
        }
    }
}
