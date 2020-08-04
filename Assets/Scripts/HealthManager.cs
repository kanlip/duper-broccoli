using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Slider hpSlider;

    private int maxHealth = 100;
    private int currentHealth;

    private Button healthPotionButton;
    private int healthPotion = 10;
    private Text amountOfPotion;
    private int healthRestore = 10;

    public GameObject gameOverPanel;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        hpSlider.maxValue = maxHealth;
        hpSlider.value = currentHealth;

        healthPotionButton = GameObject.FindWithTag("HealthPotion").GetComponent<Button>();
        amountOfPotion = GameObject.FindWithTag("HealthPotionAmount").GetComponent<Text>();

        //display the amount of potion
        amountOfPotion.text = healthPotion.ToString();
      
        //when health potion is click
        healthPotionButton.onClick.AddListener(clickRestoreHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            //minus current health by 20 when space is pressed
            currentHealth -= 20;

            //set the current hp after damage
            hpSlider.value = currentHealth;
        }

        Debug.Log("UPDATE: " + currentHealth);

        //show the game over panel if user hp = 0
        if (currentHealth <= 0)
        {
            gameOverPanel.gameObject.SetActive(true);
        }
    }

    void clickRestoreHealth()
    {
        //if there is no more potion
        if (healthPotion < 2)
        {
            //make button not interactable
            healthPotionButton.interactable = false;
        }

        //if player health is not full health
        if (currentHealth < 100)
        {
            //restore health
            currentHealth += healthRestore;

            //minus off one potion
            healthPotion -= 1;
            //set the current health after restore
            hpSlider.value = currentHealth;

            //get the current amount of potion 
            amountOfPotion.text = healthPotion.ToString();
        }

        Debug.Log("RESTORE: " + currentHealth);
        
    }
}
