using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public GameObject digitContainer;
    public Image[] digitPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetScore(int score)
    {
        float xPos = 0f;
        string scoreText = score.ToString();//.PadLeft(3, '0');

        foreach (Transform child in digitContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (char digit in scoreText.ToCharArray())
        {
            Image image = Instantiate(digitPrefabs[digit - '0']);
            image.transform.SetParent(digitContainer.transform);
            image.rectTransform.anchoredPosition = new Vector3(xPos, 0f, 0f);
            image.rectTransform.localScale = Vector3.one;
            xPos += image.rectTransform.sizeDelta.x + 2f;
        }
    }
}
