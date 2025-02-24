using UnityEngine;

public class ClickReleaseScript : MonoBehaviour
{
    Vector3 mousePosition;
    RaycastHit2D raycastHit2D;
    Transform clickObject;

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
            raycastHit2D = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
            clickObject = raycastHit2D ? raycastHit2D.collider.transform : null;

            if (clickObject)
            {
                //clickObject.GetComponent<SpriteRenderer>().color = Color.red;
                clickObject.GetComponent<Animator>().SetFloat("direction", 1f);
                clickObject.GetComponent<Animator>().Play("GameConsole_Open", -1, 0f);
            }
        }

        if (Input.GetMouseButtonUp((0)))
        {
            if (clickObject)
            {
                //clickObject.GetComponent<SpriteRenderer>().color = Color.white;
                clickObject.GetComponent<Animator>().SetFloat("direction", -1f);
                clickObject.GetComponent<Animator>().Play("GameConsole_Open", -1, 1f);
            }
        }
    }
}
