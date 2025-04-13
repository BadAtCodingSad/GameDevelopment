using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

public class Population : MonoBehaviour
{
    public Text population;
    
    public Text fedpopulation;
    
    public int populationCount=100;
    public int fedPopulationCount=0;

     
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        population.text = "Population: "+populationCount;
        fedpopulation.text = "Population fed currently: "+0+"%";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)){
            feedpopulation();
            
        }
        
    }

    public void increasePopulation()
    {
        populationCount += Mathf.RoundToInt(populationCount * 0.1f);
        population.text = "Population: "+populationCount;
        fedSetZero();
    }

    public void decreasePopulation()
    {
        populationCount=fedPopulationCount;
        population.text = "Population: "+populationCount;
        fedSetZero();
    }
    public void changePopulation()//call at the end of turn
    {
        if(fedPopulationCount==0){
            //call endgame
        }
        else if (populationCount==fedPopulationCount){
            increasePopulation();
        }
        else{
            decreasePopulation();
        }
        
    }

    public void feedpopulation()//feed population at anytime of the turn
    {
        if (InventoryManager.Instance.getFish() >= populationCount && fedPopulationCount!=populationCount)
        {
            //if we have enough fish to feed the population
            InventoryManager.Instance.removeFish(populationCount);
            fedPopulationCount = populationCount;
            fedpopulation.text = "Fed Population: "+((float)fedPopulationCount/populationCount)*100+"%";
        
        }
        else if (InventoryManager.Instance.getFish() == 0 && fedPopulationCount!=populationCount )
        {
            
            Debug.Log("Aquire food to feed the population");
        }
        else if (InventoryManager.Instance.getFish() < populationCount && InventoryManager.Instance.getFish() > 0)
        {
            fedPopulationCount = InventoryManager.Instance.getFish();
            fedpopulation.text = "Fed Population: "+((float)fedPopulationCount/populationCount)*100+"%";
            InventoryManager.Instance.removeFish(InventoryManager.Instance.getFish());
            
    
        }
        else if (fedPopulationCount==populationCount)
        {
            Debug.Log("Population already fed");
        }
        else
        {
            Debug.Log("An error has occured");
        }

        
    }
    public void fedSetZero(){
        //needs to be called after each turn
        fedPopulationCount=0;
        fedpopulation.text = "Population fed currently: "+0;
    }
}
