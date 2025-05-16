using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildInfo : MonoBehaviour
{
    public TextMeshProUGUI metal;
    public TextMeshProUGUI oil;
    public TextMeshProUGUI wood;
    public TextMeshProUGUI fish;
    public TextMeshProUGUI buffs;
    public TextMeshProUGUI buildingName;
    public Image icon;
    public Buildable buildable;
    public static GameManager instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Build() 
    {
        //GameManager.instance.BuildUpdate(buildable);

       

    

        
        GameManager.instance.BuildAction(buildable);
    }
}
