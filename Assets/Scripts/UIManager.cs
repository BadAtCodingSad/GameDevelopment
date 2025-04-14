using UnityEngine;

public class UIManager : MonoBehaviour

{

    public GameObject buildList;
    public GameObject viewUI;
    public GameObject workersUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buildList.SetActive(false);
        viewUI.SetActive(false);
        workersUI.SetActive(false);
        

    }

    // Update is called once per frame
     public void ShowUI(GameObject uiElement)
    {
        buildList.SetActive(false);
        viewUI.SetActive(false);
        workersUI.SetActive(false);
        uiElement.SetActive(true);
        
    }

    
}
