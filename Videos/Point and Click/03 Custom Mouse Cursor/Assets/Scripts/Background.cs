using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private Animator animator;

    private bool allowAnimation = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && allowAnimation)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D raycastHit2D = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
            Transform hitObject = raycastHit2D ? raycastHit2D.collider.transform : null;

            if (hitObject)
            {
                animator.Play("Background", -1, 0f);
                allowAnimation = false;
            }
        }
    }

    public void ResetAnimation()
    {
        allowAnimation = true;
    }
}
