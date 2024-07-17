using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawn : MonoBehaviour
{
    [Range(0, 100)] // zwischen 0% und 100% 
    public int iniChance; //initial chance of the tiles being alive

    [Range(1, 8)]
    public int birthLimit;

    [Range(1, 8)]
    public int deathLimit;

    [Range(1, 10)]
    public int numR; //number of repetition for the algorithm 

    private int[,] terrainMap;

    public Vector3Int tmapSize;
    public Vector3Int randVectorRa; //random Vector zum füllen der tiles
    //public Vector3Int randVectorBi;
    //public Vector3Int randVectorBu;


    public Tilemap monsterMap; //z.B. Grass
    public Tilemap waterMap;
    //public Tilemap botMap; //z.B. Stone
    public AnimatedTile rabbit;
    public AnimatedTile butterfly;
    public AnimatedTile bird;

    int width; //width of the Map
    int height; //height of the Map


    int runs;
    private bool childscene;
    public static bool isfilled;


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
                randVectorRa = new Vector3Int(-x + Random.Range(width / 4, width / 2), -y + Random.Range(height / 4, height / 2), 0);
                tileFilled(waterMap, randVectorRa);
                //Debug.Log("Random Vector werte: "+randVectorRa.x +", "+randVectorRa.y+", "+randVectorRa.z);
                if (terrainMap[x, y] == 1 && !isfilled)
                {
                    //Debug.Log("Anamalen des Tiles mit isFilled: " + isfilled);
                    int random = Random.Range(0, 3);
                    if (random == 1)
                    {
                        monsterMap.SetTile(randVectorRa, butterfly);
                    }
                    else if (random == 2)
                    {
                        monsterMap.SetTile(randVectorRa, bird);
                    }
                    else
                    {
                        monsterMap.SetTile(randVectorRa, rabbit);
                    }


                    //randVectorBi = new Vector3Int(-x + Random.Range(width / 4, width / 2), -y + Random.Range(height / 4, height / 2), 0);
                    //randVectorBu = new Vector3Int(-x + Random.Range(width / 4, width / 2), -y + Random.Range(height / 4, height / 2), 0);
                    /*if (isfilled == false)
                    {
                        monsterMap.SetTile(randVectorRa, rabbit);
                    }
                    if (tileFilled(waterMap, randVectorBu) == false)
                    {
                        monsterMap.SetTile(randVectorBu, butterfly);
                    }
                    if (tileFilled(waterMap, randVectorBi) == false)
                    {
                        monsterMap.SetTile(randVectorBi, bird);
                    }*/
                    //new Vector3Int(-x + Random.Range(width / 4, width/2), -y+Random.Range(height/4,height/2), 0),rabbit);//-x + width / 2, -y + height / 2, 0), rabbit);
                    //monsterMap.SetTile(new Vector3Int(-x + Random.Range(width / 4, width/2), -y + Random.Range(height / 4, height/2), 0),bird);//-x + width / 4, -y + height / 4, 0), bird);
                    //((monsterMap.SetTile(new Vector3Int(-x + Random.Range(width / 4, width/2), -y + Random.Range(height / 4, height/2), 0),butterfly);//-x + width / 3, -y + height / 3, 0), butterfly);
                }
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
                    if (neighb < deathLimit)
                    {
                        newMap[x, y] = 0;
                    }

                    else
                    {
                        newMap[x, y] = 1;
                    }
                }

                if (oldMap[x, y] == 0)
                {
                    if (neighb > birthLimit)
                    {
                        newMap[x, y] = 1;
                    }

                    else
                    {
                        newMap[x, y] = 0;
                    }
                }

            }

        }

        return newMap;
    }

    private void Start()
    {
        //doSim(numR);
        runs = 1;
        //isfilled = false;
    }

    void Update()
    {
        childscene = TileAutomator.childsceneloaded;
        if(childscene == true&&runs<2)
        {
            doSim(numR);
            runs++;
        }
    }

    public void tileFilled(Tilemap tilemap, Vector3Int vec)
    {
        //Debug.Log("In Methode TileFilled");
        //waterMap = tilemap;

        if (tilemap.HasTile(vec))
        {
            //Debug.Log("Tilemap has tile an position: " + vec.x + ", " + vec.y + ", " + vec.z);
            isfilled = true;
            //Debug.Log("Sprite used:" + tilemap.GetSprite(vec).name);
            //Debug.Log("isFilled: " + isfilled);
        }
        else
        {
            isfilled = false;
        }

    }

    public void clearMap(bool complete)
    {
        monsterMap.ClearAllTiles();
        if (complete)
        {
            terrainMap = null;
        }
    }
}
