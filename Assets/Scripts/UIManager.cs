using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour

{

    public GameObject buildList;
    public GameObject viewUI;
    public GameObject workersUI;
    public GameObject changesView;
    public TextMeshProUGUI changesText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    void Update(){

    }
    void Start()
    {
        buildList.SetActive(false);
        viewUI.SetActive(true);
        workersUI.SetActive(false);
        changesView.SetActive(false);

    }

    // Update is called once per frame
     public void ShowUI(GameObject uiElement)
    {
        buildList.SetActive(false);
        viewUI.SetActive(false);
        workersUI.SetActive(false);
        uiElement.SetActive(true);
        
    }
    public void ShowChange(string change) 
    {
        changesText.text = change;
        changesView.SetActive(true);
    }
    public void HideChanges() 
    {
        changesView.SetActive(false);
    }
}

