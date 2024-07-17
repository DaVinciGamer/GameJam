using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BucketColliderHandler : MonoBehaviour
{
    private PlayerController playerController;

    void Start()
    {
        // Find the PlayerController script in the scene
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController component not found in the scene.");
        }
    }

    // Method called when the collider enters another trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is the WaterCollider
        if (other.gameObject == playerController.WaterCollider)
        {
            playerController.BucketState = true;
            Debug.LogWarning("Bucket collided with WaterCollider, BucketState set to true");
        }
    }
}
