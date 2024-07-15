
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : MonoBehaviour
{
    public int currentHealth = 100;
    public int maxHealth = 100;
    void Start()
    {
        
    }

    void Update()
    {

    }

    public void takeDamage(int healthPoints)
    {
        currentHealth -= healthPoints;
        //healthBar.fillAmount = currentHealth / maxHealth;
        if (currentHealth == 0)
        {
            //show Death screen
        }
    }

    public void heal(int healthPoints)
    {
        currentHealth += healthPoints;
        currentHealth = Mathf.Clamp(healthPoints, 0, maxHealth);
        //healthBar.fillAmount = currentHealth / maxHealth;
    }
}
