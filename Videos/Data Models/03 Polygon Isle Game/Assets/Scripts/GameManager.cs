using System;
using UnityEngine;
using UnityEngine.UI; // For Image component

// Defines a custom cursor with its texture and hotspot
[Serializable]
public struct CursorDefinition
{
    public Texture2D Texture; // The texture for the cursor
    public Vector2 Hotspot;   // The pixel offset from the top-left of the cursor where the click originates
}

// Manages the overall game state, including location, direction, transitions, and cursors
public class GameManager : MonoBehaviour
{
    // Singleton instance of the GameManager
    public static GameManager Instance { get; private set; }

    // Header for game setup fields in the Inspector
    [Header("Game Setup")]
    // The initial location where the game begins
    [SerializeField] private LocationSO startingLocation;
    // The UI Image component used to display the current view
    [SerializeField] private Image viewDisplayImage;
    // The effect used for transitions between views (e.g., fade)
    [SerializeField] private ITransitionEffect transitionEffect;
    // The UI Text component to display the current location name
    [SerializeField] private Text locationText;

    // Flag indicating if a visual transition is currently in progress
    private bool _isTransitioning = false;
    public bool IsTransitioning => _isTransitioning; // Read-only public access

    // Event invoked when the transition state changes
    public static event Action<bool> OnTransitioningStateChanged;
    // Event invoked when a transition starts
    public static event Action OnTransitionStarted;
    // Event invoked when a transition finishes
    public static event Action OnTransitionFinished;

    // Flag to track if the initial game content has been loaded
    private bool hasInitialContentLoaded = false;

    // The currently active location
    private LocationSO currentLocation;
    // The direction the player is currently facing
    private CardinalDirection currentFacingDirection;

    // Header for cursor management fields in the Inspector
    [Header("Cursor Management")]
    [SerializeField] private CursorDefinition _defaultCursor; // Default cursor texture
    public CursorDefinition DefaultCursor => _defaultCursor; // Read-only public access

    [SerializeField] private CursorDefinition _forwardCursor; // Cursor for "move forward" zone
    public CursorDefinition ForwardCursor => _forwardCursor; // Read-only public access

    [SerializeField] private CursorDefinition _turnLeftCursor; // Cursor for "turn left" zone
    public CursorDefinition TurnLeftCursor => _turnLeftCursor; // Read-only public access

    [SerializeField] private CursorDefinition _turnRightCursor; // Cursor for "turn right" zone
    public CursorDefinition TurnRightCursor => _turnRightCursor; // Read-only public access

    [SerializeField] private CursorDefinition _turnAroundCursor; // Cursor for "turn around" zone
    public CursorDefinition TurnAroundCursor => _turnAroundCursor; // Read-only public access

    // Determines if player input should be accepted (i.e., not during a transition)
    public bool CanAcceptPlayerInput
    {
        get { return !_isTransitioning; }
    }

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Implements the Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // Tries to get the ITransitionEffect component if not assigned
        if (transitionEffect == null)
        {
            transitionEffect = GetComponent<ITransitionEffect>();
        }
        // Logs an error if no ITransitionEffect is found
        if (transitionEffect == null)
        {
            Debug.LogError("ITransitionEffect implementation not found on " + gameObject.name, this);
        }

        // Sets the initial cursor
        SetCursor(_defaultCursor);

        // Initializes the flag for initial content loading
        hasInitialContentLoaded = false;

