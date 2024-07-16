using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //define Public variables
    public float moveSpeed = 2.0f;
    public InputAction LeftAction;
    public InputAction RightAction;
    public InputAction UpAction;
    public InputAction DownAction;
    public InputAction ShootAction;
    public InputAction PickUpAction;

    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileSpeed = 10.0f;
    public int score = 0;
    public int currentHealth = 100;
    public int maxHealth = 100;
    public Image healthBar;

    public TMP_Text scoreText;

    public float pickUpRadius = 1.5f; // Radius of the player for picking up objects
    private GameObject carriedObject = null; // Declare carried object instance variable

    // Reference to VarInvertedWorld component
    private VarInvertedWorld varInvertedWorld;
    // New public variable for the alternate sprite
    public Sprite normalSprite;
    public Sprite invertedSprite;
    // Reference to the SpriteRenderer component
    private SpriteRenderer spriteRenderer;

    void Start()
    {
// Find VarInvertedWorld component
        varInvertedWorld = FindObjectOfType<VarInvertedWorld>();

        if (varInvertedWorld == null)
        {
            Debug.LogError("VarInvertedWorld component not found in the scene.");
        }

        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer component found on this GameObject.");
        }

        // Activate Input Actions
        LeftAction.Enable();
        RightAction.Enable();
        UpAction.Enable();
        DownAction.Enable();
        ShootAction.Enable();
        PickUpAction.Enable();

        //Call TogglePickUp method when PickUpAction is executed
        PickUpAction.performed += _ => TogglePickUp();
        
        scoreText.text = "Score: " + score.ToString();

        
    }

    void Update()
    {
        Vector2 move = Vector2.zero;

        // Check the value of the invertedWorld string
        if (varInvertedWorld != null)
        {
            if (VarInvertedWorld.invertedWorld == "true")
            {
                Debug.Log("VarInvertedWorld = true");
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

                // Set the normal sprite
                if (spriteRenderer != null && spriteRenderer.sprite != normalSprite)
                {
                    spriteRenderer.sprite = normalSprite;
                }
            }
            else if (VarInvertedWorld.invertedWorld == "false")
            {
                Debug.Log("VarInvertedWorld = false");
                if (LeftAction.IsPressed())
                {
                    move.x = 1f;
                }
                else if (RightAction.IsPressed())
                {
                    move.x = -1f;
                }

                if (UpAction.IsPressed())
                {
                    move.y = -1f;
                }
                else if (DownAction.IsPressed())
                {
                    move.y = 1f;
                }
                // Set the inverted sprite
                if (spriteRenderer != null && spriteRenderer.sprite != invertedSprite)
                {
                    spriteRenderer.sprite = invertedSprite;
                }
            }
            else
            {
                Debug.LogError("VarInvertedWorld has an invalid value: " + VarInvertedWorld.invertedWorld);
            }
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

        // if carriedObject is not null, it is set to the current position of the player
        if (carriedObject != null)
        {
            carriedObject.transform.position = transform.position;
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

     
     // Method for picking up or dropping an object
    void TogglePickUp()
    {
        // If an object is currently being carried, it is dropped and carriedObject is set to zero
        if (carriedObject != null)
        {
            carriedObject = null;
            Debug.Log("Dropped object");
        }
        //When no object is worn
        else
        {
            // Search for Collider objects in the PickUpRadius
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickUpRadius);
            foreach (var collider in colliders)
            {
                // check whether Collider Object carries the tag pickup
                if (collider.gameObject != this.gameObject && collider.gameObject.CompareTag("Pickup"))
                {
                    //Debug message that displays the name of the object to be picked up
                    Debug.Log("Picking up object: " + collider.gameObject.name);
                    //Set found object as carriedObject so that the player carries it
                    carriedObject = collider.gameObject;
                    break;
                }
            }
        }
    }
    // draw Gizmo in Pick Up Radius Size
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickUpRadius);
    }
    
}