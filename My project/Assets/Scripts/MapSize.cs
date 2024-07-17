using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSize : MonoBehaviour
{
    int width; //width of the Map
    int height; //height of the Map
    private int[,] terrainMap;

    public Vector3Int tmapSize;
    private Vector3Int tileVectors;
    public Tilemap watermap;
    public Tilemap logmap;

    public AnimatedTile logtile;

    public bool isfilled;

    // Start is called before the first frame update
    void Start()
    {
        width = tmapSize.x;
        height = tmapSize.y;
        terrainMap = new int[width, height];
    }

    public void tileFilled()//Tilemap tilemap, Vector3Int vec)
    {
        for(int x=0; x < width; x++)
        {
            for(int y=0; y<height; y++)
            {
                tileVectors = new Vector3Int(x, y, 0);

                if (watermap.HasTile(tileVectors))
                {
                    logmap.SetTile(tileVectors, logtile);
                }
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
