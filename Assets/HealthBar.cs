using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class HealthBar : MonoBehaviour
{
    public Slider slider;
    int pollution=0;
    void Start()
    {
        
        slider.maxValue = 100;
    }

    void Update()
    {
        // Update the slider value based on the pollution level
        slider.value = pollution;
    }

    public void incrementPollution(int pol){//can be used to both increase and decrease depends on action
        
        if(pollution+ pol>=100){
            //call game over
            Debug.Log("Game Over");
            slider.value = 100;
            return;}
            else{
            pollution += pol;
            slider.value = pollution;}

    }
    

  
}
