using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damageAmount = 1f; // Damage amount the projectile deals

    void Update()
    {
        // Check if the projectile is outside the camera's view
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Check if the projectile collides with an enemy
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // Deal damage to the enemy
                enemyHealth.TakeDamage(damageAmount);
            }

            // Destroy the projectile upon collision
            Destroy(gameObject);
        }
    }
}