using UnityEngine;

// Allows this struct to be serialized and displayed in the Unity Inspector
[System.Serializable]
public struct ResolutionOption
{
    public int width;         // Horizontal resolution
    public int height;        // Vertical resolution
    public int refreshRate;   // Display refresh rate in Hertz

    // Constructor to initialize resolution properties
    public ResolutionOption(int width, int height, int refreshRate)
    {
        this.width = width;
        this.height = height;
        this.refreshRate = refreshRate;
    }

    // Returns a formatted string representation of the resolution
    public override string ToString()
    {
        return $"{width}x{height} @{refreshRate}Hz";
    }
}
