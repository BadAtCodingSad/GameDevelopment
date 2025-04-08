using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void setHealth(int health){
        slider.value = health;

    }
    public void setMaxHealh(int health){
        slider.maxValue = health;
        slider.value = health;
    }

  
}
