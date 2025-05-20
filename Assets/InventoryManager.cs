using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public GameObject Inventory;
    public TextMeshProUGUI WoodT;
    public TextMeshProUGUI FishT;
    public TextMeshProUGUI MetalT;
    public TextMeshProUGUI EnergyT;
    public TextMeshProUGUI OilT;
  
    private int Wood = 0;
    private int Fish = 12;
    private int Oil = 0;
    private int Metal = 0;
    private bool MenuActivated;
    public static InventoryManager Instance;
    private GameManager gameManager;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Inventory.SetActive(false);
        MenuActivated=false;
        WoodT.text="Woody: 36 0/";
        FishT.text="Fish: 45 0/";
        MetalT.text="Metal: 45 0/";
        OilT.text="Oil : 0/";
        EnergyT.text = "Energy : 0/";


    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Inventory")&& MenuActivated){

           Inventory.SetActive(false);
           MenuActivated=false;
        }
        else if(Input.GetButtonDown("Inventory")&& !MenuActivated){
            
           Inventory.SetActive(true);
           MenuActivated=true;
        }
        if(GameManager.instance != null &&  gameManager ==  null)
            gameManager = GameManager.instance;
        Wood = gameManager.wood;
        Fish = gameManager.fish;
        Oil = gameManager.oil;
        Metal = gameManager.metal;

        WoodT.text=Wood.ToString();
        FishT.text=Fish.ToString();
        MetalT.text=Metal.ToString();
        OilT.text=Oil.ToString();
        EnergyT.text=gameManager.energy.ToString();
    }
}
