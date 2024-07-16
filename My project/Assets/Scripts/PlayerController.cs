using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public InputAction LeftAction;
    public InputAction RightAction;
    public InputAction UpAction;
    public InputAction DownAction;
    public InputAction ShootAction;

    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileSpeed = 10.0f;
    public int score = 0;
    public int currentHealth = 100;
    public int maxHealth = 100;
    public Image healthBar;

    public TMP_Text scoreText;

    void Start()
    {
        LeftAction.Enable();
        RightAction.Enable();
        UpAction.Enable();
        DownAction.Enable();
        ShootAction.Enable();

        scoreText.text = "Score: " + score.ToString();
    }

    void Update()
    {
        Vector2 move = Vector2.zero;

        if (LeftAction.IsPressed())
        {
            move.x = -1f;
        }
        else if (RightAction.IsPressed())
        {
            move.x = 1f;
        }

        if (UpAction.IsPressed())
        {
            move.y = 1f;
        }
        else if (DownAction.IsPressed())
        {
            move.y = -1f;
        }

        if (move != Vector2.zero)
        {
            move.Normalize();
        }

        float currentSpeed = moveSpeed;

        transform.position += (Vector3)move * currentSpeed * Time.deltaTime;

        if (ShootAction.triggered)
        {
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0f;

        Vector3 direction = (mousePosition - projectileSpawnPoint.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * projectileSpeed;
    }

    public void updateScore(int points)
    {
        score = score + points;
        scoreText.text = "Score: " + score.ToString();
    }

    public void takeDamage(int healthPoints)
    {
        currentHealth -= healthPoints;
        healthBar.fillAmount = currentHealth / maxHealth;
        if (currentHealth == 0)
        {
            //show Death screen
        }
    }

    public void heal(int healthPoints)
    {
        currentHealth += healthPoints;
        currentHealth = Mathf.Clamp(healthPoints, 0, maxHealth);
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}