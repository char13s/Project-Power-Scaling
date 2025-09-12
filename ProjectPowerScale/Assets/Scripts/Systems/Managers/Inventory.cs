
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(Item itemToAdd, int amount)
    {
        // Logic to add the item to the list, handling stacks, etc.
        InventoryItem existingItem = items.Find(i => i.itemData == itemToAdd);
        if (existingItem != null) {
            existingItem.stackSize += amount;
        }
        else
        {
            InventoryItem newItem = new InventoryItem { itemData = itemToAdd, stackSize = amount };
            items.Add(newItem);
        }
        Debug.Log($"Added {amount} of {itemToAdd.itemName} to inventory.");
    }

    // Other methods like RemoveItem, UseItem, etc.

}
