using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject FireObject; // Das Objekt, das das Feuer repräsentiert
    public GameObject Bucket; // Der Eimer
    public PlayerController playerController;
    private Animator animator;

    // Relative Grenzen zur Position des FireObject
    public float xMin = -1f; // Linke Grenze relativ zur Position von FireObject
    public float xMax = 1f; // Rechte Grenze relativ zur Position von FireObject
    public float yMin = -1f; // Untere Grenze relativ zur Position von FireObject
    public float yMax = 1f; // Obere Grenze relativ zur Position von FireObject

    private bool isBucketInArea = false; // Zustand, ob der Eimer im Bereich ist
    private Vector2 firePosition; // Position des FireObject

    void Start()
    {
        // Position des FireObject initialisieren
        if (FireObject != null)
        {
            firePosition = FireObject.transform.position;

            // Animator vom FireObject initialisieren
            animator = FireObject.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator-Komponente fehlt an FireObject.");
            }
        }
        else
        {
            Debug.LogError("FireObject ist nicht zugewiesen");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Bucket != null)
        {
            Vector2 bucketPosition = Bucket.transform.position;

            // Überprüfung der relativen Position des Buckets zur gespeicherten Position des FireObject
            if (bucketPosition.x >= firePosition.x + xMin && bucketPosition.x <= firePosition.x + xMax &&
                bucketPosition.y >= firePosition.y + yMin && bucketPosition.y <= firePosition.y + yMax)
            {
                //Debug.Log("Bucket ist in der Nähe von FireObject");
                if (playerController != null)
                {
                    if (playerController.PickupBucket == false && playerController.BucketState == true)
                    {
                        Debug.Log("PickupBucket ist false und BucketState ist true");
                        if (animator != null)
                        {
                            animator.SetBool("Fire", false);
                            Debug.Log("Feuer gelöscht");
                        }
                        else
                        {
                            Debug.LogError("Animator ist nicht zugewiesen");
                        }
                    }
                }
                else
                {
                    Debug.LogError("PlayerController ist nicht zugewiesen");
                }
            }
        }
        else
        {
            Debug.LogError("Bucket ist nicht zugewiesen");
        }
    }
}
