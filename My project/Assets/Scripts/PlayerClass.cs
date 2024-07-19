
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerClass : MonoBehaviour
{
    public int currentHealth = 100;
    public int maxHealth = 100;

    public void takeDamage(int healthPoints)
    {
        currentHealth -= healthPoints;
        //healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void heal(int healthPoints) // not used in Game
    {
        currentHealth += healthPoints;
        currentHealth = Mathf.Clamp(healthPoints, 0, maxHealth);
    }
}
