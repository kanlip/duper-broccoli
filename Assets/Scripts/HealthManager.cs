using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Slider hpSlider;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMaxHp(int health)
    {
        hpSlider.maxValue = health;
        hpSlider.value = health;
    }

    public void setHp(int health)
    {
        hpSlider.value = health;
    }
}
