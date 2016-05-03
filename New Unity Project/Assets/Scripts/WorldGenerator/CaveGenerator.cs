using UnityEngine;
using System.Collections;
using System;

public class CaveGenerator : MonoBehaviour {

    private int numberOfSmothings = 5;
    /*If more than wallExtensionConstant neighbouring squares are walls, make this square a wall.*/
    private int wallExtensionConstant = 4;
    /*If less than spaceExtensionConstant neighbouring squares are space, make this square a space.*/
    private int spaceExstensionConstant = 4;

    public GameObject prefab;

    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;
    private bool gizmosToDraw;


    [Range(0, 100)]
    public int randomFillPercent;

    int[,] map;

    void Start() {
        generateMap();
        
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F5)) {
            generateMap();
        }
    }

    void generateMap() {
        map = new int[width, height];
        randomFillMap();

        for (int i = 0; i < numberOfSmothings; i++){
            smoothMap();
        }


        drawBlocks();
    }

    void randomFillMap() {
       if (useRandomSeed){
            seed = System.DateTime.Now.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++) {
                /* Left border, right border, lower border, upper border*/
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1 || y == height -2) {
                    if (x == 0 || x == width - 1 || y == 0) map[x, y] = 1;
                    if (y == height - 1 ||y == height -2) map[x, y] = 0;
                }
                else { 
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void smoothMap() {
        int neighbourWallTiles;

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++){
                neighbourWallTiles = getSurroundingWallCount(x, y);

                if (neighbourWallTiles > wallExtensionConstant) {
                    map[x, y] = 1;
                }
                else if (neighbourWallTiles < spaceExstensionConstant) {
                    map[x, y] = 0;
                }
            }
        }
    }

    int getSurroundingWallCount(int gridX, int gridY) {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
                    if (neighbourX != gridX || neighbourY != gridY) {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }


    void drawBlocks() {
        if (map != null && prefab != null) {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if(map[x,y] == 1) {
                        Vector3 pos = new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, 0);
                        GameObject block = Instantiate(prefab, pos, Quaternion.identity) as GameObject;
                        block.transform.parent = this.transform;
                    }
                }
            }
        }
    }
}




