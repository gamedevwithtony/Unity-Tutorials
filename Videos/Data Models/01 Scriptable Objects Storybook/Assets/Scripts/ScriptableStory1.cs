using UnityEngine;
using System.Collections.Generic; // For List

// Allows this struct to be seen in the Unity Inspector
[System.Serializable]
public struct StoryPage
{
    public string description; // A brief description of the page
    public Sprite sprite;      // The image displayed on this page
    [TextArea(2, 5)]
    public string text;        // The main story text for this page
}

// Enables creating this object via the Unity Editor menu
[CreateAssetMenu(fileName = "StoryData1", menuName = "ScriptableObjects/ScriptableStory1")]

public class ScriptableStory1 : ScriptableObject
{
    // List of all story pages
    [SerializeField]
    private List<StoryPage> pages = new List<StoryPage>();

    // Index of the currently active story page
    private int currentPageIndex = 0;

    // Returns the current StoryPage object
    public StoryPage GetPage()
    {
        // Returns the current page if pages exist, otherwise a default empty page
        return (pages != null && pages.Count > 0)
            ? pages[currentPageIndex]
            : default(StoryPage);
    }

    // Returns the index of the current page
    public int GetPageIndex()
    {
        return currentPageIndex;
    }

    // Moves to the previous page, clamping the index
    public void PreviousPage()
    {
        // Decrements page index and clamps it within valid bounds
        currentPageIndex = (pages != null && pages.Count > 0) 
            ? Mathf.Clamp(--currentPageIndex, 0, pages.Count - 1) 
            : 0;
    }

    // Moves to the next page, clamping the index
    public void NextPage()
    {
        // Increments page index and clamps it within valid bounds
        currentPageIndex = (pages != null && pages.Count > 0) 
            ? Mathf.Clamp(++currentPageIndex, 0, pages.Count - 1) 
            : 0;
    }
}
