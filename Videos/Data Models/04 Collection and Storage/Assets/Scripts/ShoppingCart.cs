using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // For List

// Marks the class as serializable so it can be seen and saved in the Inspector
[System.Serializable]
public class CartItemInstance
{
    public CollectibleItem itemData; // The ScriptableObject holding the item's immutable data
    public GameObject sceneObjectReference; // Reference to the actual GameObject in the scene (e.g., the pickup object)

    // Constructor to initialize a new instance
    public CartItemInstance(CollectibleItem data, GameObject sceneObj)
    {
        itemData = data;
        sceneObjectReference = sceneObj;
    }
}

public class ShoppingCart : MonoBehaviour
{
    // List to hold all items currently in the shopping cart
    public List<CartItemInstance> collectedItems = new List<CartItemInstance>();

    [Header("UI References")] // Organizes fields in the Inspector under a header
    public Text cartContentsText; // Text display for the total number of items
    public Transform cartItemsContentParent; // The parent transform for dynamically created UI item entries
    public GameObject cartItemEntryPrefab; // The prefab used to create individual UI item entries

    private void Start()
    {
        UpdateCartUI(); // Initialize the UI when the script starts
    }

    // Adds a new item to the cart
    public void AddItem(CollectibleItem itemToAddData, GameObject sceneObjectPickedUp)
    {
        // Create a new instance to track both data and its scene object
        CartItemInstance newEntry = new CartItemInstance(itemToAddData, sceneObjectPickedUp);
        collectedItems.Add(newEntry); // Add the new item instance to the list

        Debug.Log($"Added {itemToAddData.itemName} to cart. Total items: {collectedItems.Count}");
        UpdateCartUI(); // Refresh the UI to reflect the change
    }

    // Removes a specific item instance from the cart
    public void RemoveSpecificItemInstance(CartItemInstance itemToRemoveInstance)
    {
        // Attempt to remove the item instance from the list
        if (collectedItems.Remove(itemToRemoveInstance))
        {
            Debug.Log($"Removed {itemToRemoveInstance.itemData.itemName} from cart. Total items: {collectedItems.Count}");

            // Re-enable the physical item in the scene if its reference exists
            if (itemToRemoveInstance.sceneObjectReference != null)
            {
                itemToRemoveInstance.sceneObjectReference.SetActive(true);
                Debug.Log($"Re-enabled {itemToRemoveInstance.itemData.itemName} in scene.");
            }
            else
            {
                // Warn if the scene object reference was lost
                Debug.LogWarning($"Scene object reference for {itemToRemoveInstance.itemData.itemName} was null. Cannot re-enable the physical item.");
            }

            UpdateCartUI(); // Refresh the UI after removal
        }
        else
        {
            // This warning indicates an unexpected state where the instance was not found
            Debug.LogWarning($"{itemToRemoveInstance.itemData.itemName} (instance) was not found in the cart. This shouldn't happen!");
        }
    }

    // Updates the cart's UI display
    private void UpdateCartUI()
    {
        // Check if essential UI references are assigned
        if (cartItemsContentParent == null || cartItemEntryPrefab == null)
        {
            Debug.LogWarning("UI setup for dynamic cart items is incomplete in ShoppingCart script!");
            return; // Exit if UI setup is missing
        }

        // Clear all existing UI item entries from the parent transform
        foreach (Transform child in cartItemsContentParent)
        {
            Destroy(child.gameObject);
        }

        // Update the main cart count text
        if (collectedItems.Count == 0)
        {
            if (cartContentsText != null)
            {
                cartContentsText.text = "Empty"; // Display "Empty" if cart has no items
            }
        }
        else
        {
            if (cartContentsText != null)
            {
                // Display the count with correct pluralization for "Item"
                cartContentsText.text = $"{collectedItems.Count} Item{(collectedItems.Count > 1 ? "s" : "")}";
            }

            // Create and populate UI entries for each item in the cart
            foreach (CartItemInstance itemInstance in collectedItems)
            {
                // Instantiate the UI entry prefab under the designated parent
                GameObject itemUI = Instantiate(cartItemEntryPrefab, cartItemsContentParent);
                CartItemUIEntry entryScript = itemUI.GetComponent<CartItemUIEntry>(); // Get the script on the new UI entry
                if (entryScript != null)
                {
                    // Initialize the UI entry with its corresponding item instance and cart reference
                    entryScript.SetItem(itemInstance, this);
                }
            }
        }
    }
}
