using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; // For Image component

public class FadeTransition : MonoBehaviour, ITransitionEffect
{
    // Reference to the first UI Image component
    [SerializeField] private Image image1;
    // Reference to the second UI Image component
    [SerializeField] private Image image2;

    // Duration of the fade effect in seconds
    [SerializeField] private float fadeDuration = 1.0f;

    // Stores the current fade coroutine to allow stopping it
    private Coroutine fadeCoroutine;
    // Callback action invoked when the fade transition completes
    private Action onFadeCompleted;

    // Start is called before the first frame update
    private void Start()
    {
        // No specific logic is needed on Start for this component's functionality
    }

    // Called when the GameObject is destroyed
    private void OnDestroy()
    {
        // Stop any active fade coroutine to prevent errors
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null; // Clear the reference
        }
        // Clear the completion callback
        onFadeCompleted = null;
    }

    // Update is called once per frame
    private void Update()
    {
        // Optional: Example code for testing fade with spacebar
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Transition();
        //}
    }

    // Determines and returns the image currently on top
    private Image GetTopImage()
    {
        return image1.transform.GetSiblingIndex() == 0 ? image1 : image2;
    }

    // Determines and returns the image currently on the bottom
    private Image GetBottomImage()
    {
        return image1.transform.GetSiblingIndex() == 0 ? image2 : image1;
    }

    // Sets the alpha (transparency) of a given image
    private void SetImageAlpha(Image img, float alpha)
    {
        if (img != null)
        {
            Color color = img.color;
            color.a = alpha;
            img.color = color;
        }
    }

    // Sets the sprite for the image that will be faded in
    public void SetSprite(Sprite sprite)
    {
        // If duration is zero, display immediately instead of fading
        if (fadeDuration == 0f)
        {
            DisplayImmediately(sprite);
            return;
        }

        // Get references to the images that will fade in and out
        Image fadingInImage = GetBottomImage();
        Image fadingOutImage = GetTopImage();

        // Transfer the sprite from the fading-in image to the fading-out image
        fadingOutImage.sprite = fadingInImage.sprite;

        // Set the new sprite on the image that will fade in
        fadingInImage.sprite = sprite;

        // Initialize alpha values for the start of the fade
        SetImageAlpha(fadingInImage, 0f);
        SetImageAlpha(fadingOutImage, 1f);
    }

    // Sets the duration for the fade transition, clamping it to be non-negative
    public void SetDuration(float duration)
    {
        fadeDuration = Mathf.Clamp(duration, 0, float.MaxValue);
    }

    // Instantly displays a sprite without any transition
    public void DisplayImmediately(Sprite sprite)
    {
        Image targetImage = GetBottomImage();

        // Set the sprite and make it fully opaque
        targetImage.sprite = sprite;
        SetImageAlpha(targetImage, 1f);
    }

    // Starts the fade transition
    public void Transition(Action callback = null)
    {
        // Store the callback to be invoked on completion
        onFadeCompleted = callback;

        // Stop any existing fade transition before starting a new one
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // Start the fade coroutine
        fadeCoroutine = StartCoroutine(DoFade());
    }

    // Coroutine that handles the actual fading animation
    private IEnumerator DoFade()
    {
        // Get references to the images involved in the fade
        Image fadingInImage = GetBottomImage();
        Image fadingOutImage = GetTopImage();

        // Ensure initial alpha states are correct
        SetImageAlpha(fadingInImage, 0f);
        SetImageAlpha(fadingOutImage, 1f);

        // Change sibling indices to bring the fading-in image to the front
        fadingInImage.transform.SetSiblingIndex(1);
        fadingOutImage.transform.SetSiblingIndex(0);

        // Perform the fade over time if fadeDuration is greater than 0
        if (fadeDuration > 0)
        {
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float progress = timer / fadeDuration;

                // Update the alpha of the fading-in image
                SetImageAlpha(fadingInImage, progress);

                yield return null; // Wait for the next frame
            }
        }

        // Ensure the fading-in image is fully opaque at the end
        SetImageAlpha(fadingInImage, 1f);

        // Invoke the completion callback if it exists
        onFadeCompleted?.Invoke();
    }
}
