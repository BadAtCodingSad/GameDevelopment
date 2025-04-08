using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject Inventory;
    public Text WoodT;
    public Text FishT;
    public Text OreT;
    public Text ROreT;
    private int WoodCap=100;
    private int FishCap=100;
    private int OreCap=100;
    private int ROreCap=100;
    private int Wood=0;
    private int Fish=0;
    private int Ore=0;
    private int ROre=0;
    private bool MenuActivated;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Inventory.SetActive(false);
        MenuActivated=false;
        WoodT.text="Wood: 0/"+WoodCap;
        FishT.text="Fish: 0/"+FishCap;
        OreT.text="Ore: 0/"+OreCap;
        ROreT.text="Refined Ore: 0/"+ROreCap;

        
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
        WoodT.text="Wood: "+Wood+"/"+WoodCap;
        FishT.text="Fish: "+Fish+"/"+FishCap;
        OreT.text="Ore: "+Ore+"/"+OreCap;
        ROreT.text="Refined Ore: "+ROre+"/"+ROreCap;
    }
    public void increaseWoodCap(int amount){
        WoodCap+=amount;
        
    }
    public void increaseFishCap(int amount){
        FishCap+=amount;
        
    }
    public void increaseOreCap(int amount){
        OreCap+=amount;
        
    }
    public void increaseROreCap(int amount){
        ROreCap+=amount;
        
    }
    public void addWood(int amount){
        if(Wood+amount>WoodCap){
            Wood=WoodCap;
            //display message for being full
        }
        else{
            Wood+=amount;
        }
        
    }
    public void addFish(int amount){
        if(Fish+amount>FishCap){
            Fish=FishCap;
            //display message for being full
        }
        else{
            Fish+=amount;
        }
        
    }
    public void addOre(int amount){
        if(Ore+amount>OreCap){
            Ore=OreCap;
            //display message for being full
        }
        else{
            Ore+=amount;
        }
        
    }
    public void addROre(int amount){
        if(ROre+amount>ROreCap){
            ROre=ROreCap;
            //display message for being full
        }
        else{
            ROre+=amount;
        }
        
    }
    //not sure if we need a decrease on the cap;

}
