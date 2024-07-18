using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject FireObject;
    public GameObject Bucket;
    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;
    public Sprite deletedFire;
<<<<<<< Updated upstream
     
=======
    private Animator animator;

    public float xMin = -10f; // Left Border of bucket position
    public float xMax = 10f; // Right Border of bucket position
    public float yMin = -5f; // Down Border of bucket position
    public float yMax = 5f; // Upper Border of bucket position

    private bool isBucketInArea = false; // Zustand, ob der Eimer im Bereich ist

>>>>>>> Stashed changes
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
<<<<<<< Updated upstream
=======
    {
        if (Bucket != null)
        {
            Vector2 position = Bucket.transform.position;

            bool isInArea = position.x >= xMin && position.x <= xMax && position.y >= yMin && position.y <= yMax;

            if (isInArea && !isBucketInArea)
            {
                Debug.LogWarning("Das Objekt befindet sich im Bereich");
                isBucketInArea = true;
                animator.SetBool("Fire", false);
                // Führe hier Aktionen aus, wenn sich das Objekt im Bereich befindet
            }
            else if (!isInArea && isBucketInArea)
            {
                Debug.Log("Das Objekt befindet sich außerhalb des Bereichs");
                isBucketInArea = false;
                // Führe hier Aktionen aus, wenn sich das Objekt außerhalb des Bereichs befindet
            }
        }
        else
        {
            Debug.LogError("Bucket ist nicht zugewiesen");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
>>>>>>> Stashed changes
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.LogWarning("Collision with Fire");
        Debug.LogWarning(playerController.PickupBucket + "PickupBucket STate");
        if (playerController.PickupBucket == false && playerController.BucketState == true)
        {
<<<<<<< Updated upstream
            Debug.LogWarning("Fire Delete Anforderungen Erfüllt");
            spriteRenderer.sprite = deletedFire;
=======
            if (playerController.PickupBucket == false && playerController.BucketState == true)
            {
                Debug.Log("PickupBucket is false and BucketState is true");
                if (spriteRenderer != null)
                {
                    
                    //spriteRenderer.sprite = deletedFire;
                    Debug.Log("Deleted Fire");
                }
                else
                {
                    Debug.LogError("SpriteRenderer is not assigned");
                }
            }
>>>>>>> Stashed changes
        }
        
    }
}
