using UnityEngine;

public class MouseScript : MonoBehaviour
{
    private Vector3 endPos;
    private float speed = 2.0f;
    private float smackSpeed = 15.0f;
    private int points = 1;
    private bool isSmacked;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endPos, step);
        if (Vector3.Distance(transform.position, endPos) < 0.001f)
        {
            Destroy(gameObject);
        }
    }

    public void SetEndPosition(Vector3 pos)
    {
        this.endPos = pos;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetSmackSpeed(float speed)
    {
        this.smackSpeed = speed;
    }

    public void SetPoints(int points)
    {
        this.points = points;
    }

    public int GetPoints()
    {
        return this.points;
    }

    public bool HasBeenSmacked()
    {
        return this.isSmacked;
    }

    public void SmackThisMouse()
    {
        this.isSmacked = true;
        this.speed = this.smackSpeed;
        GetComponent<Animator>().speed = 2.0f;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
    }
}
