using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ObjectivesManager : MonoBehaviour
{
    public Image[] checkmarks; 
    public TextMeshProUGUI[] descriptions; 
    public int TownsObj=3;
    public int SustainableObj=15;

    private bool[] completed = new bool[4];
    bool active= false;
    public RectTransform notificationPanel; 
    public TextMeshProUGUI notificationText; 
    private Coroutine notificationCoroutine;
    public GameObject objectivespanel;

    void Start(){
        objectivespanel.SetActive(false);
        
        foreach (Image check in checkmarks){
            check.enabled=false;
        }
 
    }

    void Update()
    {
    if (Input.GetKeyDown(KeyCode.Space))
        {
            objectivespanel.SetActive(!objectivespanel.activeSelf);
        }
        
       
        if (!completed[0] && GameManager.instance.noTowns >= TownsObj)
        {
            completed[0] = true;
            checkmarks[0].enabled = true;
            GameManager.instance.objectivesDone++;
            StartCoroutine(SlideNotification("Objective 1 Completed!"));
        }

     
        if (!completed[1] && GameManager.instance.noSBuilding >= SustainableObj)
        {
            completed[1] = true;
            checkmarks[1].enabled = true;
            GameManager.instance.objectivesDone++;
            StartCoroutine(SlideNotification("Objective 2 Completed!"));
        }

       
        // if (!completed[2] &&  condition)
        // {
        //     completed[2] = true;
        //     checkmarks[2].enabled = true;
        //     GameManager.instance.ObjectivesDone++;
        // }

  
        // if (!completed[3] && condition)
        // {
        //     completed[3] = true;
        //     checkmarks[3].enabled = true;
        //     GameManager.instance.ObjectivesDone++;
        // }
    }
    private IEnumerator SlideNotification(string message)
{
    notificationText.text = message;
    float startX = 2205f;
    float slideInX = 1615f;
    float slideOutX = 2205f;
    float duration = 0.5f;

    // Move to start position
    notificationPanel.anchoredPosition = new Vector2(startX, notificationPanel.anchoredPosition.y);

    // Slide in
    float t = 0;
    Debug.Log("Slide in");
    while (t < 1)
    {
        t += Time.deltaTime / duration;
        float x = Mathf.Lerp(startX, slideInX, t);
        notificationPanel.anchoredPosition = new Vector2(x, notificationPanel.anchoredPosition.y);
        yield return null;
    }
    notificationPanel.anchoredPosition = new Vector2(slideInX, notificationPanel.anchoredPosition.y);

    // Wait 5 seconds
    yield return new WaitForSeconds(5f);

    // Slide out (to right)
    t = 0;
     Debug.Log("Slide out");
    float currentX = notificationPanel.anchoredPosition.x;
    while (t < 1)
    {
        t += Time.deltaTime / duration;
        float x = Mathf.Lerp(currentX, slideOutX, t);
        notificationPanel.anchoredPosition = new Vector2(x, notificationPanel.anchoredPosition.y);
        yield return null;
    }
    notificationPanel.anchoredPosition = new Vector2(slideOutX, notificationPanel.anchoredPosition.y);
}
}