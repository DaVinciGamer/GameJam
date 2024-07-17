using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject FireObject;
    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;
    public Sprite deletedFire;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();

        // Get the SpriteRenderer from the FireObject
        if (FireObject != null)
        {
            animator = FireObject.GetComponent<Animator>();
            spriteRenderer = FireObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("No SpriteRenderer found on the FireObject");
            }
        }
        else
        {
            Debug.LogError("FireObject is not assigned");
        }
        spriteRenderer.sprite = deletedFire;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (playerController != null)
        {
            if (playerController.PickupBucket == false && playerController.BucketState == true)
            {
                Debug.Log("PickupBucket is false and BucketState is true");
                if (spriteRenderer != null)
                {
                    animator.SetBool("Fire", false);
                    //spriteRenderer.sprite = deletedFire;
                    Debug.Log("Deleted Fire");
                }
                else
                {
                    Debug.LogError("SpriteRenderer is not assigned");
                }
            }
        }
    }
}
