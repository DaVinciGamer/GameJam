using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 3f;
    [SerializeField] HealthBar healthBar;

    [SerializeField] PlayerController playerController;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<HealthBar>();
    }

    private void Start()
    {
        playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        playerController.updateScore(1);
        Destroy(gameObject);
    }
}