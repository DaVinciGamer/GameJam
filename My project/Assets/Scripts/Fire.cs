using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject FireObject;
    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;
    public Sprite deletedFire;
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.LogWarning("Collision with Fire");
        Debug.LogWarning(playerController.PickupBucket + "PickupBucket STate");
        if (playerController.PickupBucket == false && playerController.BucketState == true)
        {
            Debug.LogWarning("Fire Delete Anforderungen Erfüllt");
            spriteRenderer.sprite = deletedFire;
        }
        
    }
}
