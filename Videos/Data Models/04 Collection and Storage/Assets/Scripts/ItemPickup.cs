using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private CollectibleItem itemData; // Reference to the ScriptableObject holding item details
    [SerializeField] private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    private void Awake()
    {
        // Get the SpriteRenderer component if not assigned in the Inspector
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Set the sprite renderer's sprite to the item's icon
        if (itemData != null && spriteRenderer != null && itemData.itemIcon != null)
        {
            spriteRenderer.sprite = itemData.itemIcon;
        }
    }

    private void OnMouseDown()
    {
        // Log that the item was clicked
        Debug.Log($"Clicked on {itemData.itemName}");

        // Find the ShoppingCart in the scene
        ShoppingCart cart = FindObjectOfType<ShoppingCart>();
        if (cart != null)
        {
            // Add the item to the cart and pass this GameObject for removal reference
            cart.AddItem(itemData, this.gameObject);
        }
        else
        {
            // Log an error if the ShoppingCart is not found
            Debug.LogError("ShoppingCart not found in scene! Item not added.");
        }

        // Deactivate the item GameObject after pickup
        gameObject.SetActive(false); // Or Destroy(gameObject);
    }

    // --- For future expansion / alternative collection (e.g., player walks into it) ---
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player")) // Make sure your player GameObject has the "Player" tag
    //     {
    //         Debug.Log($"Player collected {itemData.itemName}");
    //         ShoppingCart cart = FindObjectOfType<ShoppingCart>();
    //         if (cart != null)
    //         {
    //             cart.AddItem(itemData, this.gameObject);
    //         }
    //         gameObject.SetActive(false);
    //     }
    // }
}
