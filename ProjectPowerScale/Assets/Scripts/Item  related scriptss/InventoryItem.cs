using System;
using UnityEngine;
[System.Serializable]
public class InventoryItem
{
    public Item itemData;
    public int stackSize;
    // Or unique instance data, like a specific durability value.
    public float durability;
}
