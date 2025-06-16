using UnityEngine;

public class StoryManager : MonoBehaviour
{
    // The very first page of the story, set in the Inspector
    [SerializeField]
    private ScriptableStory2 startPage;

    // The currently active story page
    private ScriptableStory2 CurrentPage;

    // Called when the script instance is being loaded
    void Awake()
    {
        // Initialize the current page to the assigned start page
        CurrentPage = startPage;
        // Log a warning and disable the script if no start page is set
        if (CurrentPage == null)
        {
            Debug.LogWarning("StoryManager: No start page assigned in the Inspector. Story will begin with no page.");
            enabled = false; // Disable this component
        }
    }

    // Retrieves the currently active story page object
    public ScriptableStory2 GetPage()
    {
        return CurrentPage;
    }

    // Moves the story to the previous page if one exists
    public void PreviousPage()
    {
        // Navigate to the previous page if it's set
        if (CurrentPage != null && CurrentPage.previousPage != null)
        {
            CurrentPage = CurrentPage.previousPage;
        }
    }

    // Advances the story to the next page if one exists
    public void NextPage()
    {
        // Navigate to the next page if it's set
        if (CurrentPage != null && CurrentPage.nextPage != null)
        {
            CurrentPage = CurrentPage.nextPage;
        }
    }
}
