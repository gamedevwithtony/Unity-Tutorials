using UnityEngine;
using System.Collections.Generic; // For List

// Allows creating this object via the Unity Editor menu
[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/Game Settings")]

public class GameSettings : ScriptableObject
{
    // --- Public Settings Variables ---
    public int resolutionIndex;  // Index for the chosen screen resolution
    public bool isFullscreen;    // Flag for fullscreen mode
    public float masterVolume;   // Global volume level (0.0 to 1.0)
    public float sfxVolume;      // Sound effects volume level (0.0 to 1.0)
    public string playerName;    // Player's name

    // --- Predefined Resolution Options (for dropdown) ---
    // A static list of available screen resolutions for display
    public static readonly List<ResolutionOption> AvailableResolutions = new List<ResolutionOption>
    {
        new ResolutionOption(800, 600, 60),
        new ResolutionOption(1280, 720, 60),
        new ResolutionOption(1920, 1080, 60),
        new ResolutionOption(2560, 1440, 60),
        new ResolutionOption(3840, 2160, 60)
        // Add more common resolutions as needed
    };

    // --- Default Values ---
    // Sets all game settings to their default values
    public void SetDefaultValues()
    {
        resolutionIndex = 2; // Default to 1920x1080 (index 2 in AvailableResolutions)
        isFullscreen = true;
        masterVolume = 0.75f; // 75% volume
        sfxVolume = 0.5f;     // 50% volume
        playerName = "Player1";
    }

    // --- Validation Method ---
    // Validates the current values of the game settings variables
    public bool ValidateSettings()
    {
        // 1. Validate resolutionIndex
        if (resolutionIndex < 0 || resolutionIndex >= AvailableResolutions.Count)
        {
            Debug.LogWarning($"Validation Error: Resolution index {resolutionIndex} is out of bounds. Max index is {AvailableResolutions.Count - 1}.");
            return false;
        }

        // 2. isFullscreen is a bool, so it's inherently valid, no need to check

        // 3. Validate masterVolume and sfxVolume (should be between 0.0 and 1.0)
        if (masterVolume < 0f || masterVolume > 1f)
        {
            Debug.LogWarning($"Validation Error: Master volume {masterVolume} is out of range (0.0-1.0).");
            return false;
        }
        if (sfxVolume < 0f || sfxVolume > 1f)
        {
            Debug.LogWarning($"Validation Error: SFX volume {sfxVolume} is out of range (0.0-1.0).");
            return false;
        }

        // 4. Validate playerName (e.g., not null or empty)
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Validation Error: Player name cannot be empty or null.");
            return false;
        }
        // Optional: Add length checks, character restrictions, etc.
        // if (playerName.Length > 20) { Debug.LogWarning(...); return false; }

        Debug.Log("Game settings validated successfully.");
        return true;
    }

    // --- Apply Settings to Actual Game (Example) ---
    // Applies the current settings to the game's environment
    public void ApplySettings()
    {
        // Apply chosen resolution and fullscreen mode
        if (resolutionIndex >= 0 && resolutionIndex < AvailableResolutions.Count)
        {
            ResolutionOption res = AvailableResolutions[resolutionIndex];
            // https://docs.unity3d.com/6000.1/Documentation/ScriptReference/FullScreenMode.html
            // Determine fullscreen mode (exclusive or windowed)
            FullScreenMode fullScreenMode = isFullscreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
            // https://docs.unity3d.com/6000.1/Documentation/ScriptReference/RefreshRate.html
            // Create Unity's RefreshRate object
            RefreshRate refreshRate = new RefreshRate { numerator = (uint)res.refreshRate, denominator = 1 };
            // Apply the resolution
            Screen.SetResolution(res.width, res.height, fullScreenMode, refreshRate);
            Debug.Log($"Applied Resolution: {res.width}x{res.height} @{res.refreshRate}Hz, Fullscreen: {isFullscreen}");
        }
        else
        {
            Debug.LogError($"Cannot apply resolution: Invalid resolution index {resolutionIndex}.");
        }

        // Apply Volume
        AudioListener.volume = masterVolume; // Simplified for example, might use audio mixers
        Debug.Log($"Applied Master Volume: {masterVolume}");
        // For SFX, you'd likely control a specific AudioMixer group.
        // E.g., mixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20); // Conversion for mixer dB
    }
}
