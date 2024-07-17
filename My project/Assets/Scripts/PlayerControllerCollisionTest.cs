using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerControllerCollisionTest : MonoBehaviour
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
    public int currentHealth = 100;
    public int maxHealth = 100;
    public Image healthBar;
    public float pickUpRadius = 1.5f; // Radius of the player for picking up objects
    private GameObject carriedObject = null; // Declare carried object instance variable

    // Reference to VarInvertedWorld component
    private VarInvertedWorld varInvertedWorld;
    // New public variable for the alternate sprite
    public Sprite normalSprite;
    public Sprite invertedSprite;
    // Reference to the SpriteRenderer component
    private SpriteRenderer spriteRenderer;

    public Animator animator;


    public float jumpHeight = 5f;
    public float jumpDuration = 1f;

    private bool jumpingState = false;
    private Vector2 lastDirection;
    private Rigidbody2D rb;

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


        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 move = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.Space) && !jumpingState)
        {
            StartCoroutine(Jump());
        }

        // Check the value of the invertedWorld string
        if (varInvertedWorld != null)
        {
            if (VarInvertedWorld.invertedWorld == "true")
            {
                animator.SetBool("LeftInv", false);
                animator.SetBool("RightInv", false);
                animator.SetBool("UpInv", false);
                animator.SetBool("DownInv", false);
                // Debug.Log("VarInvertedWorld = true");
                if (LeftAction.IsPressed())
                {
                    animator.SetBool("Left", true);
                    animator.SetBool("Right", false);
                    animator.SetBool("Up", false);
                    animator.SetBool("Down", false);
                    move.x = -1f;
                    lastDirection = Vector2.left; // last direction is relevant to know where to jump
                }
                else if (RightAction.IsPressed())
                {
                    animator.SetBool("Left", false);
                    animator.SetBool("Right", true);
                    animator.SetBool("Up", false);
                    animator.SetBool("Down", false);
                    move.x = 1f;
                    lastDirection = Vector2.right;
                }

                if (UpAction.IsPressed())
                {
                    animator.SetBool("Left", false);
                    animator.SetBool("Right", false);
                    animator.SetBool("Up", true);
                    animator.SetBool("Down", false);
                    move.y = 1f;
                    lastDirection = Vector2.up;
                }
                else if (DownAction.IsPressed())
                {
                    animator.SetBool("Left", false);
                    animator.SetBool("Right", false);
                    animator.SetBool("Up", false);
                    animator.SetBool("Down", true);
                    move.y = -1f;
                    lastDirection = Vector2.down; 
                }

                // Set the normal sprite
                if (spriteRenderer != null && spriteRenderer.sprite != normalSprite)
                {
                    spriteRenderer.sprite = normalSprite;
                }
            }
            else if (VarInvertedWorld.invertedWorld == "false")
            {
                animator.SetBool("Left", false);
                animator.SetBool("Right", false);
                animator.SetBool("Up", false);
                animator.SetBool("Down", false);
                // Debug.Log("VarInvertedWorld = false");
                if (LeftAction.IsPressed())
                {
                    animator.SetBool("LeftInv", false);
                    animator.SetBool("RightInv", true);
                    animator.SetBool("UpInv", false);
                    animator.SetBool("DownInv", false);
                    move.x = 1f;
                    lastDirection = Vector2.right;
                }
                else if (RightAction.IsPressed())
                {
                    animator.SetBool("LeftInv", true);
                    animator.SetBool("RightInv", false);
                    animator.SetBool("UpInv", false);
                    animator.SetBool("DownInv", false);
                    move.x = -1f;
                    lastDirection = Vector2.left;
                }

                if (UpAction.IsPressed())
                {
                    animator.SetBool("LeftInv", false);
                    animator.SetBool("RightInv", false);
                    animator.SetBool("UpInv", false);
                    animator.SetBool("DownInv", true);
                    move.y = -1f;
                    lastDirection = Vector2.down;
                }
                else if (DownAction.IsPressed())
                {
                    animator.SetBool("LeftInv", false);
                    animator.SetBool("RightInv", false);
                    animator.SetBool("UpInv", true);
                    animator.SetBool("DownInv", false);
                    move.y = 1f;
                    lastDirection = Vector2.up;
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


    private IEnumerator Jump()
    {
        jumpingState = true;
        //animator.SetBool("Jumping", true);

        Vector2 jumpTarget = (Vector2)transform.position + lastDirection * jumpHeight;
        Vector2 startPosition = transform.position;

        float elapsedTime = 0f;
        while (elapsedTime < jumpDuration)
        {
            transform.position = Vector2.Lerp(startPosition, jumpTarget, (elapsedTime / jumpDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = jumpTarget;
        jumpingState = false;
        //animator.SetBool("Jumping", false);

        CheckLandingCollision();
    }

    private void CheckLandingCollision()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("River"))
            {
                Debug.LogWarning("River");
            }
            else if (collider.CompareTag("Crocodile"))
            {
                Debug.LogWarning("Croco");
            }
            else if (collider.CompareTag("Log"))
            {
                Debug.LogWarning("Log");
            }
        }
    }

}