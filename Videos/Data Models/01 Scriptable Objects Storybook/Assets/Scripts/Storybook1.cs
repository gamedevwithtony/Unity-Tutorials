using System;
using UnityEngine;
using UnityEngine.UI;

public class Storybook1 : MonoBehaviour
{
    // Reference to the ScriptableObject holding story data
    [SerializeField]
    private ScriptableStory1 story;

    // Reference to the UI Text element displaying story text
    [SerializeField]
    private Text storyText;

    // Flag to control if page turning is allowed
    private bool allowPaging = true;

    // Reference to the image fading transition script
    private FadeTransition fadeScript;

    // Callback action to run when the fade animation finishes
    private Action fadeCallback;

    // Called once when the script starts
    void Start()
    {
        // Get the FadeTransition component attached to this GameObject
        fadeScript = GetComponent<FadeTransition>();
        // Assign the FadeComplete method to the fade callback
        fadeCallback += FadeComplete;

        // Optional: Example code for testing callbacks
        //fadeCallback += DoSomething1;
        //fadeCallback += DoSomething2;

        // Display the initial story text and image
        storyText.text = story.GetPage().text;
        fadeScript.SetImage(story.GetPage().sprite);
    }

    // Update is called once per frame
    void Update()
    {
        // Optional: Example code for testing fade with mouse click
        //if (Input.GetMouseButtonDown(0))
        //{
        //    fadeScript.Fade(fadeCallback);
        //    fadeCallback = null;
        //}
    }

    // Handles button clicks for navigating story pages
    public void ClickHandler(string method)
    {
        Debug.Log("ClickHandler Called");
        Debug.Log("allowPaging : " + allowPaging);

        // Exit if page turning is currently disabled
        if (!allowPaging) return;

        Debug.Log("Current pageIndex => " + story.GetPageIndex());

        // Navigate based on the button's method string
        if (method == "Previous")
        {
            story.PreviousPage(); // Move to the previous page
            Debug.Log("Previous Button : pageIndex => " + story.GetPageIndex());
        }
        else
        {
            story.NextPage(); // Move to the next page
            Debug.Log("Next Button : pageIndex => " + story.GetPageIndex());
        }

        // Update UI with the new page's text and sprite
        storyText.text = story.GetPage().text;
        fadeScript.SetImage(story.GetPage().sprite);

        // Start the image fade transition, triggering the callback on completion
        fadeScript.Fade(fadeCallback);

        // Disable paging until the fade is complete
        allowPaging = false;
    }

    // Callback method executed when the fade animation finishes
    private void FadeComplete()
    {
        // when the fade completes, allow paging
        allowPaging = true; // Re-enable page turning
        Debug.Log("Callback Ran, Fade Completed");
    }

    // Example callback method 1 (commented out in Start)
    private void DoSomething1()
    {
        Debug.Log("Callback 1 : Fade Completed");
    }

    // Example callback method 2 (commented out in Start)
    private void DoSomething2()
    {
        Debug.Log("Callback 2 : Something Else To Do?");
    }
}
