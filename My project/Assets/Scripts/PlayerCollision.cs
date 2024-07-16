using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollision : MonoBehaviour
{
    public PlayerClass playerClass;
    public Slider healthSlider;
    public EnemyDamage enemyDamage;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // Reduziere die Gesundheit des Spielers
            playerClass.currentHealth -= enemyDamage.damageAmount;

            // Aktualisiere den Slider-Wert
            healthSlider.value = (float)playerClass.currentHealth / playerClass.maxHealth;
        }
    }
}
