using UnityEngine;
using UnityEngine.UI;

public class CartItemUIEntry : MonoBehaviour, ICartItem
{
    private ShoppingCart shoppingCart; // Reference to the shopping cart managing this item
    private CartItemInstance currentItemInstance; // The specific instance of the item this UI entry represents

    public Image itemIcon; // UI Image component to display the item's icon
    public Text itemText; // UI Text component to display the item's name

    // Initializes the UI entry with item data and a reference to the shopping cart
    public void SetItem(CartItemInstance itemInstance, ShoppingCart cartRef)
    {
        currentItemInstance = itemInstance; // Store the item instance
        shoppingCart = cartRef; // Store the cart reference

        // Set the item icon's sprite and properties
        if (itemIcon != null)
        {
            itemIcon.sprite = currentItemInstance.itemData.itemIcon; // Assign the sprite from item data
            itemIcon.type = Image.Type.Simple; // Set image type to simple for consistent rendering
            itemIcon.preserveAspect = true; // Ensure the icon maintains its aspect ratio
        }

        // Set the item name text
        if (itemText != null)
        {
            itemText.text = currentItemInstance.itemData.itemName;
        }
    }

    // Called when the UI button is clicked to remove the item
    public void OnClickRemoveItem()
    {
        // Remove the item from the cart and destroy this UI entry GameObject
        if (currentItemInstance != null && shoppingCart != null)
        {
            shoppingCart.RemoveSpecificItemInstance(currentItemInstance); // Instruct the cart to remove the instance
            Destroy(gameObject); // Destroy this UI GameObject
        }
    }

    // Implementation of the ICartItem interface
    public CollectibleItem GetItemData()
    {
        // Returns the underlying CollectibleItem data, if available
        return currentItemInstance?.itemData;
    }
}
