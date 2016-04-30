using UnityEngine;
using System.Collections;
using System;

public class MapGenerator : MonoBehaviour {

    private int numberOfSmothings = 20;
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
        GenerateMap();
        DrawBlocks();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F5)) {
            GenerateMap();
        }
    }

    void GenerateMap() {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < numberOfSmothings; i++){
            SmoothMap();
        }
    }

    void RandomFillMap() {
       if (useRandomSeed){
            seed = Time.time.ToString();
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

    void SmoothMap() {
        int neighbourWallTiles;

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++){
                neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > wallExtensionConstant) {
                    map[x, y] = 1;
                }
                else if (neighbourWallTiles < spaceExstensionConstant) {
                    map[x, y] = 0;
                }
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY) {
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


    void DrawBlocks() {
        if (map != null && prefab != null) {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if(map[x,y] == 1) {
                        Vector3 pos = new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, -1);
                        Instantiate(prefab, pos, Quaternion.identity);
                    }
                }
            }
        }
    }

    void OnDrawGizmos() {/*
        if (map != null && gizmosToDraw) {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, -1);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }*/
    }
}




