using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "InventoryItems/Item")]
public class Item : ScriptableObject
{
    // This class can be used to define properties and behaviors of items in the game.
    // It can be extended with fields for item name, description, icon, etc.
    public string itemName;
    public string description;
    public Sprite icon;
    // You can add methods to define item behaviors, such as using the item, equipping it, etc.
    public void Use()
    {
        Debug.Log("Using item: " + itemName);
        // Implement item usage logic here
    }

}
[CreateAssetMenu(fileName = "New Consumable", menuName = "InventoryItems/Consumable")]
public class ConsumableItem : Item
{
    // This class can be used for items that can be consumed, like potions or food.
    public int healthRestored;
    public void Consume()
    {
        Debug.Log("Consuming item: " + itemName + ", restoring " + healthRestored + " health.");
        // Implement consumption logic here, such as restoring health to the player
    }
}
[CreateAssetMenu(fileName = "New ModSoul", menuName = "InventoryItems/ModSoul")]
public class ModSoul : Item
{
    // This class can be used for mod souls that can be equipped or activated.
    [SerializeField] private Elements element;
    [SerializeField] private float attackPower;
    public string attackName;
    public string ActivateAttack()
    {
        Debug.Log("Activating attack: " + attackName);
        // Implement logic to activate the mod soul's attack
        return attackName;
    }
}
    [CreateAssetMenu(fileName = "New ModSoul", menuName = "InventoryItems/ElementHolder")]
public class ElementHolder:Item {
    [SerializeField]  private GameObject element;
}
