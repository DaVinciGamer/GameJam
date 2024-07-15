using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 0.01f;
    public InputAction LeftAction;
    public InputAction RightAction;
    public InputAction UpAction;
    public InputAction DownAction;
    public InputAction AttackAction;
    public InputAction PickUpAction;

    public GameObject projectilePrefab; // Referenz zum Projektil-Prefab
    public float pickUpRadius = 1.5f; // Radius für das Aufheben von Objekten

    private float originalMoveSpeed;
    private GameObject carriedObject = null;

    void Start()
    {
        LeftAction.Enable();
        RightAction.Enable();
        UpAction.Enable();
        DownAction.Enable();
        AttackAction.Enable();
        PickUpAction.Enable();

        originalMoveSpeed = moveSpeed;

        AttackAction.performed += _ => FireProjectile();
        PickUpAction.performed += _ => TogglePickUp();
    }

    void Update()
    {
        float horizontal = 0.0f;
        if (LeftAction.IsPressed())
        {
            horizontal = -moveSpeed;
        }
        else if (RightAction.IsPressed())
        {
            horizontal = moveSpeed;
        }
        float vertical = 0.0f;
        if (UpAction.IsPressed())
        {
            vertical = moveSpeed;
        }
        else if (DownAction.IsPressed())
        {
            vertical = -moveSpeed;
        }

        Vector2 position = transform.position;
        position.x += 0.1f * horizontal;
        position.y += 0.1f * vertical;
        transform.position = position;

        if (carriedObject != null)
        {
            carriedObject.transform.position = transform.position;
        }
    }

    void FireProjectile()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Vector2 direction = (mousePosition - transform.position).normalized;

        projectile.GetComponent<Rigidbody2D>().velocity = direction * 10f;
    }

    void TogglePickUp()
    {
        Debug.Log("PickUpAction performed");
        if (carriedObject != null)
        {
            Debug.Log("Dropping carried object");
            carriedObject = null;
        }
        else
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickUpRadius);
            foreach (var collider in colliders)
            {
                if (collider.gameObject != this.gameObject && collider.gameObject.tag == "Pickup")
                {
                    Debug.Log("Picking up object: " + collider.gameObject.name);
                    carriedObject = collider.gameObject;
                    break;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickUpRadius);
    }
}
