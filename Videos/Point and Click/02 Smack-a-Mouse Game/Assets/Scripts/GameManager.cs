using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float mouseSpeedMin = 2.0f;
    public float mouseSpeedMax = 10.0f;
    public float mouseSpeedSmack = 15.0f;
    public float mouseSpawnDelay = 3.0f;

    public GameObject mousePrefab;

    ScoreScript scoreScript;

    int score;

    Transform mouseObject;

    bool isMouseDown = false;

    bool hitMouse = false;

    float mouseSpawnTimer;

    string previousLayer;

    private IEnumerator coroutine;

    [System.Serializable]
    public struct SpawnStruct
    {
        public string layerName;
        public int sortOrder;
        public Vector3 mouseStartPos;
        public Vector3 mouseStartRot;
        public Vector3 mouseEndPos;
    }
    public SpawnStruct[] spawnData;


    // Start is called before the first frame update
    void Start()
    {
        scoreScript = GetComponent<ScoreScript>();

        scoreScript.SetScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseButtonDown();
        }

        if (Input.GetMouseButtonUp(0))
        {
            HandleMouseButtonUp();
        }

        if (isMouseDown)
        {
            if (mouseObject && !hitMouse && !mouseObject.GetComponent<MouseScript>().HasBeenSmacked())
            {
                score += mouseObject.GetComponent<MouseScript>().GetPoints();
                mouseObject.GetComponent<MouseScript>().SmackThisMouse();
                scoreScript.SetScore(score);
                hitMouse = true;
            }
        }

        RandomMouse();
    }

    private void HandleMouseButtonDown()
    {
        isMouseDown = true;
        hitMouse = false;
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
        mouseObject = raycastHit2D ? raycastHit2D.collider.transform : null;
    }

    private void HandleMouseButtonUp()
    {
        isMouseDown = false;
        hitMouse = false;
    }

    private void RandomMouse()
    {
        mouseSpawnTimer += Time.deltaTime;
        if (mouseSpawnTimer >= mouseSpawnDelay)
        {
            int index;
            int passes = 0;

            while (true)
            {
                index = Random.Range(0, spawnData.Length);
                if (spawnData[index].layerName != previousLayer)
                {
                    previousLayer = spawnData[index].layerName;
                    break;
                }

                if (++passes >= 5) break;
            }

            int points = 1;

            float speed = Random.Range(mouseSpeedMin, mouseSpeedMax);

            GameObject mouse = Instantiate(mousePrefab);
            mouse.transform.position = spawnData[index].mouseStartPos;
            mouse.transform.rotation = Quaternion.Euler(spawnData[index].mouseStartRot);
            mouse.GetComponent<MouseScript>().SetEndPosition(spawnData[index].mouseEndPos);
            mouse.GetComponent<MouseScript>().SetPoints(points);
            mouse.GetComponent<MouseScript>().SetSpeed(speed);
            mouse.GetComponent<MouseScript>().SetSmackSpeed(mouseSpeedSmack);

            GameObject layer = GameObject.Find(spawnData[index].layerName);
            if (layer)
            {
                int layerOrder = layer.GetComponent<SpriteRenderer>().sortingOrder;
                layer.GetComponent<SpriteRenderer>().sortingOrder = spawnData[index].sortOrder;
                coroutine = ResetLayer(spawnData[index].layerName, layerOrder, 2.0f);
                StartCoroutine(coroutine);
            }
            
            mouseSpawnTimer = 0f;
        }
    }

    private IEnumerator ResetLayer(string layerName, int layerOrder, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GameObject layer = GameObject.Find(layerName);
        if (layer)
        {
            layer.GetComponent<SpriteRenderer>().sortingOrder = layerOrder;
        }
    }
}
