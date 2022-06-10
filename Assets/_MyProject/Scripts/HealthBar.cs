using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
 
    public void setSliderValue(float health)
    {
        slider.value = health;
        
    }
    public void setMaxSliderValue(float maxHealth)
    {
        slider.maxValue = maxHealth;
    }
}
