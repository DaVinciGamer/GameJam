using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    Vector3 currentPos;
    private bool istriggered;
    void Start()
    {
        istriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.isJumping)
        {
            Debug.Log("Spieler springt");
            /*if (!istriggered && Player.transform.position.y > 9)
            {
                return;
            }*/
            if (istriggered && Player.transform.position.y <= 8.7f && Player.transform.position.y > 7)
            {
                Player.transform.position = new Vector3(Player.transform.position.x, 3.6f, 0);
            }
            if (istriggered && Player.transform.position.y >= 3.8 && Player.transform.position.y < 6)
            {
                Player.transform.position = new Vector3(Player.transform.position.x, 9.2f, 0);
            }
        }
        /*else
        {
            if (!istriggered && Player.transform.position.y > 9)
            {
                return;
            }
            if (istriggered && Player.transform.position.y <= 8.7f && Player.transform.position.y > 7)
            {
                Player.transform.position = new Vector3(Player.transform.position.x, 9.2f, 0);
            }
            if (istriggered && Player.transform.position.y >= 3.8 && Player.transform.position.y < 6)
            {

                Player.transform.position = new Vector3(Player.transform.position.x, 3.6f, 0);
            }
        }*/
        
        /*else if (istriggered && Player.transform.position.y >= 10)
        {
            istriggered = false;
            return;
        }*/
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        istriggered = true;
        /*currentPos = Player.transform.position;
        Debug.Log("##############################");
        Debug.Log("Spieler läuft auf strasse!!!   " + currentPos);
        Debug.Log("##############################");
        if (currentPos.y>=3.8f)
        {
            Debug.Log("##############################");
            Debug.Log("currentpos.y= "+currentPos);
            Debug.Log("##############################");
            Player.transform.position = new Vector3(currentPos.x,3.6f,0);
        }*/
        /*Debug.Log("##############################");
        Debug.Log("##############################");
        Debug.Log("Spieler läuft auf strasse!!!   "+currentPos);
        Debug.Log("##############################");
        Debug.Log("##############################");*/
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        istriggered = false;
    }
}
