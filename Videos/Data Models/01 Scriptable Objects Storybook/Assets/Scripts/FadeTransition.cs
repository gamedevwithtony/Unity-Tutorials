using System;
using UnityEngine;

public class FadeTransition : MonoBehaviour
{
    // References to the two image SpriteRenderers
    [SerializeField]
    private SpriteRenderer srImage1, srImage2;

    // References for the top and bottom visual layers during transition
    private SpriteRenderer srTLayer, srBLayer;

    // Flag for initial setup of the first image
    private bool firstRun = true;

    // Flag to trigger the fade animation
    private bool doFade;

    // Current time elapsed during the fade
    private float fadeTimer;
    // Duration of the fade transition
    [SerializeField]
    private float fadeDelay = 1.0f;

    // Action to perform once the fade completes
    private Action fadeAction;

    // Called once when the script starts
    void Start()
    {
        // Assign initial top and bottom layers
        srTLayer = srImage2;
        srBLayer = srImage1;

        // Set initial sorting orders for layers
        srTLayer.sortingOrder = 2; // Top layer
        srBLayer.sortingOrder = 1; // Bottom layer

        // Set initial colors; top is clear, bottom is visible
        srTLayer.color = Color.clear;
        srBLayer.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        // Press Space to initiate a fade for testing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fade();
        }

        // Process the fade animation if active
        if (doFade)
        {
            // Increment timer and clamp it to the fade duration
            fadeTimer += Time.deltaTime;
            fadeTimer = Mathf.Clamp(fadeTimer, 0f, fadeDelay);

            // The code below this comment block is broken down for understanding, but can be consolidated like shown here
            //   srTLayer.color = fadeDelay > 0
            //       ? new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp01(fadeTimer / fadeDelay))
            //       : Color.white;

            // Calculate fade progress (alpha from 0 to 1), handling zero delay
            float t = fadeDelay > 0 ? Mathf.Clamp01(fadeTimer / fadeDelay) : 1f;

            // Apply calculated alpha to the top layer
            Color fadeColor = new Color(1.0f, 1.0f, 1.0f, t);

            // Apply calculated alpha to the top layer
            srTLayer.color = fadeColor;

            // Check if fade animation is complete
            if (fadeTimer >= fadeDelay)
            {
                // Stop fading
                doFade = false;
                // Swap top and bottom layers for the next transition
                (srTLayer, srBLayer) = (srBLayer, srTLayer);
                // Reset new top layer to clear and reassign sorting orders
                srTLayer.color = Color.clear;
                srTLayer.sortingOrder = 2;
                srBLayer.sortingOrder = 1;
                // Execute the callback action if set
                fadeAction?.Invoke();
            }
        }
    }

    // Sets the sprite for the appropriate image renderer
    public void SetImage(Sprite sprite)
    {
        // On first run, set sprite for the initial bottom layer
        if (firstRun)
        {
            srImage1.sprite = sprite;
            firstRun = false;
        }
        else
        {
            // For subsequent runs, set sprite for the layer that will fade in (higher sort order)
            if (srImage1.sortingOrder == 1)
            {
                srImage2.sprite = sprite;
            }
            else
            {
                srImage1.sprite = sprite;
            }
        }
    }

    // Initiates the fade transition with an optional callback
    public void Fade(Action callback = null)
    {
        // Only start fade if not already fading
        if (!doFade)
        {
            doFade = true;      // Enable fade processing
            fadeTimer = 0f;     // Reset timer
            fadeAction = callback; // Store callback
        }
    }
}
