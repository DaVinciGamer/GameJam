using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //definieren der Public Variablen
    public float moveSpeed = 0.01f;  // anpassbare Bewegungsgeschwindigkeit
    // Aktion für die Bewegungen nach oben, unten, links und rechts
    public InputAction LeftAction;   
    public InputAction RightAction;
    public InputAction UpAction;
    public InputAction DownAction;
    // Aktion für Angriff und Aufheben und Fallenlassen von Objects
    public InputAction AttackAction;
    public InputAction PickUpAction;

    public GameObject projectilePrefab; // Referenz zum Projektil-Prefab
    public float pickUpRadius = 1.5f; // Radius des Players für das Aufheben von Objekten
    private GameObject carriedObject = null; // Carried Object Instanzvariable deklarieren

    void Start()
    {
        // Aktivieren der Eingabeaktionen
        LeftAction.Enable();
        RightAction.Enable();
        UpAction.Enable();
        DownAction.Enable();
        AttackAction.Enable();
        PickUpAction.Enable();
        //Fire Projectile Methode aufrufen, wenn AttacAction ausgeführt wird
        AttackAction.performed += _ => FireProjectile();
        //TogglePickUp Methode aufrufen, wenn PickUpAction ausgeführt wird
        PickUpAction.performed += _ => TogglePickUp();
    }

    void Update()
    {
        // Initialisierung der horizonthalen Bewegung
        float horizontal = 0.0f;
        // Abfragen, ob Left- oder Right Action aufgerufen wird falls ja Player nach L oder R bewegen
        if (LeftAction.IsPressed())
        {
            horizontal = -moveSpeed;
        }
        else if (RightAction.IsPressed())
        {
            horizontal = moveSpeed;
        }
        // Initialisierung der vertikalen Bewegung
        float vertical = 0.0f;
        // Abfragen, ob Up- oder Down Action aufgerufen wird falls ja Player nach U oder D bewegen
        if (UpAction.IsPressed())
        {
            vertical = moveSpeed;
        }
        else if (DownAction.IsPressed())
        {
            vertical = -moveSpeed;
        }

        // Berechnung der neuen Position basierend auf den Eingaben
        Vector2 position = transform.position; // Abfragen der aktuellen Position des Objects im Weltkoordinatensystem
        position.x += 0.1f * horizontal; // bewegungswert mit 0.1 skallieren und zur aktuellen X-Position addieren
        position.y += 0.1f * vertical;   // bewegungswert mit 0.1 skallieren und zur aktuellen Y-Position addieren
        transform.position = position;   // neu berechnete Position setzen

        // wenn carriedObject nicht null ist, wird es auf die aktuelle Position des Players gesetzt
        if (carriedObject != null)
        {
            carriedObject.transform.position = transform.position;
        }
    }

    // Methode um Projektil zu erstellen und richtung Mauszeiger zu feuern
    void FireProjectile()
    {
        // aktuelle Maus Position auf Bildschrim lesen und in Weltkoordinaten umwandeln, z=0 da 2D game
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;
        // Instanz des Projectil Objects an aktueller Player Position erstellen
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        // berechnen des Vektors von aktueller Position zu Mausposition und normalisieren
        Vector2 direction = (mousePosition - transform.position).normalized;
        // Geschwindikeit des Projektils setzen um es in Richtung der Maus zu schießen
        projectile.GetComponent<Rigidbody2D>().velocity = direction * 10f;
    }

    // Methode um Objekt aufzuheben oder fallen zu lassen
    void TogglePickUp()
    {
        // Wenn aktuell ein Object getragen wird, wird es fallengelassen und carriedObject auf null gesetzt
        if (carriedObject != null)
        {
            carriedObject = null;
        }
        // Wenn kein Object getragen wird
        else
        {
            // nach Collider objecten im PickUpRadius suchen
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickUpRadius);
            foreach (var collider in colliders)
            {
                // überprüfen, ob Collider Object den Tag Pickup trägt
                if (collider.gameObject != this.gameObject && collider.gameObject.tag == "Pickup")
                {
                    //Debug-Nachricht die den Namen des aufzuhebenden Objekts anzeigt
                    Debug.Log("Picking up object: " + collider.gameObject.name);
                    //Gefundenes Object als carriedObject setzen, damit der Player es trägt
                    carriedObject = collider.gameObject;
                    break;
                }
            }
        }
    }

    //Methode um benutzerdeffinierten Gizmo zu erstellen, der den PickupRadius darstellt
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickUpRadius);
    }
}
