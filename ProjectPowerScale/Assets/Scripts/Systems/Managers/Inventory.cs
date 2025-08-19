
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
    }

    // Other methods like RemoveItem, UseItem, etc.

}
