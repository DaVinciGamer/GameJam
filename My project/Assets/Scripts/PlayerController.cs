using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

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

    private float originalMoveSpeed;
    private GameObject carriedObject = null;

    // Start is called before the first frame update
    void Start()
    {
        LeftAction.Enable();
        RightAction.Enable();
        UpAction.Enable();
        DownAction.Enable();
        AttackAction.Enable();
        PickUpAction.Enable();

        originalMoveSpeed = moveSpeed;

        // AttackAction auf das performed Event abonnieren
        AttackAction.performed += _ => FireProjectile();

        // PickUpAction auf das performed Event abonnieren
        PickUpAction.performed += _ => TogglePickUp();
    }

    // Update is called once per frame
    void Update()
    {
        // Bewegungscode
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
        position.x = position.x + 0.1f * horizontal;
        position.y = position.y + 0.1f * vertical;
        transform.position = position;

        // Mitnehmen des Objekts
        if (carriedObject != null)
        {
            carriedObject.transform.position = transform.position;
        }
    }

    void FireProjectile()
    {
        // Mausposition ermitteln
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0; // Z-Achse auf 0 setzen, da wir uns im 2D-Raum befinden

        // Projektil erzeugen
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Richtung vom Spieler zur Mausposition berechnen
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Projektil bewegen (dies kann auf verschiedene Arten geschehen, hier ist ein Beispiel)
        projectile.GetComponent<Rigidbody2D>().velocity = direction * 10f; // Geschwindigkeit anpassen
    }

    void TogglePickUp()
    {
        if (carriedObject != null)
        {
            // Objekt fallen lassen
            carriedObject = null;
        }
        else
        {
            // Objekt aufheben
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
            foreach (var collider in colliders)
            {
                if (collider.gameObject != this.gameObject && collider.gameObject.tag == "Pickup")
                {
                    carriedObject = collider.gameObject;
                    break;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Zeichne eine kleine Kugel, um den Aufhebebereich zu visualisieren
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
