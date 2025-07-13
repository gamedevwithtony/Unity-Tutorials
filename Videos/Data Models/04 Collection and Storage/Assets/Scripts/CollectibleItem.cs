using UnityEngine;

// Allows creating new instances of this ScriptableObject through the Unity Editor's Create menu
[CreateAssetMenu(fileName = "NewCollectibleItem", menuName = "Collectible Item/Basic Item")]

public class CollectibleItem : ScriptableObject
{
    public string itemName = "New Item"; // The visible name of the collectible item
    public Sprite itemIcon; // The icon representing this item
    [TextArea(3, 10)]
    public string itemDescription = "A generic item."; // A detailed description for the item
}
