using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript2 : MonoBehaviour
{
    public Texture2D[] cursorTextures;
    public Vector2 cursorHotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    public float animationSpeed;

    private float animationDelay;
    private int animationIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorTextures[animationIndex], cursorHotspot, cursorMode);
    }

    // Update is called once per frame
    void Update()
    {
        animationDelay += Time.deltaTime;
        if (animationDelay >= animationSpeed)
        {
            animationIndex = (animationIndex + 1) % cursorTextures.Length;
            animationDelay -= animationSpeed;
            Cursor.SetCursor(cursorTextures[animationIndex], cursorHotspot, cursorMode);
        }

    }
}
