using UnityEngine;
using System.IO; // For File operations
using System.Text; // For Encoding
using System; // For try-catch blocks

public class SettingsManager : MonoBehaviour
{
    // Assign your GameSettings ScriptableObject asset here
    [SerializeField] private GameSettings gameSettings;

    // Name of the settings file
    private const string SETTINGS_FILE_NAME = "settings.json";
    // https://docs.unity3d.com/6000.1/Documentation/ScriptReference/Application-persistentDataPath.html
    // Full path to the settings file in persistent data path
    private string SettingsFilePath => Path.Combine(Application.persistentDataPath, SETTINGS_FILE_NAME);

    // Event invoked after settings are loaded or defaulted
    public static Action OnSettingsLoaded;
    // Event invoked after settings are saved
    public static Action OnSettingsSaved;

    // Called when the script instance is being loaded
    void Awake()
    {
        if (gameSettings == null)
        {
            Debug.LogError("SettingsManager: GameSettings asset not assigned! Please assign it in the Inspector.");
            // Optionally, create one if it's null, but assigning in editor is best practice
            // gameSettings = ScriptableObject.CreateInstance<GameSettings>();
        }

        // Load settings from file or set defaults
        LoadSettings();

        // Apply the loaded/defaulted settings to the game
        gameSettings.ApplySettings();
    }

    // Called when the application quits (optional auto-save)
    void OnApplicationQuit()
    {
        // Example: SaveSettings(); // Uncomment to auto-save on quit
    }

    // --- Loading Settings ---
    // Loads game settings from a JSON file
    public void LoadSettings()
    {
        // Exit if GameSettings asset is missing
        if (gameSettings == null)
        {
            Debug.LogError("GameSettings is not assigned. Cannot load settings.");
            return;
        }

        // Check if settings file exists
        if (File.Exists(SettingsFilePath))
        {
            try
            {
                string json = File.ReadAllText(SettingsFilePath); // Read JSON from file
                Debug.Log($"Attempting to load settings from: {SettingsFilePath}");
                Debug.Log($"Loaded JSON: {json}");

                // Create a temporary instance to deserialize for validation
                GameSettings tempSettings = ScriptableObject.CreateInstance<GameSettings>();
                JsonUtility.FromJsonOverwrite(json, tempSettings);

                // Validate loaded data
                if (tempSettings.ValidateSettings())
                {
                    // If valid, apply settings to the main GameSettings object
                    JsonUtility.FromJsonOverwrite(json, gameSettings);
                    Debug.Log("Settings loaded and validated successfully.");
                }
                else
                {
                    // If validation fails, revert to default settings
                    Debug.LogError("Loaded settings failed validation. Discarding loaded data and resetting to defaults.");
                    gameSettings.SetDefaultValues();
                }
                Destroy(tempSettings); // Clean up temporary instance
            }
            catch (Exception e)
            {
                // Handle any loading or parsing errors by resetting to defaults
                Debug.LogError($"Error loading settings from {SettingsFilePath}: {e.Message}. Resetting to defaults.");
                gameSettings.SetDefaultValues();
            }
        }
        else
        {
            // If file not found, use default settings
            Debug.Log($"Settings file not found at {SettingsFilePath}. Using default settings.");
            gameSettings.SetDefaultValues();
        }

        // Apply the newly loaded or default settings to the game
        gameSettings.ApplySettings();

        // Notify subscribers that settings have been loaded
        OnSettingsLoaded?.Invoke();
    }

    // --- Saving Settings ---
    // Saves current game settings to a JSON file
    public void SaveSettings()
    {
        // Exit if GameSettings asset is missing
        if (gameSettings == null)
        {
            Debug.LogError("GameSettings is not assigned. Cannot save settings.");
            return;
        }

        try
        {
            string json = JsonUtility.ToJson(gameSettings, true); // Serialize to JSON (pretty printed)
            File.WriteAllText(SettingsFilePath, json, Encoding.UTF8); // Write JSON to file
            Debug.Log($"Settings saved to: {SettingsFilePath}");
            Debug.Log($"Saved JSON: {json}");
            OnSettingsSaved?.Invoke(); // Notify subscribers
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving settings to {SettingsFilePath}: {e.Message}");
        }
    }

    // --- Public methods for UI interaction ---
    // Updates the resolution index in GameSettings
    public void SetResolutionIndex(int newIndex)
    {
        // Only set if index is valid
        if (newIndex >= 0 && newIndex < GameSettings.AvailableResolutions.Count)
        {
            gameSettings.resolutionIndex = newIndex;
            Debug.Log($"Resolution Index set to: {newIndex}");
        }
        else
        {
            Debug.LogWarning($"Attempted to set invalid resolution index: {newIndex}");
        }
    }

    // Updates the fullscreen setting in GameSettings
    public void SetFullscreen(bool newFullscreen)
    {
        gameSettings.isFullscreen = newFullscreen;
        Debug.Log($"Fullscreen set to: {newFullscreen}");
    }

    // Updates the master volume in GameSettings, clamped 0-1
    public void SetMasterVolume(float newVolume)
    {
        gameSettings.masterVolume = Mathf.Clamp01(newVolume);
        Debug.Log($"Master Volume set to: {gameSettings.masterVolume}");
    }

    // Updates the SFX volume in GameSettings, clamped 0-1
    public void SetSfxVolume(float newVolume)
    {
        gameSettings.sfxVolume = Mathf.Clamp01(newVolume);
        Debug.Log($"SFX Volume set to: {gameSettings.sfxVolume}");
    }

    // Updates the player name in GameSettings
    public void SetPlayerName(string newName)
    {
        gameSettings.playerName = newName;
        Debug.Log($"Player Name set to: {newName}");
    }

    // --- Method to get the current settings (for UI to display) ---
    // Returns the current GameSettings ScriptableObject
    public GameSettings GetCurrentSettings()
    {
        return gameSettings;
    }
}
