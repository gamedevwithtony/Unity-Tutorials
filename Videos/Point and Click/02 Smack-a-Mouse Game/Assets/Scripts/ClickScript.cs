using UnityEngine;

public class ClickScript : MonoBehaviour
{
    public GameObject prefabCatsPaw;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            GameObject catsPaw = GameObject.Find("CatsPaw");
            if (catsPaw)
            {
                Destroy(catsPaw);
            }
            
            catsPaw = Instantiate(prefabCatsPaw);
            catsPaw.name = "CatsPaw";
            catsPaw.transform.position = new Vector3(mouseRay.origin.x, mouseRay.origin.y, 0f);
        }
    }
}
