using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFreezeRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rotation = transform.rotation;
        rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(rotation.eulerAngles);
    }
}
