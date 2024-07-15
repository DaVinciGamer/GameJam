using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerClass playerClass; // Stelle sicher, dass dies im Inspector gesetzt ist!

    void OnCollisionEnter(Collision collision)
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Enemy")
            {
                playerClass.currentHealth -= 10; // Oder jede andere Logik zur Schadensberechnung
                Debug.Log("--- OnTriggerEnter---");

                if (playerClass.currentHealth <= 0)
                {
                    Debug.Log("Player is dead!");
                }
            }
        }
    }
}
