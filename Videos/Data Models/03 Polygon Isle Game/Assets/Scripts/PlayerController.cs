using UnityEngine;

// Handles player input for movement and turning in the game world
public class PlayerController : MonoBehaviour
{
    // Reference to the GameManager singleton instance
    private GameManager gameManager;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Gets the GameManager instance
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            // If GameManager isn't found, logs an error and disables this component
            Debug.LogError("GameManager not found! Make sure it's in the scene.");
            enabled = false;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        // No specific initialization needed here for this script
    }

    // Update is called once per frame
    private void Update()
    {
        // Checks if the GameManager is ready to accept player input (i.e., not transitioning)
        if (gameManager.CanAcceptPlayerInput)
        {
            // Detects Up Arrow key press to request moving forward
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                gameManager.RequestMoveForward();
            }

            // Detects Left Arrow key press to request turning left
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                gameManager.RequestTurnLeft();
            }

            // Detects Right Arrow key press to request turning right
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                gameManager.RequestTurnRight();
            }

            // Detects Down Arrow key press to request turning around
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                gameManager.RequestTurnAround();
            }
        }
    }
}
