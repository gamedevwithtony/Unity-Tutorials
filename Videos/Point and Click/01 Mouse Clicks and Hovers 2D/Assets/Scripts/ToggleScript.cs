using UnityEngine;

public class ToggleScript : MonoBehaviour
{
    Vector3 mousePosition;
    RaycastHit2D raycastHit2D;
    Transform clickObject;

    bool isSelected = false;

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
                //isSelected = clickObject.GetComponent<SpriteRenderer>().color == Color.red;

                //clickObject.GetComponent<SpriteRenderer>().color = isSelected ? Color.white : Color.red;

                Animator animator = clickObject.GetComponent<Animator>();
                AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

                if (clipInfo.Length > 0)
                {
                    isSelected = stateInfo.normalizedTime >= 1.0f;
                }

                animator.SetFloat("direction", isSelected ? -1f : 1f);
                animator.Play("GameConsole_Open", -1, isSelected ? 1 : 0);
            }
        }
    }
}
