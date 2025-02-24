using UnityEngine;

public class HoverScript : MonoBehaviour
{
    Vector3 mousePosition;
    RaycastHit2D raycastHit2D;
    Transform prevHoverObject, nextHoverObject;

    bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;

        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);

        prevHoverObject = nextHoverObject;

        raycastHit2D = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
        nextHoverObject = raycastHit2D ? raycastHit2D.collider.transform : null;

        if (nextHoverObject)
        {
            //nextHoverObject.GetComponent<SpriteRenderer>().color = Color.red;

            if (!isPlaying)
            {
                nextHoverObject.GetComponent<Animator>().SetFloat("direction", 1f);
                nextHoverObject.GetComponent<Animator>().Play("GameConsole_Open", -1, 0);

                isPlaying = true;
            }

            if (prevHoverObject && nextHoverObject && prevHoverObject.GetInstanceID() != nextHoverObject.GetInstanceID())
            {
                //prevHoverObject.GetComponent<SpriteRenderer>().color = Color.white;
                prevHoverObject.GetComponent<Animator>().SetFloat("direction", -1f);
                prevHoverObject.GetComponent<Animator>().Play("GameConsole_Open", -1, 1);

                isPlaying = false;
            }
        }
        else
        {
            if (prevHoverObject)
            {
                //prevHoverObject.GetComponent<SpriteRenderer>().color = Color.white;
                prevHoverObject.GetComponent<Animator>().SetFloat("direction", -1f);
                prevHoverObject.GetComponent<Animator>().Play("GameConsole_Open", -1, 1);
            }

            isPlaying = false;
        }
    }
}
