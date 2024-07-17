using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSize : MonoBehaviour
{
    int width; //width of the Map
    int height; //height of the Map
    private int[,] terrainMap;

    public Vector3Int tmapSize;

    // Start is called before the first frame update
    void Start()
    {
        width = tmapSize.x;
        height = tmapSize.y;
        terrainMap = new int[width, height];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
