using UnityEngine;

public class UIManager : MonoBehaviour

{
    public int maxHealth=100;
    public int currentHealth;

    public GameObject buildList;
    public GameObject riverBuildList;
    public GameObject viewUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buildList.SetActive(false);
        riverBuildList.SetActive(false);
        viewUI.SetActive(false);
        currentHealth = maxHealth;

    }

    // Update is called once per frame
     public void ShowUI(GameObject uiElement)
    {
        buildList.SetActive(false);
        riverBuildList.SetActive(false);
        viewUI.SetActive(false);
        uiElement.SetActive(true);
        
    }

    public void LoseHealth(int health){
        currentHealth -= health;
        if(currentHealth <= 0){
            //Game Over
        }
    }
}
