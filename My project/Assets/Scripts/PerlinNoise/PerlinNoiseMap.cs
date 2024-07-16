using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseMap : MonoBehaviour
{
    Dictionary<int, GameObject> tileset;
    Dictionary<int, GameObject> tile_groups;
    public GameObject prefab_fog;
    //public GameObject prefab_gravel;
    //public GameObject prefab_terrace;

    int map_width = 120;
    int map_height = 80;

    List<List<int>> noise_grid = new List<List<int>>();
    List<List<GameObject>> tile_grid = new List<List<GameObject>>();

    float magnification = 15.0f; //ändert die flächen größen je größer die Zahl
                                 //desto breiter die Flächen je kleiner desto mehr
                                 //"Inseln" haben wir

    int x_offset = 20; // - bewegt tile links + bewegt tile rechts
    int y_offset = -10;// - bewegt tile runter + bewegt tile hoch
    void Start()
    {
        CreateTileset();
        CreateTileGroups();
        GenerateMap();
    }

    private void GenerateMap()
    {
        /** Generate a 2D grid using the Perlin noise fuction, storing it as
           both raw ID values and tile gameobjects **/

        for (int x =0; x< map_width; x++)
        {
            noise_grid.Add(new List<int>());
            tile_grid.Add(new List<GameObject>());

            for (int y =0; y< map_height; y++)
            {
                int tile_id = GetIdUsingPerlin(x,y);
                noise_grid[x].Add(tile_id);
                CreateTile(tile_id, x, y);
            }
        }
    }

    private void CreateTile(int tile_id, int x, int y)
    {
        /** Creates a new tile using the type id code, group it with common
            tiles, set it's position and store the gameobject. **/

        GameObject tile_prefab = tileset[tile_id];
        GameObject tile_group = tile_groups[tile_id];
        GameObject tile = Instantiate(tile_prefab, tile_group.transform);

        tile.name = string.Format("tile_x{0}_y{1}", x, y);
        tile.transform.localPosition = new Vector3(x, y, 0);

        tile_grid[x].Add(tile);
    }

    private int GetIdUsingPerlin(int x, int y)
    {
        /** Using a grid coordinate input, generate a Perlin noise value to be
            converted into a tile ID code. Rescale the normalised Perlin value
            to the number of tiles available. **/

        float raw_perlin = Mathf.PerlinNoise(
            (x - x_offset) / magnification,
            (y - y_offset) / magnification
        );

        float clamp_perlin = Mathf.Clamp(raw_perlin, 0.0f, 1.0f);
        float scaled_perlin = clamp_perlin * tileset.Count;

        if(scaled_perlin == 3)
        {
            scaled_perlin = 2;
        }

        return Mathf.FloorToInt(scaled_perlin);
    }

    void CreateTileset()
    {
        /** Collect and assign ID codes to the tile prefabs, for ease of access.
            Best ordered to match land elevation. **/
        tileset = new Dictionary<int, GameObject>();
        tileset.Add(0, prefab_fog);
        //tileset.Add(1, prefab_gravel);
        //tileset.Add(2, prefab_terrace);
    }

    void CreateTileGroups()
    {
        /** Create empty gameobjects for grouping tiles of the same type, ie
            forest tiles **/

        tile_groups = new Dictionary<int, GameObject>();
        foreach(KeyValuePair<int, GameObject> prefab_pair in tileset)
        {
            GameObject tile_group = new GameObject(prefab_pair.Value.name);
            tile_group.transform.parent = gameObject.transform;
            tile_group.transform.localPosition = new Vector3(0,0,0);
            tile_groups.Add(prefab_pair.Key, tile_group);
        }
    }
}
