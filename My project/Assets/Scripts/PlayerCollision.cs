using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollision : MonoBehaviour
{
    public PlayerClass playerClass;
    public Slider healthSlider;
    public EnemyDamage enemyDamage;
    public GameOver gameOver;
    private MusicController MusicController;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            try
            {
                // Reduziere die Gesundheit des Spielers
                playerClass.currentHealth -= enemyDamage.damageAmount;

                // Aktualisiere den Slider-Wert
                healthSlider.value = (float)playerClass.currentHealth / playerClass.maxHealth;

            }
            catch
            {

            }
        }

    }

    void Update()
    {
        if (playerClass.currentHealth <= 0)
        {
            ShowGameOverPanel();
        }
    }
    private void ShowGameOverPanel()
    {
        if (gameOver != null && gameOver.gameOverCanvas != null)
        {
            gameOver.gameOverCanvas.SetActive(true);
            MusicController.Instance.FadeTo(3);
        }
        else
        {
            Debug.LogError("GameOver reference or GameOverPanel is not set in the Inspector.");
        }
    }
}
