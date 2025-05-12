using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkleEffect : MonoBehaviour
{
    public GameObject sparklePrefab;

    public float sparkleInterval1;
    public float sparkleInterval2;
    public Vector2 sparkleOffset = Vector2.zero;

    private float sparkleInterval;
    private float sparkleDelay;

    private Vector3 mousePosition;
    private Vector3 lastMousePosition;

    // Start is called before the first frame update
    void Start()
    {
        sparkleInterval = sparkleInterval2;
        mousePosition = Input.mousePosition;
        lastMousePosition = mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;

        sparkleDelay += Time.deltaTime;
        if (sparkleDelay >= sparkleInterval)
        {
            mousePosition.x = mousePosition.x + sparkleOffset.x;
            mousePosition.y = mousePosition.y + sparkleOffset.y;

            Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);

            GameObject sparkle = Instantiate(sparklePrefab);

            sparkle.transform.position = new Vector3(mouseRay.origin.x, mouseRay.origin.y, 0f);

            sparkleDelay -= sparkleInterval;
        }

        if (mousePosition != lastMousePosition)
        {
            lastMousePosition = mousePosition;
            sparkleInterval = sparkleInterval1;
        }
        else
        {
            sparkleInterval = sparkleInterval2;
        }
    }
}
