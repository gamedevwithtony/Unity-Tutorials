using UnityEngine;

// Enum to represent the cardinal directions
public enum CardinalDirection
{
    North,
    East,
    South,
    West
}

// Structure to hold the data for a specific directional view
[System.Serializable]
public struct DirectionalView
{
    [Tooltip("The image displayed when facing this direction.")]
    public Sprite viewImage;

    [Tooltip("The location you move to if you 'go forward' from this view.")]
    public LocationSO forwardExit;
}

// Creates a new asset menu entry for a directional location
[CreateAssetMenu(fileName = "NewLocation", menuName = "Scriptable Objects/Directional Location")]

// Scriptable Object representing a single location in a directional navigation system
public class LocationSO : ScriptableObject
{
    public string locationName; // e.g., "Forest Clearing", "Temple Interior"

    // Header for organizing directional view settings in the inspector
    [Header("Directional Views")]
    public DirectionalView northView;
    public DirectionalView southView;
    public DirectionalView eastView;
    public DirectionalView westView;

    // The default direction the player faces when entering this location
    public CardinalDirection defaultEntryDirection = CardinalDirection.North;
}
