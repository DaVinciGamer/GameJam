using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damageAmount = 1f; // Damage amount the projectile deals
    public Rigidbody2D rb; // Reference to the Rigidbody2D component
    public float manualRotationOffset = 0f; // Additional rotation offset to adjust the projectile's angle

    void Start()
    {
        // Rotate the projectile based on its initial velocity and manual offset
        RotateProjectile();
    }

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

    void RotateProjectile()
    {
        // Calculate the angle based on the velocity of the projectile
        Vector2 direction = rb.velocity;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the manual rotation offset
        angle += manualRotationOffset;

        // Rotate the projectile to face the direction it's moving
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
