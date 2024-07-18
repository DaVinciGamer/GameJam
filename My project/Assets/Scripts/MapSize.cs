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
    private Vector3Int tileVectorspos;
    public Tilemap watermap;
    public Tilemap logmap;

    public AnimatedTile logtile;
    public AnimatedTile[] tileset;

    public bool isfilled;
    private Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        width = tmapSize.x;
        height = tmapSize.y;
        //terrainMap = new int[width, height];
        tileFilled();
    }

    public void tileFilled()//Tilemap tilemap, Vector3Int vec)
    {
        for(int x=0; x < width; x++)
        {
            for(int y=0; y<height; y++)
            {
                tileVectorspos = new Vector3Int(-x + width / 2, -y + height / 2, 0); //new Vector3Int(x, y, 0);
    
                if (watermap.HasTile(tileVectorspos)&&x%5==0)
                {
                    string spriteName = watermap.GetSprite(tileVectorspos).name;
                    //Debug.Log(spriteName);
                    if(spriteName == "Watermid" && x%5==0)//&&tileVectorspos.y==6)
                    {
                        if(tileVectorspos.y == 6)
                        {
                            //Debug.Log("In Watermid-if mit position: "+tileVectorspos.x+", "+tileVectorspos.y);
                            tileVectorspos.y = Random.Range(5, 8);
                            logmap.SetTile(tileVectorspos, logtile);
                        }


                    }
                    /*else if (spriteName == "Watermid" && tileVectorspos.y == 9)
                    {
                        Debug.Log("In Watermid-if mit position: " + tileVectorspos.x + ", " + tileVectorspos.y);
                        tileVectorspos.y = Random.Range(8, 10);
                        logmap.SetTile(tileVectorspos, logtile);

                    }
                    else if (spriteName == "Watermid" && tileVectorspos.y == 3)
                    {
                        tileVectorspos.y = Random.Range(2, 4);
                        Debug.Log("In Watermid-if mit position: " + tileVectorspos.x + ", " + tileVectorspos.y);
                        logmap.SetTile(tileVectorspos, logtile);
                    }
                    //tileset[y] = logtile;*/
                }
            }
        }
    }

    public void moveLog()
    {
        BoundsInt myB = logmap.cellBounds;
        TileBase[] allTiles = logmap.GetTilesBlock(myB);
        int count = 1;
        for (int x = 0; x < myB.size.x; x++)
        {
            for (int y = 0; y < myB.size.y; y++)
            {
                TileBase tile = allTiles[x + y * myB.size.x];
                if (tile != null)
                {
                    count = (int)(count + x * Time.deltaTime);
                    if (count < 59)
                    {
                        Vector3Int movex = new Vector3Int(count,y,0);
                        Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                        tile.RefreshTile(movex, logmap);
                        /*grid = logmap.layoutGrid;
                        grid.WorldToCell();
                        tile.transform*/
                        //logmap.RefreshTile(movex);
                    }
                    else
                    {
                        count = 1;
                    }
                }
                else
                {
                    //Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                }
            }
        }
    }
        
    // Update is called once per frame
    void Update()
    {
        //moveLog();
        /*if(tileset.Length!= 0)
        {
            for (int i = 0; i < tileset.Length; i++)
            {
                Vector3Int vec = new Vector3Int(tileVectorspos.x + i, tileVectorspos.y, 0);
                tileset[i].RefreshTile(vec, logmap);
            }
        }*/
        
    }
}
