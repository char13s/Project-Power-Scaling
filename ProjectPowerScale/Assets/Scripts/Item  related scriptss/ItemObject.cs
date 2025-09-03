using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ItemObject : MonoBehaviour
{
    [SerializeField] private Item itemData;
    public Item ItemData { get => itemData; set => itemData = value; }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Assuming the player has an Inventory component with an AddItem method
            Inventory playerInventory = other.GetComponent<Inventory>();
            if (playerInventory != null)
            {
                playerInventory.AddItem(itemData,1);
                Destroy(gameObject); // Remove the item from the scene after picking it up
            }
        }
    }
}
