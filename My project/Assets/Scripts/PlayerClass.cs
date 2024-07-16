
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerClass : MonoBehaviour
{
    public int currentHealth = 100;
    public int maxHealth = 100;

    void Start()
    {

    }

    void Update()
    {
        // if (currentHealth <= 0)
        // {
        //     Debug.Log("Test");
        //     gameOverPanel.SetActive(true);
        //     Time.timeScale = 0;
        // }
    }

    public void takeDamage(int healthPoints)
    {
        currentHealth -= healthPoints;
        //healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void heal(int healthPoints)
    {
        currentHealth += healthPoints;
        currentHealth = Mathf.Clamp(healthPoints, 0, maxHealth);
        //healthBar.fillAmount = currentHealth / maxHealth;
    }
}
