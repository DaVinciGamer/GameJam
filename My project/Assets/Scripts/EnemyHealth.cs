using Pathfinding;
using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Callbacks;
using UnityEngine;
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 3f; // current health and max-health values of enemy
    [SerializeField] HealthBar healthBar;
    [SerializeField] GameObject healthBarGameObject;

    [SerializeField] PlayerController playerController;
    Rigidbody2D rb;
    CircleCollider2D circleCollider;

    [SerializeField] GameObject destroyedEnemy;
    [SerializeField] GameObject intactEnemy;
    [SerializeField] GameObject intactEnemyTransitioned;

    public bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<HealthBar>();
        destroyedEnemy.SetActive(false);
        intactEnemy.SetActive(true);
        circleCollider = GetComponent<CircleCollider2D>();
        healthBarGameObject.SetActive(false);
    }
    private void Update()
    {
        showHealthBar();
    }
    private void showHealthBar()
    {
        if (health < maxHealth && !isDead)
        {
            healthBarGameObject.SetActive(true);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        //Debug.Log("take damage");
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Debug.Log("---------test is health < 0");
            Die();
        }
    }

    void Die()
    {
        Debug.Log("testDIE");
        isDead = true;
        destroyedEnemy.SetActive(true); //Enable the destroyed Game Object
        intactEnemy.SetActive(false); //Disable the intact Game Object
        intactEnemyTransitioned.SetActive(false);
        AIPath aiPath = GetComponentInParent<AIPath>();
        aiPath.enabled = false; //Disable the AIPath component so the destroyed Game Object stays in place
        healthBarGameObject.SetActive(false); //Disable the health bar
        circleCollider.enabled = false;
    }
}