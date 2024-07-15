using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System;

public class TileAutomator : MonoBehaviour
{
    [Range(0, 100)] // zwischen 0% und 100% 
    public int iniChance; //initial chance of the tiles being alive

    [Range(1, 8)] 
    public int birthLimit;

    [Range(1, 8)]
    public int deathLimit;

    [Range (1, 10)]
    public int numR; //number of repetition for the algorithm 
    private int count = 0;
    private int[,] terrainMap;
    public Vector3Int tmapSize;

    public Tilemap topMap; //z.B. Grass
    public Tilemap botMap; //z.B. Stone
    public RuleTile topTile;
    public Tile botTile;

    int width; //width of the Map
    int height; //height of the Map
    public void doSim(int nu)
    {
        clearMap(false);
        width = tmapSize.x;
        height = tmapSize.y;

        if (terrainMap == null)
        {
            terrainMap = new int[width, height];
            initPos();
        }


        for (int i = 0; i < nu; i++)
        {
            terrainMap = genTilePos(terrainMap);
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (terrainMap[x, y] == 1)
                    topMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), topTile);
                botMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), botTile);
            }
        }
    }

    public void initPos()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                terrainMap[x, y] = UnityEngine.Random.Range(1, 101) < iniChance ? 1 : 0;
            }
        }
    }


    public int[,] genTilePos(int[,] oldMap)
    {
        int[,] newMap = new int[width, height];
        int neighb;
        BoundsInt myB = new BoundsInt(-1, -1, 0, 3, 3, 1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                neighb = 0;
                foreach (var b in myB.allPositionsWithin)
                {
                    if (b.x == 0 && b.y == 0) continue;
                    if (x + b.x >= 0 && x + b.x < width && y + b.y >= 0 && y + b.y < height)
                    {
                        neighb += oldMap[x + b.x, y + b.y];
                    }
                    else
                    {
                        neighb++;
                    }
                }

                if (oldMap[x, y] == 1)
                {
                    if (neighb < deathLimit) newMap[x, y] = 0;

                    else
                    {
                        newMap[x, y] = 1;

                    }
                }

                if (oldMap[x, y] == 0)
                {
                    if (neighb > birthLimit) newMap[x, y] = 1;

                    else
                    {
                        newMap[x, y] = 0;
                    }
                }

            }

        }

        return newMap;
    }


    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            doSim(numR);
        }

        if (Input.GetMouseButtonDown(1))
        {
            clearMap(true);
        }

        if (Input.GetMouseButton(2))
        {
            SaveAssetMap();
            count++;
        }
    }


    public void SaveAssetMap()
    {
        string saveName = "tmapXY_" + count;
        var mf = GameObject.Find("Grid");

        if (mf)
        {
            var savePath = "Assets/" + saveName + ".prefab";
            if (PrefabUtility.CreatePrefab(savePath, mf))
            {
                EditorUtility.DisplayDialog("Tilemap saved", "Your Tilemap was saved under" + savePath, "Continue");
            }
            else
            {
                EditorUtility.DisplayDialog("Tilemap NOT saved", "An ERROR occured while trying to saveTilemap under" + savePath, "Continue");
            }
        }
    }

    public void clearMap(bool complete)
    {
        topMap.ClearAllTiles();
        botMap.ClearAllTiles();
        if (complete)
        {
            terrainMap = null;
        }
    }

    /*public void doSim(int numR) //do Simulation -> Main Funktion
    {
        clearMap(false); // dont start a new simulation
        width = tmapSize.x;
        height = tmapSize.y;

        if (terrainMap == null ) // neue TerrainMap erzeugen wenn diese null ist
        {
            terrainMap = new int[width,height];
            initPos();
        }

        for (int i = 0; i < numR; i++)
        {
            terrainMap = genTilePos(terrainMap);
        }

        for(int x = 0; x< width; x++)
        {
            for(int y = 0; y< height; y++)
            {
                if (terrainMap[x, y] == 1)
                {
                    topMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), topTile);
                    botMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), botTile);
                }
            }
        }
    }

    public int[,] genTilePos(int[,] oldMap)
    {
        int[,] newMap = new int[width,height]; // Output Array after processing the old Map
        int neighb; // anzahl der Nachbarn
        BoundsInt myB = new BoundsInt(-1,-1,0,3,3,1);

        for (int x = 0; x < width; x++) //itterieren druch die alte map
        {
            for (int y = 0; y < height; y++)
            {
                neighb = 0;

                foreach(var b in myB.allPositionsWithin)
                {
                    if (b.x == 0 && b.y == 0) continue;
                    if(x+b.x >= 0 && x+b.x < width && y+b.y >= 0 && y+b.y < height)
                    {
                        neighb += oldMap[x + b.x, y + b.y];
                    }
                    else
                    {
                        neighb++;
                    }
                }

                if (oldMap[x, y] == 1)
                {
                    if (neighb < deathLimit) newMap[x, y] = 0;
                    else
                    {
                        newMap[x, y] = 1;
                    }
                }
                if (oldMap[x, y] == 0)
                {
                    if (neighb > birthLimit) newMap[x, y] = 1;
                    else
                    {
                        newMap[x, y] = 0;
                    }
                }
            }
        }

        return newMap;
    }

    private void initPos() // Fill the Terrainmap with Random Tiles
    {
       for(int x = 0; x<width; x++)
        {
            for(int y = 0; y<height; y++)
            {
                terrainMap[x, y] = UnityEngine.Random.Range(1, 101) < iniChance ? 1 : 0; // 1 bedeutet tile ist alive 0 ist tile ist leer
            }
        }
    }

    private void clearMap(bool complete) //Do we want to start a new simulation?
    {
        topMap.ClearAllTiles();
        botMap.ClearAllTiles();

        if(complete)
        {
            terrainMap = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            doSim(numR);
        }
        if (Input.GetMouseButtonDown(1))
        {
            clearMap(true);
        }
    }*/
}
