using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
 
    //SLIDER DEGERI ATA
    public void setSliderValue(float health)
    {
        slider.value = health;
        
    }
    //MAKSIMUM SLIDER DEGERI ATA
    public void setMaxSliderValue(float maxHealth)
    {
        slider.maxValue = maxHealth;
    }
}
