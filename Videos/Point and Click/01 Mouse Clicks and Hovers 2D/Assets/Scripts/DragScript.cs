using UnityEngine;

public class DragScript : MonoBehaviour
{
    Vector3 mousePosition;
    RaycastHit2D raycastHit2D;
    Transform clickObject;

    bool isMouseDown = false;

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

        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;

            raycastHit2D = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
            clickObject = raycastHit2D ? raycastHit2D.collider.transform : null;
        }

        if (Input.GetMouseButtonUp((0)))
        {
            isMouseDown = false;
        }

        if (isMouseDown && clickObject)
        {
            raycastHit2D = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
            if (raycastHit2D)
            {
                if (clickObject.GetInstanceID() == raycastHit2D.collider.transform.GetInstanceID())
                {
                    //clickObject.GetComponent<SpriteRenderer>().color = Color.red;

                    if (!isPlaying)
                    {
                        clickObject.GetComponent<Animator>().SetFloat("direction", 1f);
                        clickObject.GetComponent<Animator>().Play("GameConsole_Open", -1, 0);

                        isPlaying = true;
                    }

                    return;
                }
            }

            //clickObject.GetComponent<SpriteRenderer>().color = Color.white;

            if (isPlaying)
            {
                clickObject.GetComponent<Animator>().SetFloat("direction", -1f);
                clickObject.GetComponent<Animator>().Play("GameConsole_Open", -1, 1);

                isPlaying = false;
            }
        }
    }
}
