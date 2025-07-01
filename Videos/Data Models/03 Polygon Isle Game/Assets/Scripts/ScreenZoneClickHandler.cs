using UnityEngine;

// Defines different interactive zones on the screen
public enum ScreenZone
{
    None,        // No specific zone clicked
    Forward,     // Top middle zone for moving forward
    TurnLeft,    // Left side zone for turning left
    TurnRight,   // Right side zone for turning right
    TurnAround   // Bottom middle zone for turning 180 degrees
}

// Handles mouse interactions with predefined screen zones to trigger game actions
public class ScreenZoneClickHandler : MonoBehaviour
{
    // Header for zone definition fields in the Inspector
    [Header("Zone Definitions (Percentages of Screen Width/Height)")]
    // Width of the left and right turn zones as a percentage of screen width
    [Range(0, 0.5f)] [SerializeField] float sideZoneWidthPercentage = 0.2f;
    // Height of the forward zone from the top of the screen as a percentage of screen height
    [Range(0, 0.5f)] [SerializeField] float forwardZoneHeightPercentage = 0.4f;
    // Height of the turn around zone from the bottom of the screen as a percentage of screen height
    [Range(0, 0.5f)] [SerializeField] float turnAroundZoneHeightPercentage = 0.15f;

    // Reference to the GameManager singleton instance
    private GameManager gameManager;
    // Tracks the currently hovered screen zone
    private ScreenZone currentHoverZone = ScreenZone.None;

    // Called when the script instance is being loaded
    private void Start()
    {
        // Gets the GameManager instance
        gameManager = GameManager.Instance;
        // Logs an error and disables this script if GameManager is not found
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found! Make sure it's in the scene.");
            enabled = false;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Continuously checks which screen zone the mouse cursor is over
        HandleMouseZoneDetection();
        // Checks for mouse clicks and triggers actions based on the current zone
        HandleMouseClick();
    }

    // Determines which screen zone the mouse cursor is currently within
    private void HandleMouseZoneDetection()
    {
        Vector2 mousePos = Input.mousePosition;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        ScreenZone newHoverZone = ScreenZone.None;

        // Checks for the Left Zone
        if (mousePos.x < screenWidth * sideZoneWidthPercentage)
        {
            newHoverZone = ScreenZone.TurnLeft;
        }
        // Checks for the Right Zone
        else if (mousePos.x > screenWidth * (1 - sideZoneWidthPercentage))
        {
            newHoverZone = ScreenZone.TurnRight;
        }
        // Checks for the Forward Zone (top middle)
        else if (mousePos.y > screenHeight * (1 - forwardZoneHeightPercentage))
        {
            // Only sets to Forward if the GameManager allows moving forward from the current location
            if (gameManager.CanMoveForward())
            {
                newHoverZone = ScreenZone.Forward;
            }
        }
        // Checks for the Turn Around Zone (bottom middle)
        else if (mousePos.y < screenHeight * turnAroundZoneHeightPercentage)
        {
            newHoverZone = ScreenZone.TurnAround;
        }

        // Updates the cursor if the hovered zone has changed
        if (newHoverZone != currentHoverZone)
        {
            currentHoverZone = newHoverZone;
            UpdateCursorBasedOnZone(currentHoverZone);
        }
    }

    // Sets the appropriate cursor based on the given screen zone
    private void UpdateCursorBasedOnZone(ScreenZone zone)
    {
        switch (zone)
        {
            case ScreenZone.Forward:
                gameManager.SetCursor(gameManager.ForwardCursor);
                break;
            case ScreenZone.TurnLeft:
                gameManager.SetCursor(gameManager.TurnLeftCursor);
                break;
            case ScreenZone.TurnRight:
                gameManager.SetCursor(gameManager.TurnRightCursor);
                break;
            case ScreenZone.TurnAround:
                gameManager.SetCursor(gameManager.TurnAroundCursor);
                break;
            case ScreenZone.None:
            default:
                gameManager.SetCursor(gameManager.DefaultCursor);
                break;
        }
    }

    // Handles mouse clicks and triggers corresponding game actions
    private void HandleMouseClick()
    {
        // Only process clicks if the GameManager allows player input
        if (gameManager.CanAcceptPlayerInput)
        {
            // Checks for a left mouse button click
            if (Input.GetMouseButtonDown(0))
            {
                // Performs an action based on the current hovered zone
                switch (currentHoverZone)
                {
                    case ScreenZone.Forward:
                        gameManager.RequestMoveForward();
                        break;
                    case ScreenZone.TurnLeft:
                        gameManager.RequestTurnLeft();
                        break;
                    case ScreenZone.TurnRight:
                        gameManager.RequestTurnRight();
                        break;
                    case ScreenZone.TurnAround:
                        gameManager.RequestTurnAround();
                        break;
                    case ScreenZone.None:
                        Debug.Log("Clicked in a non-interactive zone.");
                        break;
                }
            }
        }
    }
}
