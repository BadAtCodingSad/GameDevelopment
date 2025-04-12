using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject Inventory;
    public Text WoodT;
    public Text FishT;
    public Text MetalT;
    
    public Text OilT;
  
    private int Wood=0;
    private int Fish=50;
    private int Oil=0;
    private int Metal=0;
    private bool MenuActivated;
    public static InventoryManager Instance;

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
        WoodT.text="Wood: 0/";
        FishT.text="Fish: 0/";
        MetalT.text="Metal: 0/";
        OilT.text="Oil Ore: 0/";

        
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
        

        WoodT.text="Wood: "+Wood;
        FishT.text="Fish: "+Fish;
        MetalT.text="Metal: "+Metal;
        OilT.text="Oil: "+Oil;
    }
  
    public void addWood(int amount){//basically harvest from a tile
        
            Wood+=amount;
        }
        
    public void addMetal(int amount){
        
            Metal+=amount;
        }
    public void addOil(int amount){
        
            Oil+=amount;
        }
    public void addFish(int amount){
        
            Fish+=amount;
    
        
    }
   
    public void removeWood(int amount){
            if(Wood-amount<0){
                Debug.Log("Not enough wood");
                return;
            }
            Wood-=amount;
        }
    public void removeMetal(int amount){
        if(Metal-amount<0){
                Debug.Log("Not enough metal");
                return;
            }
            Metal-=amount;
        }
    public void removeOil(int amount){//basically use up for an item.
        if(Oil-amount<0){
                Debug.Log("Not enough oil");
                return;
            }
            Oil-=amount;
        }
    public void removeFish(int amount){
        if(Fish-amount<0){
                Debug.Log("Not enough fish");
                return;
            }
            Fish-=amount;
        }

    public int getWood(){
        return Wood;
    }
    public int getFish(){
        return Fish;
    }
    public int getMetal(){
        return Metal;
    }
    public int getOil(){
        return Oil;
    }
   
    
}
