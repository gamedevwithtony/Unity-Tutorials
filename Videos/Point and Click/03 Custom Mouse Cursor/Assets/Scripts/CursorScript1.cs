using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript1 : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Vector2 cursorHotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.SetCursor(cursorTexture, cursorHotspot, cursorMode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, cursorHotspot, cursorMode);
    }

    // We'll handle mouse exit as well for completeness
    private void OnMouseExit()
    {
        Cursor.SetCursor(null, cursorHotspot, cursorMode);
    }
}
