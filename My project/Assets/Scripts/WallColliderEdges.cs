using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallColliderEdges : MonoBehaviour
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
        if (istriggered && Player.transform.position.x <= -42 && this.name=="Left")
        {
            Player.transform.position = new Vector3(-41.8f, Player.transform.position.y, 0);
        }
        if (istriggered && Player.transform.position.y <= -31.3 && this.name == "YourMom")
        {
            Player.transform.position = new Vector3(Player.transform.position.x, -31f , 0);
        }
        if (istriggered && Player.transform.position.x >= 52 && this.name == "Right")
        {
            Player.transform.position = new Vector3(51.8f, Player.transform.position.y, 0);
        }
        if (istriggered && Player.transform.position.y >= 35 && this.name == "Top")
        {
            Player.transform.position = new Vector3(-Player.transform.position.x, 34.9f, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        istriggered = true;
        currentPos = Player.transform.position;
        /*Debug.Log("##############################");
        Debug.Log("Spieler läuft Gegen die Wand!!!   " + currentPos);
        Debug.Log("##############################");*/
        /*if (currentPos.y>=3.8f)
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