        // Ensures the transition state is initially false and notifies subscribers
        _isTransitioning = false;
        OnTransitioningStateChanged?.Invoke(false);
    }

    // Called on the frame when a script is enabled just before any Update methods are called the first time
    private void Start()
    {
        // Loads the starting location if one is assigned
        if (startingLocation != null)
        {
            LoadLocation(startingLocation, startingLocation.defaultEntryDirection);
        }
        else
        {
            // Logs an error if no starting location is set
            Debug.LogError("No starting location set for GameManager!");
        }
    }

    // Loads a new location and sets the initial facing direction
    public void LoadLocation(LocationSO newLocation, CardinalDirection entryDirection)
    {
        // Prevents loading a null location
        if (newLocation == null)
        {
            Debug.LogWarning("Attempted to load a null location.");
            return;
        }

        // Updates the current location and facing direction
        currentLocation = newLocation;
        currentFacingDirection = entryDirection;

        // Logs the loaded location and facing direction
        Debug.Log($"Loaded location: {currentLocation.locationName} - Facing: {currentFacingDirection}");

        // Updates the displayed view for the new location
        UpdateView();
    }

    // Updates the displayed image based on the current location and facing direction
    private void UpdateView()
    {
        // Gets the specific view data for the current direction
        DirectionalView currentView = GetCurrentDirectionalView();

        // Optional: Code for updating the display image directly without a transition
        /*
        if (viewDisplayImage != null)
        {
            viewDisplayImage.sprite = currentView.viewImage;
            if (viewDisplayImage.sprite == null)
            {
                Debug.LogWarning($"No image assigned for {currentFacingDirection} view at {currentLocation.locationName}");
            }
        }
        else
        {
            Debug.LogError("View Display Image UI component not assigned!");
        }
        */

        // Updates the UI text to show current location and direction
        locationText.text = $"{currentLocation.locationName} - {currentFacingDirection}";

        // Displays the first view immediately without a transition effect
        if (!hasInitialContentLoaded)
        {
            transitionEffect.DisplayImmediately(currentView.viewImage);
            hasInitialContentLoaded = true;
        }
        else
        {
            // Starts a transition for subsequent view changes
            StartTransition(currentView.viewImage);
        }
    }

    // Attempts to move forward into the next connected location
    public void RequestMoveForward()
    {
        // Prevents movement if a transition is active
        if (_isTransitioning)
        {
            Debug.Log("GameManager: Cannot MoveForward - currently transitioning.");
            return;
        }

        // Gets the current view to check for an exit
        DirectionalView currentView = GetCurrentDirectionalView();

        // Loads the next location if a forward exit is defined
        if (currentView.forwardExit != null)
        {
            LoadLocation(currentView.forwardExit, currentFacingDirection);
        }
        else
        {
            // Logs a message if no exit is available
            Debug.Log("Cannot move forward from this view: No exit defined.");
            // You might add sound effects or UI feedback here
        }
    }

    // Turns the player's facing direction left (counter-clockwise)
    public void RequestTurnLeft()
    {
        // Prevents turning if a transition is active
        if (_isTransitioning)
        {
            Debug.Log("GameManager: Cannot TurnLeft - currently transitioning.");
            return;
        }

        // Calculates the new facing direction
        currentFacingDirection = (CardinalDirection)(((int)currentFacingDirection + 3) % 4); // +3 is equivalent to -1 modulo 4
        Debug.Log("Turned Left. Now facing: " + currentFacingDirection);
        // Updates the view to reflect the new direction
        UpdateView();
    }

    // Turns the player's facing direction right (clockwise)
    public void RequestTurnRight()
    {
        // Prevents turning if a transition is active
        if (_isTransitioning)
        {
            Debug.Log("GameManager: Cannot TurnRight - currently transitioning.");
            return;
        }

        // Calculates the new facing direction
        currentFacingDirection = (CardinalDirection)(((int)currentFacingDirection + 1) % 4);
        Debug.Log("Turned Right. Now facing: " + currentFacingDirection);
        // Updates the view to reflect the new direction
        UpdateView();
    }

    // Turns the player's facing direction 180 degrees
    public void RequestTurnAround()
    {
        // Prevents turning if a transition is active
        if (_isTransitioning)
        {
            Debug.Log("GameManager: Cannot TurnAround - currently transitioning.");
            return;
        }

        // Calculates the new facing direction
        currentFacingDirection = GetOppositeDirection(currentFacingDirection);
        Debug.Log("Turned Around. Now facing: " + currentFacingDirection);
        // Updates the view to reflect the new direction
        UpdateView();
    }

    // Helper method to retrieve the correct DirectionalView based on the current facing direction
    private DirectionalView GetCurrentDirectionalView()
    {
        switch (currentFacingDirection)
        {
            case CardinalDirection.North:
                return currentLocation.northView;
            case CardinalDirection.East:
                return currentLocation.eastView;
            case CardinalDirection.South:
                return currentLocation.southView;
            case CardinalDirection.West:
                return currentLocation.westView;
            default:
                return new DirectionalView(); // Fallback - should not be reached with valid enum values
        }
    }

    // Helper method to get the opposite cardinal direction
    private CardinalDirection GetOppositeDirection(CardinalDirection dir)
    {
        switch (dir)
        {
            case CardinalDirection.North:
                return CardinalDirection.South;
            case CardinalDirection.East:
                return CardinalDirection.West;
            case CardinalDirection.South:
                return CardinalDirection.North;
            case CardinalDirection.West:
                return CardinalDirection.East;
            default:
                return CardinalDirection.North; // Fallback for unexpected values
        }
    }

    // Sets the system cursor to a custom cursor definition
    public void SetCursor(CursorDefinition cursor)
    {
        Cursor.SetCursor(cursor.Texture, cursor.Hotspot, CursorMode.Auto);
    }

    // Checks if there is a valid forward exit from the current view
    public bool CanMoveForward()
    {
        DirectionalView currentView = GetCurrentDirectionalView();
        return currentView.forwardExit != null;
    }

    // Initiates a visual transition to a new view image
    private void StartTransition(Sprite newViewImage)
    {
        // Sets the transitioning flag and notifies subscribers
        _isTransitioning = true;
        OnTransitioningStateChanged?.Invoke(true);
        OnTransitionStarted?.Invoke();

        Debug.Log("GameManager: Initiating visual transition.");

        // Configures and starts the transition effect
        transitionEffect.SetSprite(newViewImage);
        transitionEffect.Transition(() =>
        {
            OnTransitionCompleteInternal(); // Callback when the transition finishes
        });
    }

    // Called internally when a visual transition has completed
    private void OnTransitionCompleteInternal()
    {
        // Resets the transitioning flag and notifies subscribers
        _isTransitioning = false;
        OnTransitioningStateChanged?.Invoke(false);
        OnTransitionFinished?.Invoke();
        Debug.Log("GameManager: Visual transition completed.");
    }
}
