using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject FireObject;
    public GameObject Bucket;
    public PlayerController playerController;
    private Animator animator;

    // Relative boundaries to the position of FireObject
    public float xMin = -1f; // Left boundary relative to the position of FireObject
    public float xMax = 1f; // Right boundary relative to the position of FireObject
    public float yMin = -1f; // Lower boundary relative to the position of FireObject
    public float yMax = 1f; // Upper boundary relative to the position of FireObject

    private bool isBucketInArea = false;
    private Vector2 firePosition;
    [SerializeField] private GameObject winCanvas;
    private MusicController MusicController;

    void Start()
    {
        // Initialize the position of FireObject
        if (FireObject != null)
        {
            firePosition = FireObject.transform.position;

            // Initialize the animator of FireObject
            animator = FireObject.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component is missing on FireObject.");
            }
        }
        else
        {
            Debug.LogError("FireObject is not assigned");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Bucket != null)
        {
            Vector2 bucketPosition = Bucket.transform.position;

            // Check the relative position of the bucket to the stored position of FireObject
            if (bucketPosition.x >= firePosition.x + xMin && bucketPosition.x <= firePosition.x + xMax &&
                bucketPosition.y >= firePosition.y + yMin && bucketPosition.y <= firePosition.y + yMax)
            {
                //Debug.Log("Bucket is near FireObject");
                if (playerController != null)
                {
                    if (playerController.PickupBucket == false && playerController.BucketState == true) // BucketState = true --> Water in Bucket
                    {
                        Debug.Log("PickupBucket is false and BucketState is true");
                        if (animator != null)
                        {
                            animator.SetBool("Fire", false); // Play Ash Sprite Animation
                            Debug.Log("Fire extinguished");
                            StartCoroutine(WaitAndExecute(2.0f));
                        }
                        else
                        {
                            Debug.LogError("Animator is not assigned");
                        }
                    }
                }
                else
                {
                    Debug.LogError("PlayerController is not assigned");
                }
            }
        }
        else
        {
            Debug.LogError("Bucket is not assigned");
        }
    }
    private void ShowWinPanel()
    {
        winCanvas.SetActive(true);
    }

    IEnumerator WaitAndExecute(float waitTime)
    {
        MusicController.Instance.FadeTo(4);
        yield return new WaitForSeconds(waitTime);
        ShowWinPanel();
        Time.timeScale = 0; // Stop Game
        Debug.Log("You have won!");
    }
}
