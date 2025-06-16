using UnityEngine;

// Enables creating this object via the Unity Editor menu
[CreateAssetMenu(fileName = "StoryData2", menuName = "ScriptableObjects/ScriptableStory2")]

public class ScriptableStory2 : ScriptableObject
{
    public string description; // A brief description for this story page
    public Sprite sprite;      // The image to display on this page
    [Multiline]
    public string text;        // The main story text, allowing multiple lines in the Inspector

    // References to the previous and next story pages in a linked list fashion
    public ScriptableStory2 previousPage;
    public ScriptableStory2 nextPage;
}
