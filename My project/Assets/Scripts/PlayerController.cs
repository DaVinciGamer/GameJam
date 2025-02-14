using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
// ChatGPT used as a help
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
    public int currentHealth = 100;
    public int maxHealth = 100;
    public Image healthBar;
    public float pickUpRadius = 1.5f; // Radius of the player for picking up objects
    private GameObject carriedObject = null; // Declare carried object instance variable

    // Reference to VarInvertedWorld component
    private VarInvertedWorld varInvertedWorld;
    private MusicController MusicController;
    // New public variable for the alternate sprite
    public Sprite normalSprite;
    public Sprite invertedSprite;
    // Reference to the SpriteRenderer component
    private SpriteRenderer spriteRenderer;

    public Animator animator;

    // Bucket State Variable for Sprite switches between the worlds
    public bool BucketState = false; // True = Water inside
    public GameObject Bucket;
    private SpriteRenderer spriteRendererBucket;
    public Sprite brokenBucket;
    public Sprite normalBucket;
    public Sprite waterBucket;
    public bool PickupBucket;

    // Reference to the WaterCollider GameObject
    public GameObject WaterCollider;

    //Jumping
    public float jumpHeight = 5f;
    public float jumpDuration = 1f;
    private bool jumpingState = false;
    private Vector2 lastDirection;
    public PlayerClass playerClass;
    public SpriteAnimatorController[] spriteAnimators;

    private bool dead = false;
    public static bool isJumping;
    private int shootCounter = 0;
    void Start()
    {
        isJumping = false;
        // Find VarInvertedWorld component
        varInvertedWorld = FindObjectOfType<VarInvertedWorld>();
        playerClass = FindObjectOfType<PlayerClass>();

        if (varInvertedWorld == null)
        {
            //Debug.LogError("VarInvertedWorld component not found in the scene.");
        }

        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            //Debug.LogError("No SpriteRenderer component found on this GameObject.");
        }

        // Get the Bucket SpriteRenderer Component
        spriteRendererBucket = Bucket.GetComponent<SpriteRenderer>();
        if (spriteRendererBucket == null)
        {
            // Debug.LogError("No SpriteRenderer component found on Bucket.");
        }
        else { Debug.Log("Bucket Sprite gefunden"); }

        // Get the Animator component
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            //Debug.LogError("No Animator component found on this GameObject.");
        }

        // Activate Input Actions
        LeftAction.Enable();
        RightAction.Enable();
        UpAction.Enable();
        DownAction.Enable();
        ShootAction.Enable();
        PickUpAction.Enable();

        // Call TogglePickUp method when PickUpAction is executed
        PickUpAction.performed += _ => TogglePickUp();

        PickupBucket = false;
    }


    void Update()
    {
        Vector2 move = Vector2.zero;
        if (!dead)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !jumpingState)
            {
                StartCoroutine(Jump());
            }

            // Check the value of the invertedWorld string
            if (varInvertedWorld != null)
            {
                bool isInvertedWorld = VarInvertedWorld.invertedWorld == "true";

                if (!IsAnyDirectionActionPressed() && !jumpingState)
                {
                    if (isInvertedWorld)
                    {
                        animator.SetBool("IDLEInv", true);
                        animator.SetBool("IDLE", false);
                    }
                    else
                    {
                        animator.SetBool("IDLE", true);
                        animator.SetBool("IDLEInv", false);
                    }
                }
                else
                {
                    animator.SetBool("IDLE", false);
                    animator.SetBool("IDLEInv", false);
                }

                if (!isInvertedWorld)
                {
                    //Debug.LogWarning("VarInvertredWorld False");

                    // Set bucket Sprite to not broken
                    if (BucketState == false)
                    {
                        spriteRendererBucket.sprite = normalBucket;
                    }

                    // Reset inverted world animations
                    animator.SetBool("LeftInv", false);
                    animator.SetBool("RightInv", false);
                    animator.SetBool("UpInv", false);
                    animator.SetBool("DownInv", false);
                    animator.SetBool("JumpL", false); // Reset JumpL animation

                    // Handle left action
                    if (LeftAction.IsPressed())
                    {
                        animator.SetBool("Left", true);
                        animator.SetBool("Right", false);
                        animator.SetBool("Up", false);
                        animator.SetBool("Down", false);
                        move.x = -1f;
                        lastDirection = Vector2.left;
                        if (jumpingState == true)
                        {
                            animator.SetBool("Left", false);
                            animator.SetBool("JumpL", true);
                        }
                    }
                    else
                    {
                        animator.SetBool("Left", false);
                        animator.SetBool("JumpL", false);
                    }

                    // Handle right action
                    if (RightAction.IsPressed())
                    {
                        animator.SetBool("Left", false);
                        animator.SetBool("Right", true);
                        animator.SetBool("Up", false);
                        animator.SetBool("Down", false);
                        move.x = 1f;
                        lastDirection = Vector2.right;
                        if (jumpingState == true)
                        {
                            animator.SetBool("Right", false);
                            animator.SetBool("JumpR", true);
                        }
                    }
                    else
                    {
                        animator.SetBool("Right", false);
                        animator.SetBool("JumpR", false);
                    }

                    // Handle up action
                    if (UpAction.IsPressed())
                    {
                        animator.SetBool("Left", false);
                        animator.SetBool("Right", false);
                        animator.SetBool("Up", true);
                        animator.SetBool("Down", false);
                        move.y = 1f;
                        lastDirection = Vector2.up;
                        if (jumpingState == true)
                        {
                            animator.SetBool("Up", false);
                            animator.SetBool("JumpU", true);
                        }
                    }
                    else
                    {
                        animator.SetBool("Up", false);
                        animator.SetBool("JumpU", false);
                    }

                    // Handle down action
                    if (DownAction.IsPressed())
                    {
                        animator.SetBool("Left", false);
                        animator.SetBool("Right", false);
                        animator.SetBool("Up", false);
                        animator.SetBool("Down", true);
                        move.y = -1f;
                        lastDirection = Vector2.down;
                        if (jumpingState == true)
                        {
                            animator.SetBool("Down", false);
                            animator.SetBool("JumpD", true);
                        }
                    }
                    else
                    {
                        animator.SetBool("Down", false);
                        animator.SetBool("JumpD", false);
                    }

                    // Set the normal sprite
                    if (spriteRenderer != null && spriteRenderer.sprite != normalSprite)
                    {
                        spriteRenderer.sprite = normalSprite;
                    }
                }
                else
                {
                    //Debug.LogWarning("VarInvertredWorld True");

                    // Set bucket Sprite to not broken
                    if (BucketState == false)
                    {
                        spriteRendererBucket.sprite = brokenBucket;
                    }

                    // Reset normal world animations
                    animator.SetBool("Left", false);
                    animator.SetBool("Right", false);
                    animator.SetBool("Up", false);
                    animator.SetBool("Down", false);
                    animator.SetBool("JumpL", false); // Reset JumpL animation

                    // Handle left action in inverted world
                    if (LeftAction.IsPressed())
                    {
                        animator.SetBool("LeftInv", false);
                        animator.SetBool("RightInv", true);
                        animator.SetBool("UpInv", false);
                        animator.SetBool("DownInv", false);
                        move.x = 1f;
                        lastDirection = Vector2.right;
                        if (jumpingState == true)
                        {
                            animator.SetBool("RightInv", false);
                            animator.SetBool("JumpRInv", true);
                        }
                    }
                    else
                    {
                        animator.SetBool("RightInv", false);
                        animator.SetBool("JumpRInv", false);
                    }

                    // Handle right action in inverted world
                    if (RightAction.IsPressed())
                    {
                        animator.SetBool("LeftInv", true);
                        animator.SetBool("RightInv", false);
                        animator.SetBool("UpInv", false);
                        animator.SetBool("DownInv", false);
                        move.x = -1f;
                        lastDirection = Vector2.left;
                        if (jumpingState == true)
                        {
                            animator.SetBool("LeftInv", false);
                            animator.SetBool("JumpLInv", true);
                        }
                    }
                    else
                    {
                        animator.SetBool("LeftInv", false);
                        animator.SetBool("JumpLInv", false);
                    }

                    // Handle up action in inverted world
                    if (UpAction.IsPressed())
                    {
                        animator.SetBool("LeftInv", false);
                        animator.SetBool("RightInv", false);
                        animator.SetBool("UpInv", false);
                        animator.SetBool("DownInv", true);
                        move.y = -1f;
                        lastDirection = Vector2.down;
                        if (jumpingState == true)
                        {
                            animator.SetBool("DownInv", false);
                            animator.SetBool("JumpDInv", true);
                        }
                    }
                    else
                    {
                        animator.SetBool("DownInv", false);
                        animator.SetBool("JumpDInv", false);
                    }

                    // Handle down action in inverted world
                    if (DownAction.IsPressed())
                    {
                        animator.SetBool("LeftInv", false);
                        animator.SetBool("RightInv", false);
                        animator.SetBool("UpInv", true);
                        animator.SetBool("DownInv", false);
                        move.y = 1f;
                        lastDirection = Vector2.up;
                        if (jumpingState == true)
                        {
                            animator.SetBool("UpInv", false);
                            animator.SetBool("JumpuInv", true);
                        }
                    }
                    else
                    {
                        animator.SetBool("UpInv", false);
                        animator.SetBool("JumpUInv", false);
                    }

                    // Set the inverted sprite
                    if (spriteRenderer != null && spriteRenderer.sprite != invertedSprite)
                    {
                        spriteRenderer.sprite = invertedSprite;
                    }
                }
                CheckCollision();
            }
        }

        // Normalize move vector
        if (move != Vector2.zero)
        {
            move.Normalize();
        }

        // Move the player
        float currentSpeed = moveSpeed;
        transform.position += (Vector3)move * currentSpeed * Time.deltaTime;

        // Shoot projectile if action is triggered
        if (ShootAction.triggered)
        {
            ShootProjectile();
            MusicController.Instance.PlaySoundEffect(1);
        }

        // Update position of carried object
        if (carriedObject != null)
        {
            carriedObject.transform.position = transform.position;
        }

        // Check BucketState and set spriteRendererBucket.sprite
        if (BucketState == true)
        {
            spriteRendererBucket.sprite = waterBucket;
        }
    }

    private bool IsAnyDirectionActionPressed()
    {
        return LeftAction.IsPressed() || RightAction.IsPressed() || UpAction.IsPressed() || DownAction.IsPressed();
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "WaterDispenser" && PickupBucket == true)
        {
            BucketState = true;
            //Debug.Log("BucketState gesetzt auf true.");
        }
    }

    void ShootProjectile() // With the help of ChatGPT
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0f;

        Vector3 direction = (mousePosition - projectileSpawnPoint.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * projectileSpeed;
        shootCounter += 1;
        if(shootCounter == 3){
            HideSpriteById(1);
        }
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

    public void heal(int healthPoints) // not used in game
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
            //Debug.LogWarning("Dropped object");
            PickupBucket = false;
        }
        //When no object is worn
        else
        {
            HideSpriteById(0);
            // Search for Collider objects in the PickUpRadius
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickUpRadius);
            foreach (var collider in colliders)
            {
                // check whether Collider Object carries the tag pickup
                if (collider.gameObject != this.gameObject && collider.gameObject.CompareTag("Pickup"))
                {
                    //Debug message that displays the name of the object to be picked up
                    //Debug.Log("Picking up object: " + collider.gameObject.name);
                    PickupBucket = true;
                    //Set found object as carriedObject so that the player carries it
                    carriedObject = collider.gameObject;

                    break;
                }
            }
        }
    }
    // Method called when the collider enters another trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is the WaterCollider
        if (other.gameObject == WaterCollider)
        {
            BucketState = true;
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
        isJumping = true;
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
        isJumping = false;
        //animator.SetBool("Jumping", false);

        CheckCollision();
    }

    private void CheckCollision()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("River"))
            {
                Debug.LogWarning("River");
                animator.SetBool("LeftInv", false);
                animator.SetBool("RightInv", false);
                animator.SetBool("UpInv", false);
                animator.SetBool("DownInv", false);
                animator.SetBool("Left", false);
                animator.SetBool("Right", false);
                animator.SetBool("Up", false);
                animator.SetBool("Down", false);
                //animator.SetBool("Drown", true);
                if (!isJumping)
                {
                    animator.Play("Drown");
                    StartCoroutine(Delay());
                }


            }
            else if (collider.CompareTag("Log"))
            {
                Debug.LogWarning("Log");
            }
        }
    }

    private IEnumerator Delay()
    {
        Debug.LogWarning("Delay started");
        dead = true;
        yield return new WaitForSeconds(4);
        Debug.LogWarning("5 seconds have passed");
        playerClass.currentHealth = 0;
    }

    public void HideSpriteById(int id)
    {
        foreach (var animator in spriteAnimators)
        {
            animator.HideInstanceById(id);
        }
    }

}