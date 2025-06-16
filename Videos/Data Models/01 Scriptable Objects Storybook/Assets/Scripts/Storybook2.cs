using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(StoryManager))] // Ensures StoryManager is on the same GameObject
public class Storybook2 : MonoBehaviour
{
    // Reference to the StoryManager component
    private StoryManager storyManager;

    // Reference to the UI Text element for displaying story text
    [SerializeField]
    private Text storyText;

    // Flag to control if page turning is currently allowed
    private bool allowPaging = true;

    // Reference to the image fading transition script
    private FadeTransition fadeScript;

    // Callback action to execute when the fade animation finishes
    private Action fadeCallback;

    // Called when the script instance is being loaded
    void Awake()
    {
        // Get the required StoryManager component
        storyManager = GetComponent<StoryManager>();
        // Log an error and disable if StoryManager is missing
        if (storyManager == null)
        {
            Debug.LogError("Storybook2: Missing required StoryManager component on this GameObject. Disabling Storybook2.");
            enabled = false; // Disable this component
        }
    }

    // Called once when the script starts
    void Start()
    {
        // Get the FadeTransition component
        fadeScript = GetComponent<FadeTransition>();
        // Assign the FadeComplete method to the fade callback
        fadeCallback += FadeComplete;

        // Display the initial story text and image
        storyText.text = storyManager.GetPage().text;
        fadeScript.SetImage(storyManager.GetPage().sprite);
    }

    // Update is called once per frame (currently no logic here)
    void Update()
    {
        // No logic here.
    }

    // Handles button clicks for navigating story pages
    public void ClickHandler(string method)
    {
        // Stop if page turning is not allowed
        if (!allowPaging) return;

        // Navigate based on the button's input string
        if (method == "Previous")
        {
            storyManager.PreviousPage(); // Move to the previous page
        }
        else
        {
            storyManager.NextPage(); // Move to the next page
        }

        // Update the UI text and image with the new page's content
        storyText.text = storyManager.GetPage().text;
        fadeScript.SetImage(storyManager.GetPage().sprite);

        // Start the image fade transition, triggering the callback on completion
        fadeScript.Fade(fadeCallback);

        // Disable paging until the fade is complete
        allowPaging = false;
    }

    // Callback method executed when the fade animation finishes
    private void FadeComplete()
    {
        allowPaging = true; // Re-enable page turning
    }
}
